using System;

namespace SpiceEngine.GLFWBindings.Windowing
{
    public struct Window
    {
        private IntPtr _handle;

        public Window(IntPtr handle) => _handle = handle;

        public static Window None => new Window(IntPtr.Zero);

        public static implicit operator IntPtr(Window window) => window._handle;

        public static explicit operator Window(IntPtr handle) => new Window(handle);
    }
}
