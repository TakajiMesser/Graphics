using OpenTK;
using System;

namespace TangyHIDCore
{
    public class MouseClickEventArgs : EventArgs
    {
        public Vector2 MouseCoordinates { get; }

        public MouseClickEventArgs(Vector2 mouseCoordinates) => MouseCoordinates = mouseCoordinates;
    }
}
