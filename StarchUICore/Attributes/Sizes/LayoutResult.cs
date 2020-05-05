namespace StarchUICore.Attributes.Sizes
{
    public struct LayoutResult
    {
        public LayoutResult(int? x, int? y, int? width, int? height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public int? X { get; }
        public int? Y { get; }
        public int? Width { get; }
        public int? Height { get; }

        public bool IsLocated => X.HasValue && Y.HasValue;
        public bool IsMeasured => Width.HasValue && Height.HasValue;
        public bool IsLaidOut => IsLocated && IsMeasured;

        public static LayoutResult Empty() => new LayoutResult(null, null, null, null);
    }
}
