using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using SpiceEngine.Maps;
using SpiceEngine.Rendering;
using SpiceEngineCore.Game;
using SpiceEngineCore.Maps;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using TangyHIDCore;
using Timer = System.Timers.Timer;
using Vector2 = SpiceEngineCore.Geometry.Vectors.Vector2;

namespace SpiceEngine.Game
{
    public class GameWindow : OpenTK.GameWindow, IMouseTracker, IInvoker
    {
        private SimulationManager _simulationManager;
        private RenderManager _renderManager;
        private GameLoader _gameLoader = new GameLoader();

        //private Dispatcher _mainDispatcher;
        private ConcurrentQueue<Action> _mainActionQueue = new ConcurrentQueue<Action>();

        private MouseState? _mouseState = null;

        private Timer _fpsTimer = new Timer(1000);
        private List<double> _frequencies = new List<double>();

        private bool _isClosing = false;

        private IMap _map;
        private object _loadLock = new object();

        public event EventHandler<EventArgs> GameLoaded;

        public GameWindow(IMap map) : base(1280, 720, GraphicsMode.Default, "My First OpenGL Game", GameWindowFlags.Default, DisplayDevice.Default, 3, 0, GraphicsContextFlags.ForwardCompatible)
        {
            _map = map;

            Resolution = new Resolution(Width, Height);
            WindowSize = new Resolution(Width, Height);

            Console.WriteLine("GL Version: " + GL.GetString(StringName.Version));

            _fpsTimer.Elapsed += FpsTimer_Elapsed;
        }

