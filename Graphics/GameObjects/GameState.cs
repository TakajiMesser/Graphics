using Graphics.GameObjects;
using Graphics.Maps;
using Graphics.Physics.Collision;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphics.Rendering.Shaders;
using Graphics.Inputs;
using Graphics.Lighting;
using System.IO;
using Graphics.Helpers;
using Graphics.Rendering.PostProcessing;
using Graphics.Outputs;
using Graphics.Rendering.Processing;
using Graphics.Rendering.Textures;
using System.Drawing;
using System.Drawing.Imaging;

namespace Graphics.GameObjects
{
    public class GameState
    {
        private GameWindow _window;
        private InputState _inputState = new InputState();

        private ForwardRenderer _forwardRenderer;
        private SkyboxRenderer _skyboxRenderer;
        private DeferredRenderer _deferredRenderer;
        private ShadowRenderer _shadowRenderer;
        //private LightRenderer _lightRenderer;

        private List<PostProcess> _preProcesses = new List<PostProcess>();
        private List<PostProcess> _postProcesses = new List<PostProcess>();
        private TextureManager _textureManager = new TextureManager();

        private Camera _camera;
        private List<GameObject> _gameObjects = new List<GameObject>();
        private List<Brush> _brushes = new List<Brush>();
        private List<Light> _lights = new List<Light>();

        private QuadTree _gameObjectQuads;
        private QuadTree _brushQuads;
        private QuadTree _lightQuads;

        public GameState(Map map, GameWindow window)
        {
            _window = window;
            LoadPrograms();

            _window.Resized += (s, e) =>
            {
                _forwardRenderer.ResizeTextures();
                _deferredRenderer.ResizeTextures();
                _shadowRenderer.ResizeTextures();
                foreach (var process in _postProcesses)
                {
                    process.ResizeTextures();
                }
            };
            _camera = map.Camera.ToCamera(window.Resolution);

            _gameObjectQuads = new QuadTree(0, map.Boundaries);
            _brushQuads = new QuadTree(0, map.Boundaries);
            _lightQuads = new QuadTree(0, map.Boundaries);
            _lightQuads.InsertRange(map.Lights.Select(l => new BoundingCircle(l)));
            _lights.AddRange(map.Lights);

            for (var i = 0; i < map.Brushes.Count; i++)
            {
                var mapBrush = map.Brushes[i];
                var brush = mapBrush.ToBrush(_deferredRenderer._geometryProgram);

                if (brush.HasCollision)
                {
                    _brushQuads.Insert(brush.Bounds);
                }
                brush.AddPointLights(_lightQuads.Retrieve(brush.Bounds).Where(c => c.AttachedObject is PointLight).Select(c => (PointLight)c.AttachedObject));

                brush.TextureMapping = new TextureMapping()
                {
                    MainTextureID = !string.IsNullOrEmpty(mapBrush.TextureFilePath) ? _textureManager.AddTexture(mapBrush.TextureFilePath) : 0,
                    NormalMapID = !string.IsNullOrEmpty(mapBrush.NormalMapFilePath) ? _textureManager.AddTexture(mapBrush.NormalMapFilePath) : 0,
                    DiffuseMapID = !string.IsNullOrEmpty(mapBrush.DiffuseMapFilePath) ? _textureManager.AddTexture(mapBrush.DiffuseMapFilePath) : 0,
                    SpecularMapID = !string.IsNullOrEmpty(mapBrush.SpecularMapFilePath) ? _textureManager.AddTexture(mapBrush.SpecularMapFilePath) : 0
                };

                _brushes.Add(brush);
            }

            foreach (var mapObject in map.GameObjects)
            {
                var gameObject = mapObject.ToGameObject(_deferredRenderer._geometryProgram);

                gameObject.TextureMapping = new TextureMapping()
                {
                    MainTextureID = !string.IsNullOrEmpty(mapObject.TextureFilePath) ? _textureManager.AddTexture(mapObject.TextureFilePath) : 0,
                    NormalMapID = !string.IsNullOrEmpty(mapObject.NormalMapFilePath) ? _textureManager.AddTexture(mapObject.NormalMapFilePath) : 0,
                    DiffuseMapID = !string.IsNullOrEmpty(mapObject.DiffuseMapFilePath) ? _textureManager.AddTexture(mapObject.DiffuseMapFilePath) : 0,
                    SpecularMapID = !string.IsNullOrEmpty(mapObject.SpecularMapFilePath) ? _textureManager.AddTexture(mapObject.SpecularMapFilePath) : 0
                };

                if (gameObject.Name == map.Camera.AttachedGameObjectName)
                {
                    _camera.AttachToGameObject(gameObject, true, true);
                }
                
                _gameObjects.Add(gameObject);
            }
        }

        public GameObject GetByName(string name) => _gameObjects.First(g => g.Name == name);

