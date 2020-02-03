namespace StarchUICore.Attributes.Positions
{
    public struct LocatedPosition
    {
        public LocatedPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }
    }
}