        private void FpsTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_frequencies.Count > 0)
            {
                var total = 0.0;

                for (var i = 0; i < _frequencies.Count; i++)
                {
                    total += _frequencies[i];
                }

                _renderManager.Frequency = total / _frequencies.Count;//_frequencies.Average();
                _frequencies.Clear();
            }
        }

        public void LoadAndRun()
        {
            // "Register" the calling thread as the graphics context thread before we start async loading
            MakeCurrent();
            //_mainDispatcher = Dispatcher.CurrentDispatcher;

            // Begin the background Task for loading the game world
            LoadAsync();

            // Begin a loop that blocks until the game world is loaded (to prevent the window from being disposed)
            while (true)
            {
                ProcessLoadEvents();

                if (_isClosing)
                {
                    break;
                }
            }
        }

        private void ProcessLoadEvents()
        {
            if (IsLoaded)
            {
                Run(60.0f, 0.0f);
                _isClosing = true;
            }
            else
            {
                // Check the queue every second. Once we find items in the queue, handle all of them before waiting again
                CheckAndHandleQueue();
                Task.Delay(1000).Wait();
            }
        }

        private void CheckAndHandleQueue()
        {
            while (_mainActionQueue.TryDequeue(out Action action))
            {
                //LogWatch("Popped item off queue");
                action();
                //LogWatch("Completed queue item");
            }
        }

        public async Task RunAsync(Action action)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            _mainActionQueue.Enqueue(() =>
            {
                action();
                taskCompletionSource.TrySetResult(true);
            });

            await taskCompletionSource.Task;
        }

        public void RunSync(Action action) => _mainActionQueue.Enqueue(action);

        // TODO - No-op...
        public void ForceUpdate() { }

        private async void LoadAsync()
        {
            _renderManager = new RenderManager(Resolution, WindowSize)
            {
                RenderMode = RenderModes.Full,
                Invoker = this
            };
            _fpsTimer.Start();

            _simulationManager = new SimulationManager(Resolution);
            _simulationManager.Load();
            _simulationManager.InputManager.EscapePressed += (s, args) => Close();
            _simulationManager.SetMouseTracker(this);
            _simulationManager.RenderProvider = _renderManager;

            var map = _map as Map3D;
            _simulationManager.PhysicsSystem.SetBoundaries(map.Boundaries);

            _gameLoader.SetEntityProvider(_simulationManager.EntityProvider);
            _gameLoader.AddComponentLoader(_simulationManager.PhysicsSystem);
            _gameLoader.AddComponentLoader(_simulationManager.BehaviorSystem);
            _gameLoader.AddComponentLoader(_simulationManager.AnimationSystem);
            _gameLoader.AddComponentLoader(_simulationManager.UISystem);
            _gameLoader.AddRenderableLoader(_renderManager);

            _gameLoader.AddFromMap(_map);

            _renderManager.SetEntityProvider(_simulationManager.EntityProvider);
            _renderManager.SetAnimationProvider(_simulationManager.AnimationSystem);
            _renderManager.SetUIProvider(_simulationManager.UISystem);
            
            _renderManager.LoadFromMap(_map);

            //_gameLoader.Load();
            _gameLoader.TimedOut += (s, args) => RunSync(() => throw new TimeoutException());
            await _gameLoader.LoadAsync();

            // Set up UIManager to track mouse selections for UI control interactions
            _simulationManager.InputManager.MouseDownSelected += (s, args) => _simulationManager.UISystem.RegisterSelection(_renderManager.GetEntityIDFromSelection(args.MouseCoordinates));
            _simulationManager.InputManager.MouseUpSelected += (s, args) => _simulationManager.UISystem.RegisterDeselection(_renderManager.GetEntityIDFromSelection(args.MouseCoordinates));
            //_simulationManager.BehaviorSystem.SetSelectionTracker(_renderManager);

            //_stopWatch.Stop();
            //LogWatch("Total");

            IsLoaded = true;

            //_renderManager.LoadFromMap(_map);

            /*_gameManager.BehaviorManager.Load();

            if (!string.IsNullOrEmpty(_map.Camera.AttachedActorName))
            {
                var actor = _gameManager.EntityManager.GetActor(_map.Camera.AttachedActorName);
                _gameManager.Camera.AttachToEntity(actor, true, false);
            }

            ProcessOnMainThread(() =>
            {
                MakeCurrent();
                _renderManager.LoadFromMap(_map);
                IsLoaded = true;
            });*/

            GameLoaded?.Invoke(this, new EventArgs());
        }

        public Resolution Resolution { get; private set; }
        public Resolution WindowSize { get; private set; }

        public Vector2? MouseCoordinates => _mouseState.HasValue
            ? new Vector2(_mouseState.Value.X, _mouseState.Value.Y)
            : (Vector2?)null;

        public Vector2? RelativeCoordinates => _mouseState.HasValue
            ? PointToClient(new Point(_mouseState.Value.X, _mouseState.Value.Y)).ToVector2()
            : (Vector2?)null;

        public bool IsMouseInWindow => _mouseState != null
            ? (_mouseState.Value.X.IsBetween(0, WindowSize.Width) && _mouseState.Value.Y.IsBetween(0, WindowSize.Height))
            : false;

        public bool IsLoaded { get; private set; }

        protected override void OnResize(EventArgs e)
        {
            WindowSize.Width = Width;
            WindowSize.Height = Height;

            if (_renderManager != null && _renderManager.IsLoaded)
            {
                _renderManager.ResizeWindow();
            }
            //Resolution.Width = Width;
            //Resolution.Height = Height;
            //_gameState?.Resize();
        }

        protected override void OnLoad(EventArgs e)
        {
            Location = new Point(0, 0);
            //WindowState = WindowState.Maximized;
            //Size = new System.Drawing.Size(1280, 720);

            /*_gameManager = new GameManager(Resolution, this);
            _gameManager.InputManager.EscapePressed += (s, args) => Close();

            _renderManager = new RenderManager(Resolution, WindowSize)
            {
                RenderMode = RenderModes.Full
            };
            _fpsTimer.Start();

            _gameManager.LoadFromMap(_map);

            _gameLoader = new GameLoader(_gameManager.EntityManager);
            _gameLoader.SetPhysicsLoader(_gameManager.PhysicsManager);
            _gameLoader.SetBehaviorLoader(_gameManager.BehaviorManager);
            _gameLoader.AddRenderableLoader(_renderManager);

            _gameLoader.AddFromMap(_map);

            _renderManager.SetEntityProvider(_gameManager.EntityManager);
            _renderManager.SetCamera(_gameManager.Camera);

            //_gameLoader.Load();
            await _gameLoader.LoadAsync();
            _renderManager.LoadFromMap(_map);
            _gameManager.BehaviorManager.Load();

            if (!string.IsNullOrEmpty(_map.Camera.AttachedActorName))
            {
                var actor = _gameManager.EntityManager.GetActor(_map.Camera.AttachedActorName);
                _gameManager.Camera.AttachToEntity(actor, true, false);
            }

            IsLoaded = true;
            GameLoaded?.Invoke(this, new EventArgs());*/

            /*lock (_loadLock)
            {
                IsLoaded = true;

                if (_map != null)
                {
                    var entityMapping = _gameManager.LoadFromMap(_map);
                    _renderManager.SetEntityProvider(_gameManager.EntityManager);
                    _renderManager.SetCamera(_gameManager.Camera);
                    _renderManager.LoadFromMap(_map, entityMapping);
                }
            }*/
        }

        /*public void LoadMap(Map map)
        {
            lock (_loadLock)
            {
                _map = map;

                if (IsLoaded)
                {
                    var entityMapping = _gameManager.LoadFromMap(_map);
                    _renderManager.LoadFromMap(_map, _gameManager.EntityManager, entityMapping);
                }
            }
        }*/

        //protected override void OnMouseEnter(EventArgs e) => CursorVisible = false;

        //protected override void OnMouseLeave(EventArgs e) => CursorVisible = true;

        // Handle game logic, guaranteed to run at a fixed rate, regardless of FPS
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            //_mouseDevice = InputDriver.Mouse;
            //_mouseDevice = Mouse;
            _mouseState = Mouse.GetCursorState();
            _simulationManager.Update();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            //if (IsLoaded)
            //{
            if (_renderManager != null && _renderManager.IsLoaded)
                _frequencies.Add(RenderFrequency);
                _renderManager.Tick();

                GL.UseProgram(0);
                SwapBuffers();
            //}
        }
    }
}
