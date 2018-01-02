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

namespace Graphics.GameObjects
{
    public class GameState
    {
        private ShaderProgram _program;
        private Camera _camera;
        private InputState _inputState = new InputState();

        private List<Brush> _brushes = new List<Brush>();
        private List<GameObject> _gameObjects = new List<GameObject>();
        private List<Light> _lights = new List<Light>();

        private QuadTree _brushQuads;
        private QuadTree _gameObjectQuads;
        private QuadTree _lightQuads;

        public GameWindow Window { get; private set; }

        public GameState(ShaderProgram program, Map map, GameWindow window)
        {
            _program = program;
            Window = window;

            _brushQuads = new QuadTree(0, map.Boundaries);
            _gameObjectQuads = new QuadTree(0, map.Boundaries);
            _lightQuads = new QuadTree(0, map.Boundaries);

            _camera = new Camera(map.Camera.Name, program, Window.Width, Window.Height);

            _lightQuads.InsertRange(map.Lights.Select(l => new BoundingCircle(l)));

            foreach (var brush in map.Brushes.Select(b => b.ToBrush(program)))
            {
                brush.AddTestColors();
                brush._program = program;

                if (brush.HasCollision)
                {
                    _brushQuads.Insert(brush.Bounds);
                }
                brush.AddLights(_lightQuads.Retrieve(brush.Bounds).Select(c => (Light)c.AttachedObject));

                _brushes.Add(brush);
            }

            foreach (var gameObject in map.GameObjects.Select(g => g.ToGameObject(program)))
            {
                if (gameObject.Name == map.Camera.AttachedGameObjectName)
                {
                    _camera.AttachedObject = gameObject;
                }

                gameObject._program = program;
                gameObject.Mesh.AddTestColors();
                gameObject.Bounds = gameObject.Name == "Player"
                    ? (Collider)new BoundingCircle(gameObject)
                    : new BoundingBox(gameObject);
                
                _gameObjects.Add(gameObject);
            }
        }

        public GameObject GetByName(string name) => _gameObjects.First(g => g.Name == name);

        public void UpdateAspectRatio(int width, int height) => _camera.UpdateAspectRatio(width, height);

        public void AddGameObject(GameObject gameObject)
        {
            if (string.IsNullOrEmpty(gameObject.Name)) throw new ArgumentException("GameObject must have a name defined");
            if (_gameObjects.Any(g => g.Name == gameObject.Name)) throw new ArgumentException("GameObject must have a unique name");

            gameObject._program = _program;
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
        }

        public void RenderFrame()
        {
            _camera.OnRenderFrame();

            foreach (var brush in _brushes)
            {
                brush.OnRenderFrame();
            }

            foreach (var gameObject in PerformOcclusionCulling(PerformFrustumCulling(_gameObjects)))
            {
                gameObject.OnRenderFrame();
            }

            PollForInput();
        }

        private void PollForInput() => _inputState.UpdateState(Keyboard.GetState(), Mouse.GetState(), Window);

        private IEnumerable<GameObject> PerformFrustumCulling(IEnumerable<GameObject> gameObjects)
        {
            // Don't render meshes that are not in the camera's view

            // Using the position of the gameObject, determine if we should render the mesh
            // We will also need a bounding sphere or bounding box from the mesh to determine this
            foreach (var gameObject in gameObjects)
            {
                Vector3 position = gameObject.Position;
            }
            
            return gameObjects;
        }

        private IEnumerable<GameObject> PerformOcclusionCulling(IEnumerable<GameObject> gameObjects)
        {
            // Don't render meshes that are obscured by closer meshes
            foreach (var gameObject in gameObjects)
            {
                Vector3 position = gameObject.Position;
            }

            return gameObjects;
        }

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
