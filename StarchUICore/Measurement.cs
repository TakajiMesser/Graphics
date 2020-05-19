using System;

namespace StarchUICore
{
    public class Measurement
    {
        public Measurement(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public bool NeedsXMeasuring { get; private set; }
        public bool NeedsYMeasuring { get; private set; }
        public bool NeedsWidthMeasuring { get; private set; }
        public bool NeedsHeightMeasuring { get; private set; }

        public bool NeedsMeasuring => NeedsXMeasuring || NeedsYMeasuring || NeedsWidthMeasuring || NeedsHeightMeasuring;

        public void SetX(int value)
        {
            X = value;
            NeedsXMeasuring = false;
        }

        public void SetY(int value)
        {
            Y = value;
            NeedsYMeasuring = false;
        }

        public void SetWidth(int value)
        {
            Width = value;
            NeedsWidthMeasuring = false;
        }

        public void SetHeight(int value)
        {
            Height = value;
            NeedsHeightMeasuring = false;
        }

        public void SetValue(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;

            NeedsXMeasuring = false;
            NeedsYMeasuring = false;
            NeedsWidthMeasuring = false;
            NeedsHeightMeasuring = false;
        }

        public void InvalidateX() => NeedsXMeasuring = true;
        public void InvalidateY() => NeedsYMeasuring = true;
        public void InvalidateWidth() => NeedsWidthMeasuring = true;
        public void InvalidateHeight() => NeedsHeightMeasuring = true;

        public void Invalidate()
        {
            InvalidateX();
            InvalidateY();
            InvalidateWidth();
            InvalidateHeight();
        }

        public void Translate(int x, int y)
        {
            X += x;
            Y += y;
        }

        public void Scale(float width, float height)
        {
            Width = (int)Math.Floor(Width * width);
            Height = (int)Math.Floor(Height * height);
        }

        public static Measurement Empty => new Measurement(0, 0, 0, 0)
        {
            NeedsXMeasuring = true,
            NeedsYMeasuring = true,
            NeedsWidthMeasuring = true,
            NeedsHeightMeasuring = true
        };
    }
}
