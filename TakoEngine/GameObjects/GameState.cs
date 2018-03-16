using TakoEngine.GameObjects;
using TakoEngine.Maps;
using TakoEngine.Physics.Collision;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoEngine.Rendering.Shaders;
using TakoEngine.Inputs;
using TakoEngine.Lighting;
using System.IO;
using TakoEngine.Helpers;
using TakoEngine.Rendering.PostProcessing;
using TakoEngine.Outputs;
using TakoEngine.Rendering.Processing;
using TakoEngine.Rendering.Textures;
using System.Drawing;
using System.Drawing.Imaging;

namespace TakoEngine.GameObjects
{
    public class GameState
    {
        internal InputState _inputState = new InputState();

        private RenderManager _renderManager;
        private TextureManager _textureManager = new TextureManager();

        internal Camera _camera;
        private List<GameObject> _gameObjects = new List<GameObject>();
        private List<Brush> _brushes = new List<Brush>();
        private List<Light> _lights = new List<Light>();

        private QuadTree _gameObjectQuads;
        private QuadTree _brushQuads;
        private QuadTree _lightQuads;

        public GameState(Map map, Resolution resolution)
        {
            _renderManager = new RenderManager(resolution);
            _textureManager.EnableMipMapping = true;
            _textureManager.EnableAnisotropy = true;

            LoadMap(map, resolution);
        }

        public void Resize()
        {
            _renderManager.Resize();
        }

        private void LoadMap(Map map, Resolution resolution)
        {
            _camera = map.Camera.ToCamera(resolution);

            _gameObjectQuads = new QuadTree(0, map.Boundaries);
            _brushQuads = new QuadTree(0, map.Boundaries);
            _lightQuads = new QuadTree(0, map.Boundaries);
            _lightQuads.InsertRange(map.Lights.Select(l => new BoundingCircle(l)));

            foreach (var mapLight in map.Lights)
            {
                AddEntity(mapLight);
            }

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

                AddEntity(brush);
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

                AddEntity(gameObject);
            }

            _renderManager.Load(_brushes, _gameObjects, map.SkyboxTextureFilePaths);
        }

        private int _nextAvailableID = 1;

        public GameObject GetByName(string name) => _gameObjects.First(g => g.Name == name);
        public GameEntity GetByID(int id)
        {
            var gameObject = _gameObjects.FirstOrDefault(g => g.ID == id);
            if (gameObject != null)
            {
                return gameObject;
            }
            else
            {
                var brush = _brushes.FirstOrDefault(b => b.ID == id);
                if (brush != null)
                {
                    return brush;
                }
                else
                {
                    var light = _lights.FirstOrDefault(l => l.ID == id);
                    if (light != null)
                    {
                        return light;
                    }
                    else
                    {
                        throw new KeyNotFoundException("Could not find any GameEntity with ID " + id);
                    }
                }
            }
        }

        public void AddEntity(GameEntity entity)
        {
            // Assign an ID
            if (entity.ID == 0)
            {
                entity.ID = _nextAvailableID;
                _nextAvailableID++;
            }

            switch (entity)
            {
                case GameObject gameObject:
                    if (string.IsNullOrEmpty(gameObject.Name)) throw new ArgumentException("GameObject must have a name defined");
                    if (_gameObjects.Any(g => g.Name == gameObject.Name)) throw new ArgumentException("GameObject must have a unique name");

                    _gameObjects.Add(gameObject);
                    break;
                case Brush brush:
                    _brushes.Add(brush);
                    break;
                case Light light:
                    _lights.Add(light);
                    break;
            }
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

        public void SetFrequency(double frequency) => _renderManager.Frequency = frequency;

        public void RenderWireframe() => _renderManager.RenderEntityIDs(_camera, _lights, _brushes, _gameObjects);//_renderManager.RenderWireframe(_camera, _brushes, _gameObjects);

        public void RenderDiffuseFrame() => _renderManager.RenderDiffuseFrame(_textureManager, _camera, _brushes, _gameObjects);

        public void RenderLitFrame() => _renderManager.RenderLitFrame(_textureManager, _camera, _lights, _brushes, _gameObjects);

        public void RenderFullFrame() => _renderManager.RenderFullFrame(_textureManager, _camera, _lights, _brushes, _gameObjects);

        private void PollForInput() => _inputState.UpdateState(Keyboard.GetState(), Mouse.GetState());

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
