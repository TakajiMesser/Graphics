using SpiceEngineCore.Utilities;
using StarchUICore.Attributes.Positions;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Attributes.Units;

namespace StarchUICore.Groups
{
    public class ColumnGroup : Group
    {
        protected override LayoutResult OnLayout(LayoutInfo layoutInfo)
        {
            var width = GetMeasuredWidth(layoutInfo.AvailableWidth, layoutInfo.ParentWidth);
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
            }
        }

        protected override MeasuredSize OnMeasure(MeasuredSize availableSize)
        {
            /*var width = Size.Width.ToDimensionPixels(availableSize.Width, availableSize.ContainingWidth);
            var height = Size.Height.ToDimensionPixels(availableSize.Height, availableSize.ContainingHeight);

            var remainingWidth = width - Padding.GetWidth(availableSize.Width, availableSize.ContainingWidth);
            var remainingHeight = height - Padding.GetHeight(availableSize.Height, availableSize.ContainingHeight);

            foreach (var child in Children)
            {
                var remainingSize = new MeasuredSize(remainingWidth, remainingHeight, width, height);
                child.Measure(remainingSize);

                var measurement = child.Measurement;
                remainingHeight -= measurement.Height;

                if (remainingHeight < 0)
                {
                    remainingHeight = 0;
                }
            }

            if (Size.Height is AutoUnits)
            {
                height -= remainingHeight;
            }

            return new MeasuredSize(remainingWidth, height);*/
            return new MeasuredSize();
        }

        protected override LocatedPosition OnLocate(LocatedPosition availablePosition) => throw new System.NotImplementedException();

        public override IGroup Duplicate() => throw new System.NotImplementedException();
    }
}
