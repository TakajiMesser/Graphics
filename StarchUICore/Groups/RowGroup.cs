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

                var currentRelativeX = Padding.Left.ToOffsetPixels(0, layoutInfo.ParentWidth).Value;
                var currentRelativeY = Padding.Top.ToOffsetPixels(0, layoutInfo.ParentHeight).Value;

                var largestChildHeight = 0;
                var spacing = Spacing.ToDimensionPixels(layoutInfo.AvailableWidth, layoutInfo.ParentWidth);

                for (var i = 0; i < ChildCount; i++)
                {
                    var child = GetChildAt(i);

                    var childLayout = new LayoutInfo(remainingWidth, remainingHeight, width.Value, height.Value, currentRelativeX, currentRelativeY, absoluteX.Value, absoluteY.Value);
                    child.Layout(childLayout);

                    // Was the child successfully laid out?
                    if (child.IsLaidOut)
                    {
                        // Determine how far off our requested width is from the actual width that the child reported back
                        var xDifference = currentRelativeX - child.Location.X - absoluteX.Value; //child.Location.X - absoluteX.Value

                        // Update our remaining width based on the child's actual width
                        remainingWidth = (remainingWidth - child.Measurement.Width + xDifference - (i < ChildCount - 1 ? spacing : 0)).ClampBottom(0);

                        // Update our current X from the child's reported absolute X
                        currentRelativeX += child.Measurement.Width + xDifference + spacing;

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
                        return !child.Measurement.NeedsMeasuring && Size.Height is AutoUnits
                            ? new LayoutResult(null, null, null, height)
                            : new LayoutResult();
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

                // Now that we've potentially updated the width and height, we can reapply the anchors
                if (Size.Width is AutoUnits)
                {
                    var anchoredX = HorizontalAnchor.GetAnchorX(relativeX.Value, width.Value, layoutInfo.RelativeX, layoutInfo.RelativeX + layoutInfo.AvailableWidth, layoutInfo.ParentWidth, layoutInfo.ParentAbsoluteX);
                    //absoluteX = anchoredX.HasValue ? layoutInfo.ParentAbsoluteX + anchoredX.Value : absoluteX;
                    var newAbsoluteX = anchoredX.HasValue ? layoutInfo.ParentAbsoluteX + anchoredX.Value : absoluteX;
                    var absoluteXDifference = newAbsoluteX - absoluteX;

                    // TODO - This is terrible... but for now, REPOSITION all children (Maybe this should be done via child event subscribing?
                    foreach (var child in Children)
                    {
                        child.Location.SetValue(child.Location.X + absoluteXDifference.Value, child.Location.Y);
                        child.InvokeLayoutChange();
                    }

                    absoluteX = newAbsoluteX;
                }

                if (Size.Height is AutoUnits)
                {
                    var anchoredY = VerticalAnchor.GetAnchorY(relativeY.Value, height.Value, layoutInfo.RelativeY, layoutInfo.RelativeY + layoutInfo.AvailableHeight, layoutInfo.ParentHeight, layoutInfo.ParentAbsoluteY);
                    //absoluteY = anchoredY.HasValue ? layoutInfo.ParentAbsoluteY + anchoredY.Value : absoluteY;
                    var newAbsoluteY = anchoredY.HasValue ? layoutInfo.ParentAbsoluteY + anchoredY.Value : absoluteY;
                    var absoluteYDifference = newAbsoluteY - absoluteY;

                    // TODO - This is terrible... but for now, REPOSITION all children (Maybe this should be done via child event subscribing?
                    foreach (var child in Children)
                    {
                        // TODO - Refactor layout process so that child reports RELATIVE position to parent, who then assigns the child its absolute position
                        // If this child based its position off of the size of its parent, then we need to reanchor its Y position now
                        var newChildY = /*!(child.Position.Y is AutoUnits) && */child.VerticalAnchor.RelativeElement == null
                            ? newAbsoluteY + child.VerticalAnchor.GetAnchorY(0, child.Measurement.Height, 0, height.Value, height.Value, newAbsoluteY.Value).Value
                            : child.Location.Y + absoluteYDifference.Value;

                        child.Location.SetValue(child.Location.X, newChildY.Value);
                        child.InvokeLayoutChange();
                    }

                    absoluteY = newAbsoluteY;
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

        protected override LocatedPosition OnLocate(LocatedPosition availablePosition) => throw new System.NotImplementedException();

        public override IGroup Duplicate() => throw new System.NotImplementedException();
    }
}
