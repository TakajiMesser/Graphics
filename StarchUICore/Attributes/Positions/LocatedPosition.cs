namespace StarchUICore.Attributes.Positions
{
    public struct LocatedPosition
    {
        public LocatedPosition(int x, int y) : this(x, y, x, y) { }
        public LocatedPosition(int x, int y, int containerX, int containerY)
        {
            X = x;
            Y = y;
            ContainerX = containerX;
            ContainerY = containerY;
        }

        public int X { get; }
        public int Y { get; }
        public int ContainerX { get; }
        public int ContainerY { get; }
    }
}
