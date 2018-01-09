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

        private GeometryRenderer _geometryRenderer;
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
            _geometryRenderer = new GeometryRenderer(window.Resolution);
            _postProcesses.Add(new MotionBlur(window.Resolution));
            _postProcesses.Add(new InvertColors(window.Resolution) { Enabled = false });
            _postProcesses.Add(new RenderToScreen(window.Resolution));
            
            LoadPrograms();

            _window = window;

            _gameObjectQuads = new QuadTree(0, map.Boundaries);
            _brushQuads = new QuadTree(0, map.Boundaries);
            _lightQuads = new QuadTree(0, map.Boundaries);
            _lightQuads.InsertRange(map.Lights.Select(l => new BoundingCircle(l)));

            _camera = map.Camera.ToCamera(window.Width, window.Height);

            foreach (var brush in map.Brushes.Select(b => b.ToBrush(_geometryRenderer._geometryProgram)))
            {
                if (brush.HasCollision)
                {
                    _brushQuads.Insert(brush.Bounds);
                }
                brush.AddLights(_lightQuads.Retrieve(brush.Bounds).Select(c => (Light)c.AttachedObject));

                _brushes.Add(brush);
            }

            foreach (var mapObject in map.GameObjects)
            {
                var gameObject = mapObject.ToGameObject(_geometryRenderer._geometryProgram);

                gameObject.TextureMapping = new TextureMapping()
                {
                    MainTextureID = !string.IsNullOrEmpty(mapObject.TextureFilePath) ? _textureManager.AddTexture(mapObject.TextureFilePath) : 0,
                    NormalMapID = !string.IsNullOrEmpty(mapObject.NormalMapFilePath) ? _textureManager.AddTexture(mapObject.NormalMapFilePath) : 0
                };

                if (gameObject.Name == map.Camera.AttachedGameObjectName)
                {
                    _camera.AttachedObject = gameObject;
                }
                
                _gameObjects.Add(gameObject);
            }
        }

        public GameObject GetByName(string name) => _gameObjects.First(g => g.Name == name);

        public void UpdateAspectRatio(int width, int height) => _camera.UpdateAspectRatio(width, height);

        private void LoadPrograms()
        {
            foreach (var process in _preProcesses)
            {
                process.Load();
            }

            _geometryRenderer.Load();

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
                gameObject.AddLights(_lightQuads.Retrieve(gameObject.Bounds)
                    .Select(c => (Light)c.AttachedObject));

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
            //GL.Enable(EnableCap.CullFace);
            //GL.CullFace(CullFaceMode.Back);

            _geometryRenderer.Render(_textureManager, _camera, _brushes, _gameObjects);

            // Now, extract the final texture from the geometry renderer, so that we can pass it off to the post-processes
            var texture = _geometryRenderer.FinalTexture;

            GL.Disable(EnableCap.DepthTest);

            foreach (var process in _postProcesses.Where(p => p.Enabled))
            {
                if (process.GetType() == typeof(InvertColors))
                {
                    var invert = (InvertColors)process;
                    invert.Render(texture);
                    texture = invert.FinalTexture;
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
