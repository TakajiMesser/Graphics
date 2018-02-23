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

        private RenderManager _renderManager;
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

            _textureManager.EnableMipMapping = true;
            _textureManager.EnableAnisotropy = true;

            _renderManager = new RenderManager(window.Resolution);
            //_renderManager.Load(_brushes, _gameObjects);
            _window.Resized += (s, e) => _renderManager.ResizeTextures();

            _camera = map.Camera.ToCamera(window.Resolution);

            _gameObjectQuads = new QuadTree(0, map.Boundaries);
            _brushQuads = new QuadTree(0, map.Boundaries);
            _lightQuads = new QuadTree(0, map.Boundaries);
            _lightQuads.InsertRange(map.Lights.Select(l => new BoundingCircle(l)));
            _lights.AddRange(map.Lights);

            foreach (var mapBrush in map.Brushes)
            {
                var brush = mapBrush.ToBrush();
                //brush.Model.Mesh.Load(_renderManager._deferredRenderer._geometryProgram);

                if (brush.HasCollision)
                {
                    _brushQuads.Insert(brush.Bounds);
                }
                brush.AddPointLights(_lightQuads.Retrieve(brush.Bounds).Where(c => c.AttachedObject is PointLight).Select(c => (PointLight)c.AttachedObject));

                brush.Mesh.TextureMapping = mapBrush.TexturesPaths.ToTextureMapping(_textureManager);

                _brushes.Add(brush);
            }

            foreach (var mapObject in map.GameObjects)
            {
                var gameObject = mapObject.ToGameObject(_textureManager);
                //gameObject.Model.Mesh.Load(_renderManager._deferredRenderer._geometryProgram);

                switch (gameObject.Model)
                {
                    case SimpleModel s:
                        for (var i = 0; i < s.Meshes.Count; i++)
                        {
                            if (i < mapObject.TexturesPaths.Count)
                            {
                                s.Meshes[i].TextureMapping = mapObject.TexturesPaths[i].ToTextureMapping(_textureManager);
                            }
                        }
                        break;

                    case AnimatedModel a:
                        for (var i = 0; i < a.Meshes.Count; i++)
                        {
                            if (i < mapObject.TexturesPaths.Count)
                            {
                                a.Meshes[i].TextureMapping = mapObject.TexturesPaths[i].ToTextureMapping(_textureManager);
                            }
                        }
                        break;
                }

                if (gameObject.Name == map.Camera.AttachedGameObjectName)
                {
                    _camera.AttachToGameObject(gameObject, true, true);
                }
                
                _gameObjects.Add(gameObject);
            }

            _renderManager.Load(_brushes, _gameObjects, map.SkyboxTextureFilePaths);
        }

        public GameObject GetByName(string name) => _gameObjects.First(g => g.Name == name);

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
            _renderManager.RenderFrame(_textureManager, _camera, _lights, _brushes, _gameObjects);
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
