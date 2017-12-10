using Graphics.GameObjects;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.GameStates
{
    public class GameState
    {
        private Camera _camera;
        private ShaderProgram _program;
        private Dictionary<string, GameObject> _gameObjects = new Dictionary<string, GameObject>();

        public Camera Camera => _camera;

        public GameState(Camera camera, ShaderProgram program)
        {
            _camera = camera;
            _program = program;
        }

        public GameObject GetByName(string name) => _gameObjects[name];

        public void AddGameObject(GameObject gameObject)
        {
            if (string.IsNullOrEmpty(gameObject.Name)) throw new ArgumentException("GameObject must have a name defined");
            if (_gameObjects.ContainsKey(gameObject.Name)) throw new ArgumentException("GameObject must have a unique name");

            gameObject._program = _program;
            _gameObjects.Add(gameObject.Name, gameObject);
        }

        public void UpdateFrame()
        {
            _camera.OnUpdateFrame();

            foreach (var gameObject in _gameObjects)
            {
                gameObject.Value.OnUpdateFrame();
            }
        }

        public void RenderFrame()
        {
            _camera.OnRenderFrame();

            foreach (var gameObject in PerformOcclusionCulling(PerformFrustumCulling(_gameObjects.Values)))
            {
                gameObject.OnRenderFrame();
            }
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
