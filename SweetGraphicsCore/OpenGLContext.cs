using SpiceEngine.GLFWBindings;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.HID;
using SpiceEngineCore.Rendering;

namespace SweetGraphicsCore
{
    public class OpenGLContext : IRenderContext
    {
        private IWindowContext _windowContext;

        public OpenGLContext(IWindowContext windowContext)
        {
            _windowContext = windowContext;

            MakeCurrent();
            GL.LoadFunctions();
            GL.ClearColor(Color4.Black);
        }

        public bool IsDisposed { get; private set; }

        public void MakeCurrent() => _windowContext.MakeCurrent();

        public void Update() => _windowContext.SwapBuffers();

        public void Dispose() { }
    }
}
