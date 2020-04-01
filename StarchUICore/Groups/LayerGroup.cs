using SpiceEngineCore.Utilities;
using StarchUICore.Attributes.Positions;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Attributes.Units;

namespace StarchUICore.Groups
{
    public class LayerGroup : Group
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

                var largestChildWidth = 0;
                var largestChildHeight = 0;

                foreach (var child in Children)
                {
                    var childLayout = new LayoutInfo(remainingWidth, remainingHeight, width.Value, height.Value, currentRelativeX, currentRelativeY, absoluteX.Value, absoluteY.Value);
                    child.Layout(childLayout);

                    if (child.IsLaidOut)
                    {
                        if (Size.Width is AutoUnits && child.Measurement.Height > 0 && child.Measurement.Width > largestChildWidth)
                        {
                            largestChildWidth = child.Measurement.Width;
                        }

                        if (Size.Height is AutoUnits && child.Measurement.Width > 0 && child.Measurement.Height > largestChildHeight)
                        {
                            largestChildHeight = child.Measurement.Height;
                        }
                    }
                    else
                    {
                        return !child.Measurement.NeedsMeasuring && Size.Height is AutoUnits
                            ? new LayoutResult(null, null, null, height)
                            : new LayoutResult();
                    }
                }

                if (Size.Width is AutoUnits)
                {
                    var xDifference = (remainingWidth - largestChildWidth).ClampBottom(0);
                    width -= xDifference;
                }

                if (Size.Height is AutoUnits)
                {
                    var yDifference = (remainingHeight - largestChildHeight).ClampBottom(0);
                    height -= yDifference;
                }

                if (!(Size.MaximumWidth is AutoUnits))
                {
                    width = width.Value.ClampTop(Size.MaximumWidth.ToDimensionPixels(layoutInfo.AvailableWidth, layoutInfo.ParentWidth));
                }

                if (!(Size.MinimumHeight is AutoUnits))
                {
                    height = height.Value.ClampBottom(Size.MinimumHeight.ToDimensionPixels(layoutInfo.AvailableHeight, layoutInfo.ParentHeight));
                }

                if (Size.Width is AutoUnits)
                {
                    var anchoredX = HorizontalAnchor.GetAnchorX(relativeX.Value, width.Value, layoutInfo.RelativeX, layoutInfo.RelativeX + layoutInfo.AvailableWidth, layoutInfo.ParentWidth, layoutInfo.ParentAbsoluteX);
                    var newAbsoluteX = anchoredX.HasValue ? layoutInfo.ParentAbsoluteX + anchoredX.Value : absoluteX;
                    var absoluteXDifference = newAbsoluteX - absoluteX;

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
                    var newAbsoluteY = anchoredY.HasValue ? layoutInfo.ParentAbsoluteY + anchoredY.Value : absoluteY;
                    var absoluteYDifference = newAbsoluteY - absoluteY;

                    foreach (var child in Children)
                    {
                        child.Location.SetValue(child.Location.X, child.Location.Y + absoluteYDifference.Value);
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

        protected override MeasuredSize OnMeasure(MeasuredSize availableSize) => throw new System.NotImplementedException();

        protected override LocatedPosition OnLocate(LocatedPosition availablePosition) => throw new System.NotImplementedException();

        public override IGroup Duplicate() => throw new System.NotImplementedException();
    }
}
