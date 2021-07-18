using System;

namespace StarchUICore.Animations
{
    public class ScaleAnimation : Animation
    {
        public ScaleAnimation(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; set; }
        public int Height { get; set; }

        protected override void Update(float percent)
        {
            var width = (int)Math.Floor(Width * percent);
            var height = (int)Math.Floor(Height * percent);

            _element.Measurement.Scale(width, height);
        }
    }
}
