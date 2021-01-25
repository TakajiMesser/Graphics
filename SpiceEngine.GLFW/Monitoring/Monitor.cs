using System;

namespace SpiceEngine.GLFW.Monitoring
{
    public struct Monitor
    {
        private IntPtr _handle;

        public Monitor(IntPtr handle) => _handle = handle;

        public static Monitor None => new Monitor(IntPtr.Zero);

        public static implicit operator IntPtr(Monitor monitor) => monitor._handle;

        public static explicit operator Monitor(IntPtr handle) => new Monitor(handle);
    }
}
