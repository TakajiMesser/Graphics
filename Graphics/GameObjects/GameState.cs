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

namespace Graphics.GameObjects
{
    public class GameState
    {
        private GameWindow _window;
        private InputState _inputState = new InputState();

        private ShaderProgram _geometryProgram;
        private List<PostProcess> _preProcesses = new List<PostProcess>();
        private List<PostProcess> _postProcesses = new List<PostProcess>();

        private Camera _camera;
        private List<Brush> _brushes = new List<Brush>();
        private List<GameObject> _gameObjects = new List<GameObject>();
        private List<Light> _lights = new List<Light>();

        private QuadTree _brushQuads;
        private QuadTree _gameObjectQuads;
        private QuadTree _lightQuads;

        public GameState(Map map, GameWindow window)
        {
            _postProcesses.Add(new MotionBlur(new Resolution(window.Width, window.Height)));
            LoadPrograms();

            _window = window;

            _brushQuads = new QuadTree(0, map.Boundaries);
            _gameObjectQuads = new QuadTree(0, map.Boundaries);
            _lightQuads = new QuadTree(0, map.Boundaries);
            _lightQuads.InsertRange(map.Lights.Select(l => new BoundingCircle(l)));

            _camera = new Camera(map.Camera.Name, _geometryProgram, window.Width, window.Height);

            foreach (var brush in map.Brushes.Select(b => b.ToBrush(_geometryProgram)))
            {
                brush.AddTestColors();
                brush._program = _geometryProgram;

                if (brush.HasCollision)
                {
                    _brushQuads.Insert(brush.Bounds);
                }
                brush.AddLights(_lightQuads.Retrieve(brush.Bounds).Select(c => (Light)c.AttachedObject));

                _brushes.Add(brush);
            }

            foreach (var gameObject in map.GameObjects.Select(g => g.ToGameObject(_geometryProgram)))
            {
                if (gameObject.Name == map.Camera.AttachedGameObjectName)
                {
                    _camera.AttachedObject = gameObject;
                }

                gameObject._program = _geometryProgram;
                gameObject.Mesh.AddTestColors();
                gameObject.Bounds = gameObject.Name == "Player"
                    ? (Collider)new BoundingCircle(gameObject)
                    : new BoundingBox(gameObject);
                
                _gameObjects.Add(gameObject);
            }
        }

        private void LoadPrograms()
        {
            foreach (var process in _preProcesses)
            {
                process.Load();
            }

            _geometryProgram = new ShaderProgram(new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.VERTEX_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.FRAGMENT_SHADER_PATH)));

            foreach (var process in _postProcesses)
            {
                process.Load();
            }
        }

        public GameObject GetByName(string name) => _gameObjects.First(g => g.Name == name);

        public void UpdateAspectRatio(int width, int height) => _camera.UpdateAspectRatio(width, height);

        public void AddGameObject(GameObject gameObject)
        {
            if (string.IsNullOrEmpty(gameObject.Name)) throw new ArgumentException("GameObject must have a name defined");
            if (_gameObjects.Any(g => g.Name == gameObject.Name)) throw new ArgumentException("GameObject must have a unique name");

            gameObject._program = _geometryProgram;
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
            _geometryProgram.Use();

            _camera.OnRenderFrame();

            foreach (var brush in _brushes)
            {
                brush.OnRenderFrame();
            }

            foreach (var gameObject in PerformOcclusionCulling(PerformFrustumCulling(_gameObjects)))
            {
                gameObject.OnRenderFrame();
            }
        }

        private void PollForInput() => _inputState.UpdateState(Keyboard.GetState(), Mouse.GetState(), _window);

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
