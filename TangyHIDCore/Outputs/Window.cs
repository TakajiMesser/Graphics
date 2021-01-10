using OpenTK;
using OpenTK.Graphics.OpenGL;
using SpiceEngineCore.Game;
using SpiceEngineCore.Rendering;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Configuration = SpiceEngineCore.Game.Settings.Configuration;

namespace TangyHIDCore.Outputs
{
    public abstract class Window : NativeWindow, IInvoker
    {
        private ConcurrentQueue<Action> _mainActionQueue = new ConcurrentQueue<Action>();

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

        public Window(Configuration configuration)
        {
            _configuration = configuration;

            _msPerUpdate = 1000.0 / _configuration.UpdatesPerSecond;
            _msPerRender = 1000.0 / _configuration.RendersPerSecond;

            Display = new Display(Width, Height, false);
        }

        public Display Display { get; }
        public bool IsLoaded { get; protected set; }
        public bool IsExiting { get; private set; }

        public void Start()
        {
            // TODO - Cannot access this method
            // MakeCurrent();
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
                    CheckAndHandleQueue();
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
            _updateWatch.Start();
            _renderWatch.Start();

            while (true)
            {
                ProcessEvents();

                if (!Exists || IsExiting)
                {
                    return;
                }

                CheckForUpdate();
                CheckForRender();
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

        private void CheckAndHandleQueue()
        {
            while (_mainActionQueue.TryDequeue(out Action action))
            {
                action();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            Display.Window.Update(Width, Height);
            // _gameState?.Resize();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            IsExiting = true;
            base.OnClosing(e);
        }

        protected virtual void Update()
        {
            _simulator.Tick();
        }

        protected virtual void Render()
        {
            // TODO - Would be better to avoid checks every render cycle
            if (_renderer != null && _renderer.IsLoaded)
            {
                _renderer.Tick();
                GL.UseProgram(0); // TODO - Why is this needed?

                // TODO - Cannot access this method
                //SwapBuffers();
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

                DispatchUpdates(nUpdates);
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

                DispatchRenders(nRenders);
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
