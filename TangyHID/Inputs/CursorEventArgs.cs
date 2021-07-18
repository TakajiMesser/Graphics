using System;

namespace TangyHIDCore.Inputs
{
    public class CursorEventArgs : EventArgs
    {
        public CursorEventArgs(double x, double y)
        {
            X = x;
            Y = y;
        }
        
        public double X { get; }
        public double Y { get; }
    }
}
