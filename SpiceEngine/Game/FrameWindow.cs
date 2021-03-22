using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngine.Maps;
using SpiceEngine.Rendering;
using SpiceEngineCore.Game;
using SpiceEngineCore.Rendering;
using SweetGraphicsCore.Buffers;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TangyHIDCore.Outputs;
using Configuration = SpiceEngineCore.Game.Settings.Configuration;
using PixelFormat = SpiceEngine.GLFWBindings.GLEnums.PixelFormat;

namespace SpiceEngine.Game
{
    public enum UpdateStyles
    {
        Manual,
        Auto
    }

    public class FrameWindow : TangyHIDCore.Outputs.HiddenWindow, IRenderContextProvider, IInvoker
    {
        private ConcurrentQueue<Action> _loadQueue = new ConcurrentQueue<Action>();

        private Image _frame;
        private FrameBuffer _frameBuffer;
        private RenderBuffer _colorBuffer;
        private RenderBuffer _depthBuffer;
        private WriteableBitmap _bitmap;

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

        public FrameWindow(Configuration configuration, Image frame) : base(configuration)
        {
            _configuration = configuration;
            _frame = frame;

            _msPerUpdate = 1000.0 / _configuration.UpdatesPerSecond;
            _msPerRender = 1000.0 / _configuration.RendersPerSecond;

            Display = new Display(Width, Height, false);
            LoadBuffers(Width, Height);
        }

        public UpdateStyles UpdateStyle { get; set; }
        public IRenderContext CurrentContext => this;
        public Display Display { get; private set; }
        public Map Map { get; set; }
        public bool IsLoaded { get; protected set; }

        private void LoadBuffers(int width, int height)
        {
            _colorBuffer = new RenderBuffer(this, width, height, RenderbufferTarget.Renderbuffer, InternalFormat.Rgba);
            _colorBuffer.Load();

            _depthBuffer = new RenderBuffer(this, width, height, RenderbufferTarget.Renderbuffer, InternalFormat.DepthComponent);
            _depthBuffer.Load();

            _frameBuffer = new FrameBuffer(this);
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment0, _colorBuffer);
            _frameBuffer.Add(FramebufferAttachment.DepthAttachment, _depthBuffer);
            _frameBuffer.Load();

            _bitmap = new WriteableBitmap(width, height, 1.0, 1.0, PixelFormats.Bgra32, null);
            _frame.Source = _bitmap;
        }

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

        public async Task LoadAsync()
        {
            var renderManager = new RenderManager(CurrentContext, Display)
            {
                Invoker = this
            };

            var simulationManager = new SimulationManager(Display.Resolution);
            simulationManager.Load();
            //simulationManager.InputManager.RegisterDevices(_hiddenWindow);
            //simulationManager.SetMouseTracker(this);
            simulationManager.RenderProvider = renderManager;
            simulationManager.PhysicsSystem.SetBoundaries(Map.Boundaries);

            _gameLoader = new SpiceEngineCore.Game.GameLoader();
            _gameLoader.SetEntityProvider(simulationManager.EntityProvider);
            _gameLoader.AddComponentLoader(simulationManager.PhysicsSystem);
            _gameLoader.AddComponentLoader(simulationManager.BehaviorSystem);
            _gameLoader.AddComponentLoader(simulationManager.AnimationSystem);
            _gameLoader.AddComponentLoader(simulationManager.UISystem);
            _gameLoader.AddRenderableLoader(renderManager);

            _gameLoader.AddFromMap(Map);

            renderManager.SetEntityProvider(simulationManager.EntityProvider);
            renderManager.SetAnimationProvider(simulationManager.AnimationSystem);
            renderManager.SetUIProvider(simulationManager.UISystem);

            renderManager.LoadFromMap(Map);

            //_gameLoader.Load();
            _gameLoader.TimedOut += (s, args) => RunSync(() => throw new TimeoutException());
            await _gameLoader.LoadAsync();

            // Set up UIManager to track mouse selections for UI control interactions
            simulationManager.InputManager.MouseDownSelected += (s, args) => simulationManager.UISystem.RegisterSelection(renderManager.GetEntityIDFromSelection(args.MouseCoordinates));
            simulationManager.InputManager.MouseUpSelected += (s, args) => simulationManager.UISystem.RegisterDeselection(renderManager.GetEntityIDFromSelection(args.MouseCoordinates));
            //_simulationManager.BehaviorSystem.SetSelectionTracker(_renderManager);

            //_stopWatch.Stop();
            //LogWatch("Total");

            //_simulator = simulationManager;
            _renderer = renderManager;

            IsLoaded = true;
        }

        public void Run()
        {
            //Visible = true;

            //_updateWatch.Start();
            _renderWatch.Start();

            while (true)
            {
                ProcessEvents();

                if (!Exists || IsExiting)
                {
                    DestroyWindow();
                    return;
                }

                //CheckForUpdate();
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

        public void ForceUpdate()
        {
            if (UpdateStyle == UpdateStyles.Manual)
            {
                Render();
            }
        }

        private void ProcessLoadQueue()
        {
            while (_loadQueue.TryDequeue(out Action action))
            {
                action();
            }
        }

        protected virtual void Update(double elapsedMilliseconds = 0.0) => _simulator.Tick();

        protected virtual void Render(double elapsedMilliseconds = 0.0)
        {
            // TODO - Would be better to avoid checks every render cycle
            if (_renderer != null && _renderer.IsLoaded)
            {
                _renderer.Tick();
                SwapBuffers();
                UpdateFrame();
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

        private void UpdateFrame()
        {
            _frameBuffer.BindAndRead(ReadBufferMode.ColorAttachment0);
            _bitmap.Lock();

            GL.PixelStorei(PixelStoreParameter.UnpackAlignment, 1);
            GL.ReadPixels(0, 0, Width, Height, PixelFormat.Bgra, PixelType.UnsignedByte, _bitmap.BackBuffer);
            GL.Finish();

            _bitmap.AddDirtyRect(new System.Windows.Int32Rect(0, 0, Width, Height));
            _bitmap.Unlock();
        }

        public void Resize(int width, int height)
        {
            _colorBuffer.Resize(width, height);
            _depthBuffer.Resize(width, height);

            _bitmap = new WriteableBitmap(Width, Height, 1.0, 1.0, PixelFormats.Bgra32, null);
            _frame.Source = _bitmap;
        }
    }
}
