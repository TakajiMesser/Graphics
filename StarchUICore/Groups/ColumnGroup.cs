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

                var largestChildWidth = 0;
                var spacing = Spacing.ToDimensionPixels(layoutInfo.AvailableHeight, layoutInfo.ParentHeight);

                for (var i = 0; i < ChildCount; i++)
                {
                    var child = GetChildAt(i);
                    child.TabCount = TabCount + 1;

                    var childLayout = new LayoutInfo(remainingWidth, remainingHeight, width.Value, height.Value, currentRelativeX, currentRelativeY, absoluteX.Value, absoluteY.Value);
                    child.Layout(childLayout);

                    if (child.IsLaidOut)
                    {
                        var yDifference = currentRelativeY - child.Location.Y - absoluteY.Value;
                        remainingHeight = (remainingHeight - child.Measurement.Height + yDifference - (i < ChildCount - 1 ? spacing : 0)).ClampBottom(0);

                        currentRelativeY += child.Measurement.Height + yDifference + spacing;

                        if (Size.Width is AutoUnits && child.Measurement.Height > 0 && child.Measurement.Width > largestChildWidth)
                        {
                            largestChildWidth = child.Measurement.Width;
                        }
                    }
                    else
                    {
                        return !child.Measurement.NeedsMeasuring && Size.Width is AutoUnits
                            ? new LayoutResult(null, null, width, null)
                            : new LayoutResult();
                    }
                }

                var widthChange = 0;
                var heightChange = 0;
                var xChange = 0;
                var yChange = 0;

                if (Size.Width is AutoUnits)
                {
                    widthChange = -(remainingWidth - largestChildWidth).ClampBottom(0);
                    width += widthChange;
                }

                if (Size.Height is AutoUnits)
                {
                    heightChange = -remainingHeight;
                    height += heightChange;
                }

                if (!(Size.MinimumWidth is AutoUnits))
                {
                    width = width.Value.ClampBottom(Size.MinimumWidth.ToDimensionPixels(layoutInfo.AvailableWidth, layoutInfo.ParentWidth));
                }

                if (!(Size.MaximumHeight is AutoUnits))
                {
                    height = height.Value.ClampTop(Size.MaximumHeight.ToDimensionPixels(layoutInfo.AvailableHeight, layoutInfo.ParentHeight));
                }

                if (Size.Width is AutoUnits)
                {
                    var anchoredX = HorizontalAnchor.GetAnchorX(relativeX.Value, width.Value, layoutInfo.RelativeX, layoutInfo.RelativeX + layoutInfo.AvailableWidth, layoutInfo.ParentWidth, layoutInfo.ParentAbsoluteX);
                    var newAbsoluteX = anchoredX.HasValue ? layoutInfo.ParentAbsoluteX + anchoredX.Value : absoluteX;
                    xChange = newAbsoluteX.Value - absoluteX.Value;
                    absoluteX = newAbsoluteX;
                }

                if (Size.Height is AutoUnits)
                {
                    var anchoredY = VerticalAnchor.GetAnchorY(relativeY.Value, height.Value, layoutInfo.RelativeY, layoutInfo.RelativeY + layoutInfo.AvailableHeight, layoutInfo.ParentHeight, layoutInfo.ParentAbsoluteY);
                    var newAbsoluteY = anchoredY.HasValue ? layoutInfo.ParentAbsoluteY + anchoredY.Value : absoluteY;
                    yChange = newAbsoluteY.Value - absoluteY.Value;
                    absoluteY = newAbsoluteY;
                }

                // TODO - This is terrible... but for now, REPOSITION all children (Maybe this should be done via child event subscribing?
                foreach (var child in Children)
                {
                    child.ApplyCorrections(widthChange, heightChange, xChange, yChange);
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
