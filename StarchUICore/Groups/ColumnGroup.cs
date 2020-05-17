using StarchUICore.Attributes.Positions;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Attributes.Units;

namespace StarchUICore.Groups
{
    public class ColumnGroup : Group
    {
        protected override int GetMeasuredWidth(LayoutInfo layoutInfo)
        {
            var dockWidth = HorizontalDock.GetReferenceWidth(layoutInfo);
            var constrainedWidth = Size.Width.ToDimensionPixels(layoutInfo.AvailableValue, dockWidth);

            if (Size.Width is AutoUnits)
            {
                // We need to shrink down to match the width of our largest child
                var contentWidth = Padding.GetWidth(layoutInfo.AvailableValue, layoutInfo.ParentWidth);
                var largestChildWidth = 0;

                foreach (var child in Children)
                {
                    if (child.Measurement.Width > largestChildWidth)
                    {
                        largestChildWidth = child.Measurement.Width;
                    }
                }

                contentWidth += largestChildWidth;

                if (constrainedWidth > contentWidth)
                {
                    constrainedWidth = contentWidth;
                }
            }

            constrainedWidth = Size.MinimumWidth.ConstrainAsMinimum(constrainedWidth, dockWidth);
            constrainedWidth = Size.MaximumWidth.ConstrainAsMinimum(constrainedWidth, dockWidth);

            LayoutProgress.SetWidth(constrainedWidth - Padding.GetWidth(layoutInfo.AvailableValue, layoutInfo.ParentWidth));
            return constrainedWidth;
        }

        protected override int GetMeasuredHeight(LayoutInfo layoutInfo)
        {
            var dockHeight = VerticalDock.GetReferenceHeight(layoutInfo);
            var constrainedHeight = Size.Height.ToDimensionPixels(layoutInfo.AvailableValue, dockHeight);

            if (Size.Height is AutoUnits)
            {
                // We need to shrink down to match the height of our contents
                var contentHeight = Padding.GetHeight(layoutInfo.AvailableValue, layoutInfo.ParentHeight);
                var spacing = Spacing.ToDimensionPixels(layoutInfo.AvailableValue, layoutInfo.ParentHeight);

                contentHeight += spacing * (ChildCount - 1);

                foreach (var child in Children)
                {
                    contentHeight += child.Measurement.Height;
                }

                if (constrainedHeight > contentHeight)
                {
                    constrainedHeight = contentHeight;
                }
            }

            constrainedHeight = Size.MinimumHeight.ConstrainAsMinimum(constrainedHeight, dockHeight);
            constrainedHeight = Size.MaximumHeight.ConstrainAsMinimum(constrainedHeight, dockHeight);

            LayoutProgress.SetHeight(constrainedHeight - Padding.GetHeight(layoutInfo.AvailableValue, layoutInfo.ParentHeight));
            LayoutProgress.SetSpacing(Spacing.ToDimensionPixels(layoutInfo.AvailableValue, layoutInfo.ParentWidth));
            return constrainedHeight;
        }

        public override IGroup Duplicate() => throw new System.NotImplementedException();
    }
}
