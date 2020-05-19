namespace StarchUICore
{
    public class LayoutProgress
    {
        public int CurrentX { get; private set; }
        public int CurrentY { get; private set; }

        public int RemainingWidth { get; private set; }
        public int RemainingHeight { get; private set; }

        public int Spacing { get; private set; }

        public bool NeedsSpacingMeasuring { get; private set; } = true;
        public bool NeedsWidthMeasuring { get; private set; } = true;
        public bool NeedsHeightMeasuring { get; private set; } = true;

        public void Invalidate()
        {
            NeedsSpacingMeasuring = true;
            NeedsWidthMeasuring = true;
            NeedsHeightMeasuring = true;
        }

        public void SetX(int value)
        {
            CurrentX = value;
        }

        public void SetY(int value)
        {
            CurrentY = value;
        }

        public void SetWidth(int value)
        {
            RemainingWidth = value;
            NeedsWidthMeasuring = false;
        }

        public void SetHeight(int value)
        {
            RemainingHeight = value;
            NeedsHeightMeasuring = false;
        }

        public void SetSpacing(int value)
        {
            Spacing = value;
            NeedsSpacingMeasuring = false;
        }

        public void UpdateWidth(int childWidth)
        {
            var consumedWidth = childWidth + Spacing;

            RemainingWidth -= childWidth;
            CurrentX += consumedWidth;
        }

        public void UpdateHeight(int childHeight)
        {
            var consumedHeight = childHeight + Spacing;

            RemainingHeight -= childHeight;
            CurrentY += consumedHeight;
        }
    }
}
