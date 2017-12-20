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

namespace Graphics.GameStates
{
    public class GameState
    {
        private Player _player;
        private Camera _camera;
        private ShaderProgram _program;
        private Dictionary<string, GameObject> _gameObjects = new Dictionary<string, GameObject>();

        private KeyboardState _keyState;
        private KeyboardState _previousKeyState;
        private MouseState _mouseState;
        private MouseState _previousMouseState;
        private MouseDevice _mouse;
        private MouseDevice _previousMouse;

        public GameWindow Window { get; private set; }
        public Camera Camera => _camera;

        public GameState(Player player, Camera camera, ShaderProgram program)
        {
            _player = player;
            _player._program = program;
            _camera = camera;
            _program = program;
        }

        public GameState(ShaderProgram program, Map map, GameWindow window)
        {
            _program = program;
            Window = window;

            _player = map.Player.ToPlayer(program);
            _camera = new Camera(map.Camera.Name, program, Window.Width, Window.Height);

            if (_player != null)
            {
                _player.Collider = new BoundingSphere(_player.Mesh.Vertices)
                {
                    Center = _player.Position
                };

                _player.Mesh.AddTestColors();
                _player.Mesh.AddTestLight();

                _player._program = program;

                if (_player.Name == map.Camera.AttachedGameObjectName)
                {
                    _camera.AttachedObject = _player;
                }
            }

            foreach (var gameObject in map.GameObjects.Select(g => g.ToGameObject(program)))
            {
                if (gameObject.Name == map.Camera.AttachedGameObjectName)
                {
                    _camera.AttachedObject = gameObject;
                }

                gameObject.Collider = new BoundingSphere(gameObject.Mesh.Vertices)
                {
                    Center = gameObject.Position
                };

                gameObject.Mesh.AddTestColors();
                gameObject.Mesh.AddTestLight();

                gameObject._program = program;
                _gameObjects.Add(gameObject.Name, gameObject);
            }
        }

        public GameObject GetByName(string name) => _gameObjects[name];

        public void UpdateAspectRatio(int width, int height)
        {
            _camera.UpdateAspectRatio(width, height);
        }

        public void AddGameObject(GameObject gameObject)
        {
            if (string.IsNullOrEmpty(gameObject.Name)) throw new ArgumentException("GameObject must have a name defined");
            if (_gameObjects.ContainsKey(gameObject.Name)) throw new ArgumentException("GameObject must have a unique name");

            gameObject._program = _program;
            _gameObjects.Add(gameObject.Name, gameObject);
        }

        public void HandleInput()
        {
            _player.OnHandleInput(_keyState, _mouseState, _mouse, Window.Width, Window.Height, _camera);
            _camera.OnHandleInput(_keyState, _mouseState, _previousKeyState, _previousMouseState);

            foreach (var gameObject in _gameObjects)
            {
                gameObject.Value.OnHandleInput(_keyState, _mouseState, _previousKeyState, _previousMouseState);
            }
        }

        public void UpdateFrame()
        {
            // For each object that has a non-zero transform, we need to determine the set of game objects to compare it against for hit detection
            _player.OnUpdateFrame(_gameObjects.Select(g => g.Value.Collider));
            _camera.OnUpdateFrame();

            foreach (var gameObject in _gameObjects)
            {
                gameObject.Value.OnUpdateFrame();
            }
        }

        public void RenderFrame()
        {
            _camera.OnRenderFrame();
            _player.OnRenderFrame();

            foreach (var gameObject in PerformOcclusionCulling(PerformFrustumCulling(_gameObjects.Values)))
            {
                gameObject.OnRenderFrame();
            }

            PollForInput();
        }

        private void PollForInput()
        {
            _previousKeyState = _keyState;
            _previousMouseState = _mouseState;
            _previousMouse = _mouse;

            _keyState = Keyboard.GetState();
            _mouseState = Mouse.GetState();
            _mouse = Window.Mouse;
        }

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
            // 

            throw new NotImplementedException();
        }

        public static GameState LoadFromFile(string path)
        {
            throw new NotImplementedException();
        }
    }
}
