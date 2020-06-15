using System;

namespace SpiceEngineCore.Rendering
{
    public class Resolution
    {
        private int _width;
        private int _height;

        public Resolution(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width
        {
            get => _width;
            set
            {
                _width = value;
                ResolutionChanged?.Invoke(this, new ResolutionEventArgs(_width, _height));
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                _height = value;
                ResolutionChanged?.Invoke(this, new ResolutionEventArgs(_width, _height));
            }
        }

        public float AspectRatio => (float)_width / _height;

        public event EventHandler<ResolutionEventArgs> ResolutionChanged;
    }
}
