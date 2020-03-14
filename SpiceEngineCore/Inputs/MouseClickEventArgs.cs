using OpenTK;
using System;

namespace SpiceEngineCore.Inputs
{
    public class MouseClickEventArgs : EventArgs
    {
        public Vector2 MouseCoordinates { get; }

        public MouseClickEventArgs(Vector2 mouseCoordinates) => MouseCoordinates = mouseCoordinates;
    }
}
