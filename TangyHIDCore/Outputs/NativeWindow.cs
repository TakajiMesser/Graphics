using SpiceEngineCore.Game;
using SpiceEngineCore.Game.Settings;
using SpiceEngineCore.HID;
using SpiceEngineCore.Rendering;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TangyHIDCore.Outputs
{
    public enum UpdateStyles
    {
        Manual,
        Auto
    }

    public abstract class NativeWindow : Window, IInvoker
    {
        private bool _hasWork = false;
        private ConcurrentQueue<Action> _workQueue = new ConcurrentQueue<Action>();

        private Stopwatch _updateWatch = new Stopwatch();
        private Stopwatch _renderWatch = new Stopwatch();

        private double _msPerUpdate;
        private double _msPerRender;

        private double _msSinceUpdate;
        private double _msSinceRender;

        protected ISimulate _simulator;
        protected IRender _renderer;

        public NativeWindow(IWindowConfig configuration, IWindowContextFactory windowFactory) : base(configuration, windowFactory)
        {
            _msPerUpdate = 1000.0 / configuration.UpdatesPerSecond;
            _msPerRender = 1000.0 / configuration.RendersPerSecond;

            Display = new Display(Width, Height, false);
            WindowSize = new Resolution(Width, Height);
        }

        public UpdateStyles UpdateStyle { get; set; }
        public Display Display { get; }

        public bool IsStarted { get; private set; }
        public bool IsLoaded { get; private set; }
        public bool IsRunning { get; private set; }

        public event EventHandler Loaded;

        // TODO - No-op...
        public virtual void ForceUpdate() { }

        public void InvokeSync(Action action)
        {
            _workQueue.Enqueue(action);
            _hasWork = true;
        }

        public async Task InvokeAsync(Action action)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            _workQueue.Enqueue(() =>
            {
                action();
                taskCompletionSource.TrySetResult(true);
            });

            _hasWork = true;
            await taskCompletionSource.Task;
        }

        public void StartLoad()
        {
            IsRunning = true;

            //MakeCurrent();
            LoadAsync().ContinueWith(_ =>
            {
                IsLoaded = true;
            });

            while (IsRunning)
            {
                ProcessEvents();

                if (IsLoaded || !Exists || IsExiting)
                {
                    Stop();
                    Loaded?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public void Start()
        {
            IsRunning = true;

            //MakeCurrent();
            LoadAsync().ContinueWith(_ =>
            {
                IsLoaded = true;
                Loaded?.Invoke(this, EventArgs.Empty);
            });

            while (IsRunning)
            {
                ProcessEvents();

                if (IsLoaded)
                {
                    Run();
                    return;
                }
                else
                {
                    // TODO - Remove this shitty busy-polling
                    // Check the load queue every second, until we are loaded. Once we find items in the queue, handle all of them before waiting again
                    //ProcessLoadQueue();
                    //Task.Delay(1000).Wait();
                }

                if (!Exists || IsExiting)
                {
                    return;
                }
            }
        }

        public void Stop() => IsRunning = false;

        public abstract Task LoadAsync();

        private void Run()
        {
            _updateWatch.Start();
            _renderWatch.Start();

            while (IsRunning)
            {
                //ProcessEvents();

                if (!Exists || IsExiting)
                {
                    DestroyWindow();
                    return;
                }

                CheckForUpdate();
                CheckForRender();
            }
        }

        protected override bool ProcessEvents()
        {
            if (base.ProcessEvents() && _hasWork)
            {
                _hasWork = false;

                while (_workQueue.TryDequeue(out Action action))
                {
                    action();
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnSizeChanged(int width, int height)
        {
            base.OnSizeChanged(width, height);
            //_context.Update(WindowInfo);
            Display.Window.Update(Width, Height);
            // _gameState?.Resize();
        }

        protected virtual void Update(double elapsedMilliseconds = 0.0)
        {
            ProcessEvents();
            _simulator.Tick();
        }

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
