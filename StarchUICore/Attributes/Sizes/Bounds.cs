namespace StarchUICore.Attributes.Sizes
{
    public struct Bounds
    {
        public Bounds(int minimumX, int minimumY, int maximumX, int maximumY)
        {
            MinimumX = minimumX;
            MinimumY = minimumY;
            MaximumX = maximumX;
            MaximumY = maximumY;
        }

        public int MinimumX { get; }
        public int MinimumY { get; }
        public int MaximumX { get; }
        public int MaximumY { get; }
    }
}
