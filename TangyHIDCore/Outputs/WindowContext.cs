using SpiceEngine.GLFWBindings;
using SpiceEngineCore.HID;

namespace TangyHIDCore.Outputs
{
    public class WindowContext : IWindowContext
    {
        private SpiceEngine.GLFWBindings.Windowing.Window _windowHandle;

        public WindowContext(SpiceEngine.GLFWBindings.Windowing.Window windowHandle) => _windowHandle = windowHandle;

        public bool IsDisposed { get; private set; }

        public void Initialize()
        {

        }

        public void MakeCurrent() => GLFW.MakeContextCurrent(_windowHandle);

        public void SwapBuffers() => GLFW.SwapBuffers(_windowHandle);

        public void Dispose() { }
    }
}
