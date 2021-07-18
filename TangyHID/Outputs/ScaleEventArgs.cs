using System;

namespace TangyHIDCore.Outputs
{
    public class ScaleEventArgs : EventArgs
    {
        public ScaleEventArgs(float x, float y)
        {
            X = x;
            Y = y;
        }
        
        public float X { get; }
        public float Y { get; }
    }
}
