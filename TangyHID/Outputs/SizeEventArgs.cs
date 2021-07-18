using System;

namespace TangyHIDCore.Outputs
{
    public class SizeEventArgs : EventArgs
    {
        public SizeEventArgs(int width, int height)
        {
            Width = width;
            Height = height;
        }
        
        public int Width { get; }
        public int Height { get; }
    }
}
