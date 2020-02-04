namespace StarchUICore.Attributes.Sizes
{
    public struct MeasuredSize
    {
        public MeasuredSize(int width, int height) : this(width, height, width, height) { }
        public MeasuredSize(int width, int height, int containingWidth, int containingHeight)
        {
            Width = width;
            Height = height;
            ContainingWidth = containingWidth;
            ContainingHeight = containingHeight;
        }

        public int Width { get; }
        public int Height { get; }
        public int ContainingWidth { get; }
        public int ContainingHeight { get; }
    }
}
