using System;

namespace SpiceEngineCore.Rendering
{
    public class ResolutionEventArgs : EventArgs
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public float AspectRatio => (float)Width / Height;

        public ResolutionEventArgs(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
