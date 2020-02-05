namespace StarchUICore.Attributes.Positions
{
    public struct LocatedPosition
    {
        public LocatedPosition(int absoluteX, int absoluteY) : this(absoluteX, absoluteY, absoluteX, absoluteY) { }
        public LocatedPosition(int absoluteX, int absoluteY, int relativeX, int relativeY)
        {
            AbsoluteX = absoluteX;
            AbsoluteY = absoluteY;
            RelativeX = relativeX;
            RelativeY = relativeY;
        }

        public int AbsoluteX { get; }
        public int AbsoluteY { get; }
        public int RelativeX { get; }
        public int RelativeY { get; }
    }
}
