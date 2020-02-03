namespace StarchUICore.Attributes.Sizes
{
    public class Measurement
    {
        public Measurement(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public bool NeedsMeasuring { get; private set; }

        public void SetValue(int width, int height)
        {
            Width = width;
            Height = height;
            NeedsMeasuring = false;
        }

        public void Invalidate() => NeedsMeasuring = true;

        public static Measurement Empty => new Measurement(0, 0)
        {
            NeedsMeasuring = true
        };
        //public override bool Equals(object obj) => obj is UnitPosition position && X == position.X && Y == position.Y;
    }
}
