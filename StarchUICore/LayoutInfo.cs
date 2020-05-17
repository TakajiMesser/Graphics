namespace StarchUICore
{
    public class LayoutInfo
    {
        public LayoutInfo(int availableValue, int parentX, int parentY, int parentWidth, int parentHeight)
        {
            AvailableValue = availableValue;

            ParentX = parentX;
            ParentY = parentY;
            ParentWidth = parentWidth;
            ParentHeight = parentHeight;
        }

        public int AvailableValue { get; }

        public int ParentX { get; }
        public int ParentY { get; }
        public int ParentWidth { get; }
        public int ParentHeight { get; }
    }
}
