using StarchUICore.Attributes.Positions;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Attributes.Units;

namespace StarchUICore.Groups
{
    public class RowGroup : Group
    {
        protected override MeasuredSize OnMeasure(MeasuredSize availableSize)
        {
            var width = Size.Width.Constrain(availableSize.Width);
            var height = Size.Height.Constrain(availableSize.Height);

            var remainingWidth = width - Padding.GetWidth(availableSize.Width);
            var remainingHeight = height - Padding.GetHeight(availableSize.Height);

            foreach (var child in Children)
            {
                var remainingsize = new MeasuredSize(remainingWidth, remainingHeight);
                child.Measure(remainingsize);

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
