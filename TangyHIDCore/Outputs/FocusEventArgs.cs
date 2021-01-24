using System;

namespace TangyHIDCore.Outputs
{
    public class FocusEventArgs : EventArgs
    {
        public FocusEventArgs(bool isFocused) => IsFocused = isFocused;

        public bool IsFocused { get; }
    }
}
