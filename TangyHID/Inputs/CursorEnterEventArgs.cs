using System;

namespace TangyHIDCore.Inputs
{
    public class CursorEnterEventArgs : EventArgs
    {
        public CursorEnterEventArgs(bool entered) => Entered = entered;

        public bool Entered { get; }
    }
}
