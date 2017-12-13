using Graphics.GameObjects;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Camera Camera => _camera;

        public GameState(Player player, Camera camera, ShaderProgram program)
        {
            _player = player;
            _player._program = program;
            _camera = camera;
            _program = program;
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
            _player.OnHandleInput(_keyState, _mouseState, _previousKeyState, _previousMouseState);
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

            _keyState = Keyboard.GetState();
            _mouseState = Mouse.GetState();
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

        }

        public void LoadFromFile(string path)
        {

        }
    }
}
