namespace StarchUICore.Attributes.Sizes
{
    public struct LayoutInfo
    {
        public LayoutInfo(int availableWidth, int availableHeight, int parentWidth, int parentHeight, int relativeX, int relativeY, int parentAbsoluteX, int parentAbsoluteY)
        {
            AvailableWidth = availableWidth;
            AvailableHeight = availableHeight;
            ParentWidth = parentWidth;
            ParentHeight = parentHeight;

            RelativeX = relativeX;
            RelativeY = relativeY;
            ParentAbsoluteX = parentAbsoluteX;
            ParentAbsoluteY = parentAbsoluteY;
        }

        public int AvailableWidth { get; }
        public int AvailableHeight { get; }
        public int ParentWidth { get; }
        public int ParentHeight { get; }

        public int RelativeX { get; }
        public int RelativeY { get; }
        public int ParentAbsoluteX { get; }
        public int ParentAbsoluteY { get; }
    }
}
