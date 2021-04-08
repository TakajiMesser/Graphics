using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngine.Maps;
using SpiceEngine.Rendering;
using SpiceEngineCore.Game;
using SpiceEngineCore.Game.Settings;
using SpiceEngineCore.HID;
using SpiceEngineCore.Rendering;
using SweetGraphicsCore.Buffers;
using SweetGraphicsCore.Selection;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TangyHIDCore.Outputs;
using PixelFormat = SpiceEngine.GLFWBindings.GLEnums.PixelFormat;

namespace SpiceEngine.Game
{
    public class FrameWindow : NativeWindow
    {
        private Image _frame;
        private GameLoader _gameLoader;

        private IRenderContext _renderContext;
        private FrameBuffer _frameBuffer;
        private RenderBuffer _colorBuffer;
        private RenderBuffer _depthBuffer;
        private WriteableBitmap _bitmap;

        public FrameWindow(IWindowConfig configuration, IWindowContextFactory windowFactory, Image frame) : base(configuration, windowFactory) => _frame = frame;

        public Map Map { get; set; }

        public void LoadBuffers(IRenderContext renderContext)
        {
            _renderContext = renderContext;

            _colorBuffer = new RenderBuffer(_renderContext, Width, Height, RenderbufferTarget.Renderbuffer, InternalFormat.Rgba);
            _colorBuffer.Load();

            _depthBuffer = new RenderBuffer(_renderContext, Width, Height, RenderbufferTarget.Renderbuffer, InternalFormat.DepthComponent);
            _depthBuffer.Load();

            _frameBuffer = new FrameBuffer(_renderContext);
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment0, _colorBuffer);
            _frameBuffer.Add(FramebufferAttachment.DepthAttachment, _depthBuffer);
            _frameBuffer.Load();

            _bitmap = new WriteableBitmap(Width, Height, 1.0, 1.0, PixelFormats.Bgra32, null);
            _frame.Source = _bitmap;
        }

        private SimulationManager _simulationManager;
        private EditorRenderManager _renderManager;
        private ISelectionProvider _selectionProvider;
        private PanelCamera _panelCamera;
        private RenderModes _renderMode;

        public void LoadSimulation(SimulationManager simulationManager, EditorRenderManager renderManager, ISelectionProvider selectionProvider, PanelCamera panelCamera, RenderModes renderMode)
        {
            _simulationManager = simulationManager;
            _renderManager = renderManager;
            _selectionProvider = selectionProvider;
            _panelCamera = panelCamera;
            _renderMode = renderMode;
        }

        public override async Task LoadAsync()
        {
            _panelCamera.GridRenderer = _renderManager;
            _panelCamera.Load();

            _renderManager.RenderMode = _renderMode;
            _renderManager.SetEntityProvider(_simulationManager.EntityProvider);
            _renderManager.SetAnimationProvider(_simulationManager.AnimationSystem);
            _renderManager.SetUIProvider(_simulationManager.UISystem);
            _renderManager.SetSelectionProvider(_selectionProvider);
            _renderManager.LoadFromMap(Map);

            _simulator = _simulationManager;
            _renderer = _renderManager;
        }

        // TODO - We don't want to update the simulator from every viewport...
        protected override void Update(double elapsedMilliseconds = 0.0) { } // => _simulator.Tick();

        protected override void Render(double elapsedMilliseconds = 0.0)
        {
            base.Render(elapsedMilliseconds);
            UpdateFrame();
        }

        public override void ForceUpdate()
        {
            /*if (UpdateStyle == UpdateStyles.Manual)
            {
                Render();
            }*/

            if (!IsRunning)
            {
                //MakeCurrent();
                ProcessEvents();
                SwapBuffers();
                UpdateFrame();
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
