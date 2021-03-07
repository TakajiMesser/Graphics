using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.Utilities;
using SpiceEngineCore.Rendering;

namespace SweetGraphicsCore
{
    /*public class OpenGLContext : IRenderContext
    {
        private GraphicsContext _context;
        private IWindowInfo _windowInfo;

        public OpenGLContext()
        {
            _windowInfo = windowInfo;
            _context = new GraphicsContext(GraphicsMode.Default, windowInfo, majorVersion, minorVersion, GraphicsContextFlags.ForwardCompatible);
            _context.MakeCurrent(windowInfo);
            _context.LoadAll();
        }

        public bool IsDisposed { get; private set; }

        public void MakeCurrent() => _context.MakeCurrent(_windowInfo);

        public void Update() => _context.Update(_windowInfo);

        public void SwapBuffers() => _context.SwapBuffers();

        public void Dispose() => _context.Dispose();




        /*bool IsDisposed { get; }

        void MakeCurrent();
        void Update();
        void SwapBuffers();*
    }*/
}