        private void LoadPrograms()
        {
            _textureManager.EnableMipMapping = true;
            _textureManager.EnableAnisotropy = true;

            _forwardRenderer = new ForwardRenderer(_window.Resolution);
            _deferredRenderer = new DeferredRenderer(_window.Resolution);
            _shadowRenderer = new ShadowRenderer(_window.Resolution);
            //_lightRenderer = new LightRenderer(_window.Resolution);
            _skyboxRenderer = new SkyboxRenderer(_window.Resolution);
            
            _postProcesses.Add(new MotionBlur(_window.Resolution) { Enabled = false });
            _postProcesses.Add(new Blur(_window.Resolution) { Enabled = true });
            _postProcesses.Add(new InvertColors(_window.Resolution) { Enabled = false });
            _postProcesses.Add(new RenderToScreen(_window.Resolution) { Enabled = true });

            foreach (var process in _preProcesses)
            {
                process.Load();
            }

            _forwardRenderer.Load();
            _deferredRenderer.Load();
            _shadowRenderer.Load();
            //_lightRenderer.Load();
            _skyboxRenderer.Load();

            foreach (var process in _postProcesses)
            {
                process.Load();
            }
        }

        public void AddGameObject(GameObject gameObject)
        {
            if (string.IsNullOrEmpty(gameObject.Name)) throw new ArgumentException("GameObject must have a name defined");
            if (_gameObjects.Any(g => g.Name == gameObject.Name)) throw new ArgumentException("GameObject must have a unique name");

            _gameObjects.Add(gameObject);
        }

        public void Initialize()
        {
            foreach (var gameObject in _gameObjects)
            {
                gameObject.OnInitialization();
            }
        }

        public void HandleInput()
        {
            _camera.OnHandleInput(_inputState);

            foreach (var gameObject in _gameObjects)
            {
                gameObject.OnHandleInput(_inputState, _camera);
            }
        }

        public void UpdateFrame()
        {
            // Update the gameobject colliders every frame, since they could have moved
            _gameObjectQuads.Clear();
            _gameObjectQuads.InsertRange(_gameObjects.Select(g => g.Bounds).Where(c => c != null));

            // For each object that has a non-zero transform, we need to determine the set of colliders to compare it against for hit detection
            foreach (var gameObject in _gameObjects)
            {
                gameObject.ClearLights();
                gameObject.AddPointLights(_lightQuads.Retrieve(gameObject.Bounds)
                    .Where(c => c.AttachedObject is PointLight)
                    .Select(c => (PointLight)c.AttachedObject));

                var filteredColliders = _brushQuads.Retrieve(gameObject.Bounds)
                    .Concat(_gameObjectQuads
                        .Retrieve(gameObject.Bounds)
                            .Where(c => ((GameObject)c.AttachedObject).Name != gameObject.Name));

                gameObject.OnUpdateFrame(filteredColliders);
            }

            _camera.OnUpdateFrame();

            PollForInput();
        }

        public void RenderFrame()
        {
            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            //_forwardRenderer.Render(_textureManager, _camera, _brushes, _gameObjects);
            //_skyboxRenderer.Render(_camera, _forwardRenderer._frameBuffer);

            _deferredRenderer.GeometryPass(_textureManager, _camera, _brushes, _gameObjects);
            _shadowRenderer.Render(_camera, _lights, _brushes, _gameObjects);
            _deferredRenderer.LightPass(_camera, _lights, _shadowRenderer.PointDepthTexture, _shadowRenderer.SpotDepthTexture);
            _skyboxRenderer.Render(_camera, _deferredRenderer.GBuffer);

            // Read from GBuffer's final texture, so that we can post-process it
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, _deferredRenderer.GBuffer._handle);
            GL.ReadBuffer(ReadBufferMode.ColorAttachment6);
            var texture = _deferredRenderer.FinalTexture;

            GL.Disable(EnableCap.DepthTest);

            foreach (var process in _postProcesses.Where(p => p.Enabled))
            {
                if (process.GetType() == typeof(InvertColors))
                {
                    var invert = (InvertColors)process;
                    invert.Render(texture);
                    texture = invert.FinalTexture;
                }
                else if (process.GetType() == typeof(MotionBlur))
                {
                    var blur = (MotionBlur)process;
                    blur.Render(_deferredRenderer.VelocityTexture, _deferredRenderer.DepthStencilTexture, texture, 60.0f);
                    texture = blur.FinalTexture;
                }
                else if (process.GetType() == typeof(Blur))
                {
                    var blur = (Blur)process;
                    blur.Render(texture, _deferredRenderer.VelocityTexture, 60.0f);
                    texture = blur.FinalTexture;
                }
                else if (process.GetType() == typeof(RenderToScreen))
                {
                    var renderToScreen = (RenderToScreen)process;
                    renderToScreen.Render(texture);
                }
            }
        }

        private void PollForInput() => _inputState.UpdateState(Keyboard.GetState(), Mouse.GetState(), _window);

        public void SaveToFile(string path)
        {
            throw new NotImplementedException();
        }

        public static GameState LoadFromFile(string path)
        {
            throw new NotImplementedException();
        }
    }
}
