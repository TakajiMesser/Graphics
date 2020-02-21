using SpiceEngineCore.Utilities;
using StarchUICore.Attributes.Positions;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Attributes.Units;

namespace StarchUICore.Groups
{
    public class RowGroup : Group
    {
        protected override LayoutResult OnLayout(LayoutInfo layoutInfo)
        {
            var width = GetMeasuredWidth(layoutInfo.AvailableWidth, layoutInfo.ParentWidth);
            var height = GetMeasuredHeight(layoutInfo.AvailableHeight, layoutInfo.ParentHeight);

            var relativeX = GetRelativeX(layoutInfo.RelativeX, layoutInfo.ParentAbsoluteX, layoutInfo.AvailableWidth, layoutInfo.ParentWidth, width);
            var relativeY = GetRelativeY(layoutInfo.RelativeY, layoutInfo.ParentAbsoluteY, layoutInfo.AvailableHeight, layoutInfo.ParentHeight, height);

            var absoluteX = GetAbsoluteX(layoutInfo.ParentAbsoluteX, relativeX, width);
            var absoluteY = GetAbsoluteY(layoutInfo.ParentAbsoluteY, relativeY, height);

            if (width.HasValue && height.HasValue && absoluteX.HasValue && absoluteY.HasValue)
            {
                var remainingWidth = width.Value - Padding.GetWidth(layoutInfo.AvailableWidth, layoutInfo.ParentWidth);
                var remainingHeight = height.Value - Padding.GetHeight(layoutInfo.AvailableHeight, layoutInfo.ParentHeight);

                var currentRelativeX = relativeX.Value;
                var currentRelativeY = relativeY.Value;

                var largestChildHeight = 0;
                var spacing = Spacing.ToDimensionPixels(layoutInfo.AvailableWidth, layoutInfo.ParentWidth);

                foreach (var child in Children)
                {
                    var childLayout = new LayoutInfo(remainingWidth, remainingHeight, width.Value, height.Value, currentRelativeX, currentRelativeY, absoluteX.Value, absoluteY.Value);
                    child.Layout(childLayout);

                    // Was the child successfully laid out?
                    if (child.IsLaidOut)
                    {
                        // Determine how far off our requested width is from the actual width that the child reported back
                        var xDifference = child.Location.X - absoluteX.Value;

                        // Update our remaining width based on the child's actual width
                        remainingWidth -= child.Measurement.Width + xDifference;

                        // Update our current X from the child's reported absolute X
                        currentRelativeX = child.Measurement.Width + xDifference;
                        currentRelativeX += spacing;

                        remainingWidth.ClampBottom(0);

                        // TODO - What if the child reports a different Y than we expected? What if the child wants to be centered (Auto Y Units)?
                        // If this group should auto-size by height, we need to keep track of the shortest child height
                        if (Size.Height is AutoUnits && child.Measurement.Width > 0 && child.Measurement.Height > largestChildHeight)
                        {
                            largestChildHeight = child.Measurement.Height;
                        }
                    }
                    else
                    {
                        // We need to halt this group's layout, and report back what we can
                        if (!child.Measurement.NeedsMeasuring && (Size.Height is AutoUnits))
                        {
                            return new LayoutResult(null, null, null, height);
                        }
                        else
                        {
                            return new LayoutResult();
                        }
                    }
                }

                // TODO - Also need to handle AUTO Position units here...
                // If this RowGroup is meant to fit its content, then we should correct the width here
                if (Size.Width is AutoUnits)
                {
                    width -= remainingWidth;
                }

                // If the RowGroup is meant to fit its content, we want to determine how much "extra" height we should shave off
                // due to the smallest child height being less than what we expected (remainingHeight)
                if (Size.Height is AutoUnits)
                {
                    var yDifference = (remainingHeight - largestChildHeight).ClampBottom(0);
                    height -= yDifference;
                }

                // Now that we have measured all of the children as well, reapply the necessary size constraints
                if (!(Size.MaximumWidth is AutoUnits))
                {
                    width = width.Value.ClampTop(Size.MaximumWidth.ToDimensionPixels(layoutInfo.AvailableWidth, layoutInfo.ParentWidth));
                }

                if (!(Size.MinimumHeight is AutoUnits))
                {
                    height = height.Value.ClampBottom(Size.MinimumHeight.ToDimensionPixels(layoutInfo.AvailableHeight, layoutInfo.ParentHeight));
                }

                return new LayoutResult(absoluteX, absoluteY, width, height);
            }
            else
            {
                return new LayoutResult();
            }
        }

        protected override MeasuredSize OnMeasure(MeasuredSize availableSize)
        {
            /*var width = Size.Width.Constrain(availableSize.Width, availableSize.ContainingWidth);
            var height = Size.Height.Constrain(availableSize.Height, availableSize.ContainingHeight);

            var remainingWidth = width - Padding.GetWidth(availableSize.Width, availableSize.ContainingWidth);
            var remainingHeight = height - Padding.GetHeight(availableSize.Height, availableSize.ContainingHeight);

            foreach (var child in Children)
            {
                var remainingSize = new MeasuredSize(remainingWidth, remainingHeight, width, height);
                child.Measure(remainingSize);

                var measurement = child.Measurement;
                remainingWidth -= measurement.Width;

                if (remainingWidth < 0)
                {
                    remainingWidth = 0;
                }
            }

            if (Size.Width is AutoUnits)
            {
                width -= remainingWidth;
            }

            return new MeasuredSize(width, remainingHeight);*/
            return new MeasuredSize();
        }

        protected override LocatedPosition OnLocate(LocatedPosition availablePosition)
        {
            throw new System.NotImplementedException();
        }

        public override IGroup Duplicate()
        {
            throw new System.NotImplementedException();
        }
    }
}
