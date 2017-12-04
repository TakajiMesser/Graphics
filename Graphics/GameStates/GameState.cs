using Graphics.GameObjects;
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
            foreach (var gameObject in _gameObjects)
            {
                gameObject.Value.OnUpdateFrame();
            }
        }

        public void RenderFrame()
        {
            _camera.OnRenderFrame();

            foreach (var gameObject in _gameObjects)
            {
                gameObject.Value.OnRenderFrame();
            }
        }

        public void SaveToFile(string path)
        {

        }

        public void LoadFromFile(string path)
        {

        }
    }
}
