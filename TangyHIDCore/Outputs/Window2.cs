using SpiceEngineCore.Game;
using SpiceEngineCore.Rendering;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using Configuration = SpiceEngineCore.Game.Settings.Configuration;

namespace TangyHIDCore.Outputs
{
    public abstract class Window2 : NativeWindow, IInvoker
    {
        //private GraphicsContext _context;
        private ConcurrentQueue<Action> _loadQueue = new ConcurrentQueue<Action>();

        private Stopwatch _updateWatch = new Stopwatch();
        private Stopwatch _renderWatch = new Stopwatch();

        private double _msPerUpdate;
        private double _msPerRender;

        private double _msSinceUpdate;
        private double _msSinceRender;

        protected ISimulate _simulator;
        protected IRender _renderer;
        protected Configuration _configuration;
        protected GameLoader _gameLoader;

        //base(1280, 720, GraphicsMode.Default, "My First OpenGL Game", GameWindowFlags.Default, DisplayDevice.Default, 3, 0, GraphicsContextFlags.ForwardCompatible)
        public Window2(Configuration configuration) : base(configuration)
        {
            _configuration = configuration;

            _msPerUpdate = 1000.0 / _configuration.UpdatesPerSecond;
            _msPerRender = 1000.0 / _configuration.RendersPerSecond;

            Display = new Display(Width, Height, false);
        }

        public Display Display { get; }
        public bool IsLoaded { get; protected set; }

        public void Start()
        {
            MakeCurrent();
            LoadAsync();

            while (true)
            {
                if (IsLoaded)
                {
                    Run();
                    return;
                }
                else
                {
                    // Check the queue every second. Once we find items in the queue, handle all of them before waiting again
                    ProcessLoadQueue();
                    Task.Delay(1000).Wait();
                }

                if (!Exists || IsExiting)
                {
                    return;
                }
            }
        }

        public abstract Task LoadAsync();

        public void Run()
        {
            //Visible = true;

            _updateWatch.Start();
            _renderWatch.Start();

            while (true)
            {
                ProcessEvents();

                if (!Exists || IsExiting)
                {
                    DestroyWindow();
                    return;
                }

                CheckForUpdate();
                CheckForRender();
            }
        }

        public async Task RunAsync(Action action)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            _loadQueue.Enqueue(() =>
            {
                action();
                taskCompletionSource.TrySetResult(true);
            });

            await taskCompletionSource.Task;
        }

        public void RunSync(Action action) => _loadQueue.Enqueue(action);

        // TODO - No-op...
        public void ForceUpdate() { }

        private void ProcessLoadQueue()
        {
            while (_loadQueue.TryDequeue(out Action action))
            {
                action();
            }
        }

        protected override void OnSizeChanged(int width, int height)
        {
            base.OnSizeChanged(width, height);
            //_context.Update(WindowInfo);
            Display.Window.Update(Width, Height);
            // _gameState?.Resize();
        }

        protected virtual void Update(double elapsedMilliseconds = 0.0) => _simulator.Tick();

        protected virtual void Render(double elapsedMilliseconds = 0.0)
        {
            // TODO - Would be better to avoid checks every render cycle
            if (_renderer != null && _renderer.IsLoaded)
            {
                _renderer.Tick();
                SwapBuffers();
            }
        }

        private void CheckForUpdate()
        {
            _msSinceUpdate += _updateWatch.Elapsed.TotalMilliseconds;
            _updateWatch.Restart();

            // How many updates, if any, should take place?
            if (_msSinceUpdate >= _msPerUpdate)
            {
                var nUpdates = (int)(_msSinceUpdate / _msPerUpdate);
                _msSinceUpdate -= nUpdates * _msPerUpdate;
                var elapsed = _msSinceUpdate;

                Update(elapsed);
            }
        }

        private void CheckForRender()
        {
            _msSinceRender += _renderWatch.Elapsed.TotalMilliseconds;
            _renderWatch.Restart();

            // How many updates, if any, should take place?
            if (_msSinceRender >= _msPerRender)
            {
                var nRenders = (int)(_msSinceRender / _msPerRender);
                _msSinceRender -= nRenders * _msPerRender;
                var elapsed = _msSinceRender;

                Render(elapsed);
            }
        }

        private void DispatchUpdates(int nUpdates)
        {
            for (var i = 0; i < nUpdates; i++)
            {
                Update();
            }
        }

        private void DispatchRenders(int nRenders)
        {
            for (var i = 0; i < nRenders; i++)
            {
                Render();
            }
        }
    }
}
