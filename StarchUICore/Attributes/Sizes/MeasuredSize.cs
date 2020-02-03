namespace StarchUICore.Attributes.Sizes
{
    public struct MeasuredSize
    {
        public MeasuredSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; }
        public int Height { get; }
    }
}
