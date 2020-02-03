using System;

namespace StarchUICore.Attributes.Positions
{
    public struct Anchor
    {
        public Anchor(bool left, bool top, bool right, bool bottom)
        {
            if (!left && !top && !right && !bottom) throw new ArgumentException("At least one anchor must be set");

            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public bool Left { get; }
        public bool Top { get; }
        public bool Right { get; }
        public bool Bottom { get; }

        public static Anchor Default() => new Anchor(true, true, false, false);
        public static Anchor ToLeft() => new Anchor(true, false, false, false);
        public static Anchor ToTop() => new Anchor(false, true, false, false);
        public static Anchor ToRight() => new Anchor(false, false, true, false);
        public static Anchor ToBottom() => new Anchor(false, false, false, true);
    }
}
