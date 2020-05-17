using StarchUICore.Attributes.Positions;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Attributes.Units;

namespace StarchUICore.Groups
{
    public class RowGroup : Group
    {
        protected override int GetMeasuredWidth(LayoutInfo layoutInfo)
        {
            var dockWidth = HorizontalDock.GetReferenceWidth(layoutInfo);
            var constrainedWidth = Size.Width.ToDimensionPixels(layoutInfo.AvailableValue, dockWidth);

            if (Size.Width is AutoUnits)
            {
                // We need to shrink down to match the width of our contents
                var contentWidth = Padding.GetWidth(layoutInfo.AvailableValue, layoutInfo.ParentWidth);
                var spacing = Spacing.ToDimensionPixels(layoutInfo.AvailableValue, layoutInfo.ParentWidth);

                contentWidth += spacing * (ChildCount - 1);

                foreach (var child in Children)
                {
                    contentWidth += child.Measurement.Width;
                }

                if (constrainedWidth > contentWidth)
                {
                    constrainedWidth = contentWidth;
                }
            }

            constrainedWidth = Size.MinimumWidth.ConstrainAsMinimum(constrainedWidth, dockWidth);
            constrainedWidth = Size.MaximumWidth.ConstrainAsMinimum(constrainedWidth, dockWidth);

            LayoutProgress.SetWidth(constrainedWidth - Padding.GetWidth(layoutInfo.AvailableValue, layoutInfo.ParentWidth));
            LayoutProgress.SetSpacing(Spacing.ToDimensionPixels(layoutInfo.AvailableValue, layoutInfo.ParentWidth));
            return constrainedWidth;
        }

        protected override int GetMeasuredHeight(LayoutInfo layoutInfo)
        {
            var dockHeight = VerticalDock.GetReferenceHeight(layoutInfo);
            var constrainedHeight = Size.Height.ToDimensionPixels(layoutInfo.AvailableValue, dockHeight);

            if (Size.Height is AutoUnits)
            {
                // We need to shrink down to match the height of our largest child
                var contentHeight = Padding.GetHeight(layoutInfo.AvailableValue, layoutInfo.ParentHeight);
                var largestChildHeight = 0;

                foreach (var child in Children)
                {
                    if (child.Measurement.Height > largestChildHeight)
                    {
                        largestChildHeight = child.Measurement.Height;
                    }
                }

                contentHeight += largestChildHeight;

                if (constrainedHeight > contentHeight)
                {
                    constrainedHeight = contentHeight;
                }
            }

            constrainedHeight = Size.MinimumHeight.ConstrainAsMinimum(constrainedHeight, dockHeight);
            constrainedHeight = Size.MaximumHeight.ConstrainAsMinimum(constrainedHeight, dockHeight);

            LayoutProgress.SetHeight(constrainedHeight - Padding.GetHeight(layoutInfo.AvailableValue, layoutInfo.ParentHeight));
            return constrainedHeight;
        }

        public override IGroup Duplicate() => throw new System.NotImplementedException();
    }
}
