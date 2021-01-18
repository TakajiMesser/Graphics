using OpenTK.Graphics;
using OpenTK.Platform;

namespace SweetGraphicsCore
{
    public class OpenGLContext
    {
        private GraphicsContext _context;
        private IWindowInfo _windowInfo;

        public OpenGLContext(IWindowInfo windowInfo, int majorVersion, int minorVersion)
        {
            _windowInfo = windowInfo;
            _context = new GraphicsContext(GraphicsMode.Default, windowInfo, majorVersion, minorVersion, GraphicsContextFlags.ForwardCompatible);
            _context.MakeCurrent(windowInfo);
            _context.LoadAll();
        }

        public void MakeCurrent() => _context.MakeCurrent(_windowInfo);

        public void Update() => _context.Update(_windowInfo);

        public void SwapBuffers() => _context.SwapBuffers();

        public void Dispose() => _context.Dispose();
    }
}
