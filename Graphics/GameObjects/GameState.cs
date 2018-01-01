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

namespace Graphics.GameObjects
{
    public class GameState
    {
        private Camera _camera;
        private ShaderProgram _program;
        private Dictionary<string, GameObject> _gameObjects = new Dictionary<string, GameObject>();
        private List<Brush> _brushes = new List<Brush>();
        private InputState _inputState = new InputState();
        private QuadTree _quadTree;

        public GameWindow Window { get; private set; }

        public GameState(Camera camera, ShaderProgram program)
        {
            _camera = camera;
            _program = program;
        }

        public GameState(ShaderProgram program, Map map, GameWindow window)
        {
            _program = program;
            Window = window;

            _quadTree = new QuadTree(0, map.Boundaries);
            _camera = new Camera(map.Camera.Name, program, Window.Width, Window.Height);

            foreach (var brush in map.Brushes.Select(b => b.ToBrush(program)))
            {
                brush.AddTestColors();
                brush.AddTestLight();
                brush._program = program;
                _brushes.Add(brush);

                if (brush.Collider != null)
                {
                    _quadTree.Insert(brush.Collider);
                } 
            }

            foreach (var gameObject in map.GameObjects.Select(g => g.ToGameObject(program)))
            {
                if (gameObject.Name == map.Camera.AttachedGameObjectName)
                {
                    _camera.AttachedObject = gameObject;
                }

                gameObject.Collider = gameObject.Name == "Player"
                    ? (Collider)new BoundingSphere(gameObject)
                    : new BoundingBox(gameObject);

                gameObject.Mesh.AddTestColors();
                gameObject.Mesh.AddTestLight();

                gameObject._program = program;
                _gameObjects.Add(gameObject.Name, gameObject);

                if (gameObject.Collider != null)
                {
                    _quadTree.Insert(gameObject.Collider);
                }
            }
        }

        public GameObject GetByName(string name) => _gameObjects[name];

        public void UpdateAspectRatio(int width, int height) => _camera.UpdateAspectRatio(width, height);

        public void AddGameObject(GameObject gameObject)
        {
            if (string.IsNullOrEmpty(gameObject.Name)) throw new ArgumentException("GameObject must have a name defined");
            if (_gameObjects.ContainsKey(gameObject.Name)) throw new ArgumentException("GameObject must have a unique name");

            gameObject._program = _program;
            _gameObjects.Add(gameObject.Name, gameObject);

            if (gameObject.Collider != null)
            {
                _quadTree.Insert(gameObject.Collider);
            }
        }

        public void Initialize()
        {
            foreach (var gameObject in _gameObjects)
            {
                gameObject.Value.OnInitialization();
            }
        }

        public void HandleInput()
        {
            _camera.OnHandleInput(_inputState);

            foreach (var gameObject in _gameObjects)
            {
                gameObject.Value.OnHandleInput(_inputState, _camera);
            }
        }

        public void UpdateFrame()
        {
            // For each object that has a non-zero transform, we need to determine the set of colliders to compare it against for hit detection
            foreach (var gameObject in _gameObjects)
            {
                var filteredColliders = _quadTree.Retrieve(gameObject.Value.Collider)
                    .Where(c => c.AttachedObject.GetType() != typeof(GameObject)
                        || ((GameObject)c.AttachedObject).Name != gameObject.Value.Name);

                gameObject.Value.OnUpdateFrame(filteredColliders);
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

            foreach (var gameObject in PerformOcclusionCulling(PerformFrustumCulling(_gameObjects.Values)))
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
