namespace StarchUICore.Attributes.Positions
{
    public class Location
    {
        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }

        public bool NeedsLocating { get; private set; }

        public void SetValue(int x, int y)
        {
            X = x;
            Y = y;
            NeedsLocating = false;
        }

        public void Invalidate() => NeedsLocating = true;

        public static Location Empty => new Location(0, 0)
        {
            NeedsLocating = true
        };
        //public override bool Equals(object obj) => obj is UnitPosition position && X == position.X && Y == position.Y;
    }
}
