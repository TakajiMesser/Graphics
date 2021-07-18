using System;

namespace TangyHIDCore.Outputs
{
    public class MaximizeEventArgs : EventArgs
    {
        public MaximizeEventArgs(bool isMaximized) => IsMaximized = isMaximized;

        public bool IsMaximized { get; }
    }
}
