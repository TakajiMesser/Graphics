namespace StarchUICore.Attributes.Sizes
{
    public struct LayoutResult
    {
        public LayoutResult(int width, int height, int x, int y)
        {
            Width = width;
            Height = height;
            X = x;
            Y = y;
        }

        public int Width { get; }
        public int Height { get; }
        public int X { get; }
        public int Y { get; }
    }
}
