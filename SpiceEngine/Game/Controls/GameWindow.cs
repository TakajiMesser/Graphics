namespace SpiceEngine.Game.Controls
{
    /*public class GameWindow : NativeWindow, IGameWindow, IMouseTracker, IInvoker, IDisposable
    {
        private const string TITLE = "My Game Window";

        private IGraphicsContext _glContext;
        private VSyncMode _vsyncMode;

        private GameLoader _gameLoader;
        private GameManager _gameManager;
        private RenderManager _renderManager;

        //private Dispatcher _mainDispatcher;
        private ConcurrentQueue<Action> _mainActionQueue = new ConcurrentQueue<Action>();

        private MouseState? _mouseState = null;

        private Timer _fpsTimer = new Timer(1000);
        private List<double> _frequencies = new List<double>();

        private bool _isClosing = false;

        private IMap _map;
        private object _loadLock = new object();

        public GameWindow(int width, int height) : base(width, height, TITLE, GameWindowFlags.Fullscreen, GraphicsMode.Default, DisplayDevice.Default)
        {

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

        public event EventHandler<EventArgs> GameLoaded;

        public GameWindow() : base(1280, 720, GraphicsMode.Default, "My First OpenGL Game", GameWindowFlags.Default, DisplayDevice.Default, 3, 0, GraphicsContextFlags.ForwardCompatible)
        {
            InitializeGLContext();

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

        public void SetMap(IMap map) => _map = map;

        private void InitializeGLContext()
        {
            try
            {
                _glContext = new GraphicsContext(GraphicsMode.Default, WindowInfo, 3, 0, GraphicsContextFlags.ForwardCompatible);
                _glContext.MakeCurrent(WindowInfo);

                var internalContext = _glContext as IGraphicsContextInternal;
                internalContext.LoadAll();

                _vsyncMode = VSyncMode.On;
            }
            catch (Exception ex)
            {
                base.Dispose();
                throw;
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

        public void Run(Action action) => _mainActionQueue.Enqueue(action);

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

        private async void LoadAsync()
        {
            _gameManager = new GameManager(Resolution, this);
            _gameManager.InputManager.EscapePressed += (s, args) => Close();

            _renderManager = new RenderManager(Resolution, WindowSize)
            {
                RenderMode = RenderModes.Full,
                Invoker = this
            };
            _fpsTimer.Start();

            _gameManager.LoadFromMap(_map);

            _gameLoader = new GameLoader();
            _gameLoader.SetEntityProvider(_gameManager.EntityManager);
            _gameLoader.SetPhysicsLoader(_gameManager.PhysicsManager);
            _gameLoader.SetBehaviorLoader(_gameManager.BehaviorManager);
            _gameLoader.SetAnimatorLoader(_gameManager.AnimationManager);
            _gameLoader.AddRenderableLoader(_renderManager);

            _gameLoader.AddFromMap(_map);

            _renderManager.SetEntityProvider(_gameManager.EntityManager);
            _renderManager.SetAnimationProvider(_gameManager.AnimationManager);
            
            _renderManager.LoadFromMap(_map);

            //_gameLoader.Load();
            _gameLoader.TimedOut += (s, args) => Run(() => throw new TimeoutException());
            await _gameLoader.LoadAsync();

            var defaultCamera = _gameManager.EntityManager.Cameras.First();
            _gameManager.Camera = defaultCamera;
            _gameManager.BehaviorManager.SetCamera(defaultCamera);
            _renderManager.SetCamera(defaultCamera);

            //_stopWatch.Stop();
            //LogWatch("Total");

            IsLoaded = true;

            //_renderManager.LoadFromMap(_map);

            GameLoaded?.Invoke(this, new EventArgs());
        }

        

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
        }

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


    }*/
}
