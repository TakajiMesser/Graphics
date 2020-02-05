using StarchUICore.Attributes.Positions;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Attributes.Units;
using System;

namespace StarchUICore.Groups
{
    public class RowGroup : Group
    {
        protected override LayoutResult OnLayout(LayoutInfo layoutInfo)
        {
            var relativeX = Position.X.GetValue(layoutInfo.Size.ContainingWidth);
            var relativeY = Position.Y.GetValue(layoutInfo.Size.ContainingHeight);

            var absoluteX = layoutInfo.Position.AbsoluteX + relativeX;
            var absoluteY = layoutInfo.Position.AbsoluteY + relativeY;

            var width = Size.Width.Constrain(layoutInfo.Size.Width, layoutInfo.Size.ContainingWidth);
            var height = Size.Height.Constrain(layoutInfo.Size.Height, layoutInfo.Size.ContainingHeight);

            var remainingWidth = width - Padding.GetWidth(layoutInfo.Size.Width, layoutInfo.Size.ContainingWidth);
            var remainingHeight = height - Padding.GetHeight(layoutInfo.Size.Height, layoutInfo.Size.ContainingHeight);

            foreach (var child in Children)
            {
                // TODO - Take Group Spacing into consideration here...
                // TODO - I might be double-adding the relative X to the absolute X here...
                var childSize = new MeasuredSize(remainingWidth, remainingHeight, width, height);
                var childPosition = new LocatedPosition(absoluteX + relativeX, absoluteY + relativeY, relativeX, relativeY);

                var childLayout = new LayoutInfo(childSize, childPosition);
                child.Layout(childLayout);

                // Update our remaining width based on the child's actual width
                var measurement = child.Measurement;
                remainingWidth -= measurement.Width;

                // Update our current X by using the child's actual reported X (keep in mind the returned value is ABSOLUTE, not relative)
                // TODO - If the returned absolute X position of the child differs from what we expected for the absolute X,
                //        we ALSO need to update the remaining width accordingly
                var location = child.Location;
                relativeX = (location.X - absoluteX) + measurement.Width;

                if (remainingWidth < 0)
                {
                    remainingWidth = 0;
                }
            }

            // TODO - Also need to handle AUTO Position units here...
            if (Size.Width is AutoUnits)
            {
                width -= remainingWidth;
            }

            return new LayoutResult(width, remainingHeight, absoluteX, absoluteY);
        }

        protected override MeasuredSize OnMeasure(MeasuredSize availableSize)
        {
            var width = Size.Width.Constrain(availableSize.Width, availableSize.ContainingWidth);
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

            return new MeasuredSize(width, remainingHeight);
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
