using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using SpiceEngine.Entities.Builders;
using SpiceEngine.Inputs;
using SpiceEngine.Maps;
using SpiceEngine.Outputs;
using SpiceEngine.Rendering;
using SpiceEngine.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Timer = System.Timers.Timer;

namespace SpiceEngine.Game
{
    public class GameWindow : OpenTK.GameWindow, IMouseTracker
    {
        private GameManager _gameManager;
        private RenderManager _renderManager;
        private EntityLoader _entityLoader;

        //private Dispatcher _mainDispatcher;
        private static ConcurrentQueue<Action> _mainActionQueue = new System.Collections.Concurrent.ConcurrentQueue<Action>();

        private MouseState? _mouseState = null;

        private Timer _fpsTimer = new Timer(1000);
        private List<double> _frequencies = new List<double>();

        private Map _map;
        private object _loadLock = new object();

        public event EventHandler<EventArgs> GameLoaded;

        public GameWindow(Map map) : base(1280, 720, GraphicsMode.Default, "My First OpenGL Game", GameWindowFlags.Default, DisplayDevice.Default, 3, 0, GraphicsContextFlags.ForwardCompatible)
        {
            _map = map;

            Resolution = new Resolution(Width, Height);
            WindowSize = new Resolution(Width, Height);

            Console.WriteLine("GL Version: " + GL.GetString(StringName.Version));

            _fpsTimer.Elapsed += (s, e) =>
            {
                if (_frequencies.Count > 0)
                {
                    _renderManager.Frequency = _frequencies.Average();
                    _frequencies.Clear();
                }
            };
        }

        private Timer _loadTimer = new Timer();
        private Stopwatch _loadWatch = new Stopwatch();
        private int _loadTimeout = 300000;

        public void LoadAndRun()
        {
            _loadWatch.Start();

            // "Register" the calling thread as the graphics context thread before we start async loading
            MakeCurrent();
            //_mainDispatcher = Dispatcher.CurrentDispatcher;

            // Begin the background Task for loading the game world
            LoadAsync();

            // Begin a loop that blocks until the game world is loaded (to prevent the window from being disposed)
            while (true)
            {
                ProcessLoadEvents();
            }
            /*while (true)
            {
                if (IsLoaded)
                {
                    Run(60.0f, 0.0f);
                }
                else if (_loadWatch.ElapsedMilliseconds > _loadTimeout)
                {
                    throw new TimeoutException();
                }
            }*/
        }

        private void ProcessLoadEvents()
        {
            if (IsLoaded)
            {
                Run(60.0f, 0.0f);
            }
            else if (_loadWatch.ElapsedMilliseconds > _loadTimeout)
            {
                throw new TimeoutException();
            }
            else
            {
                if (_mainActionQueue.TryDequeue(out Action action))
                {
                    action();
                }

                Task.Delay(1000).Wait();
            }
        }

        public static void ProcessOnMainThread(Action action) => _mainActionQueue.Enqueue(action);

        private async void LoadAsync()
        {
            _gameManager = new GameManager(Resolution, this);
            _gameManager.InputManager.EscapePressed += (s, args) => Close();

            _renderManager = new RenderManager(Resolution, WindowSize)
            {
                RenderMode = RenderModes.Full
            };
            _fpsTimer.Start();

            _gameManager.LoadFromMap(_map);

            _entityLoader = new EntityLoader(_gameManager.EntityManager);
            _entityLoader.SetPhysicsManager(_gameManager.PhysicsManager);
            _entityLoader.SetBehaviorManager(_gameManager.BehaviorManager);
            _entityLoader.AddRenderManager(_renderManager);

            _entityLoader.AddFromMap(_map);

            _renderManager.SetEntityProvider(_gameManager.EntityManager);
            _renderManager.SetCamera(_gameManager.Camera);

            //_entityLoader.Load();
            await _entityLoader.LoadAsync();

            //_renderManager.LoadFromMap(_map);

            _gameManager.BehaviorManager.Load();

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
            });

            GameLoaded?.Invoke(this, new EventArgs());
        }

        public Resolution Resolution { get; private set; }
        public Resolution WindowSize { get; private set; }

        public Vector2? MouseCoordinates => _mouseState.HasValue
            ? new Vector2(_mouseState.Value.X, _mouseState.Value.Y)
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

            _entityLoader = new EntityLoader(_gameManager.EntityManager);
            _entityLoader.SetPhysicsManager(_gameManager.PhysicsManager);
            _entityLoader.SetBehaviorManager(_gameManager.BehaviorManager);
            _entityLoader.AddRenderManager(_renderManager);

            _entityLoader.AddFromMap(_map);

            _renderManager.SetEntityProvider(_gameManager.EntityManager);
            _renderManager.SetCamera(_gameManager.Camera);

            //_entityLoader.Load();
            await _entityLoader.LoadAsync();
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
            //OpenTK.Input.
            //_mouseDevice = InputDriver.Mouse;
            //_mouseDevice = Mouse;
            _mouseState = Mouse.GetCursorState();
            _gameManager.Update();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            //if (IsLoaded)
            //{
                _frequencies.Add(RenderFrequency);
                _renderManager.Tick();

                GL.UseProgram(0);
                SwapBuffers();
            //}
        }
    }
}
