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

        protected override LayoutResult OnLayout(LayoutInfo layoutInfo)
        {
            /*var width = GetMeasuredWidth(layoutInfo.AvailableWidth, layoutInfo.ParentWidth);
            var height = GetMeasuredHeight(layoutInfo.AvailableHeight, layoutInfo.ParentHeight);

            var relativeX = GetRelativeX(layoutInfo.RelativeX, layoutInfo.ParentAbsoluteX, layoutInfo.AvailableWidth, layoutInfo.ParentWidth, width);
            var relativeY = GetRelativeY(layoutInfo.RelativeY, layoutInfo.ParentAbsoluteY, layoutInfo.AvailableHeight, layoutInfo.ParentHeight, height);

            var absoluteX = GetAbsoluteX(layoutInfo.ParentAbsoluteX, relativeX, width);
            var absoluteY = GetAbsoluteY(layoutInfo.ParentAbsoluteY, relativeY, height);

            Log(width, height, relativeX, relativeY, absoluteX, absoluteY);

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
                    child.TabCount = TabCount + 1;

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

                var widthChange = 0;
                var heightChange = 0;
                var xChange = 0;
                var yChange = 0;

                // TODO - Also need to handle AUTO Position units here...
                // If this RowGroup is meant to fit its content, then we should correct the width here
                if (Size.Width is AutoUnits)
                {
                    widthChange = -remainingWidth;
                    width += widthChange;
                }

                // If the RowGroup is meant to fit its content, we want to determine how much "extra" height we should shave off
                // due to the smallest child height being less than what we expected (remainingHeight)
                if (Size.Height is AutoUnits)
                {
                    heightChange = -(remainingHeight - largestChildHeight).ClampBottom(0);
                    height += heightChange;
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
                    xChange = newAbsoluteX.Value - absoluteX.Value;
                    absoluteX = newAbsoluteX;
                }

                if (Size.Height is AutoUnits)
                {
                    var anchoredY = VerticalAnchor.GetAnchorY(relativeY.Value, height.Value, layoutInfo.RelativeY, layoutInfo.RelativeY + layoutInfo.AvailableHeight, layoutInfo.ParentHeight, layoutInfo.ParentAbsoluteY);
                    //absoluteY = anchoredY.HasValue ? layoutInfo.ParentAbsoluteY + anchoredY.Value : absoluteY;
                    var newAbsoluteY = anchoredY.HasValue ? layoutInfo.ParentAbsoluteY + anchoredY.Value : absoluteY;
                    yChange = newAbsoluteY.Value - absoluteY.Value;

                    // TODO - This is terrible... but for now, REPOSITION all children (Maybe this should be done via child event subscribing?
                    /*foreach (var child in Children)
                    {
                        // TODO - Refactor layout process so that child reports RELATIVE position to parent, who then assigns the child its absolute position
                        // If this child based its position off of the size of its parent, then we need to reanchor its Y position now
                        var newChildY = /*!(child.Position.Y is AutoUnits) && *child.VerticalAnchor.RelativeElement == null
                            ? newAbsoluteY + child.VerticalAnchor.GetAnchorY(0, child.Measurement.Height, 0, height.Value, height.Value, newAbsoluteY.Value).Value
                            : child.Location.Y + absoluteYDifference.Value;

                        child.Location.SetValue(child.Location.X, newChildY.Value);
                        child.InvokeLayoutChange();
                    }*

                    absoluteY = newAbsoluteY;
                }

                // TODO - This is terrible... but for now, REPOSITION all children (Maybe this should be done via child event subscribing?
                foreach (var child in Children)
                {
                    if (child is Group)
                    {
                        child.ApplyCorrections(widthChange, heightChange, xChange, yChange);
                    }
                    else
                    {
                        child.ApplyCorrections(widthChange, heightChange, xChange, yChange);
                    }
                }

                return new LayoutResult(absoluteX, absoluteY, width, height);
            }
            else
            {
                return new LayoutResult();
            }*/
            return LayoutResult.Empty();
        }

        public override IGroup Duplicate() => throw new System.NotImplementedException();
    }
}
