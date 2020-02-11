using System;

namespace StarchUICore.Animations
{
    public class TranslationAnimation : Animation
    {
        public TranslationAnimation(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        protected override void Update(float percent)
        {
            var x = (int)Math.Floor(X * percent);
            var y = (int)Math.Floor(Y * percent);

            _element.Location.Translate(x, y);
        }
    }
}
