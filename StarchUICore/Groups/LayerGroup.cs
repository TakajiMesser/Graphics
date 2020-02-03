using StarchUICore.Attributes.Positions;
using StarchUICore.Attributes.Sizes;

namespace StarchUICore.Groups
{
    public class LayerGroup : Group
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
            }

            return new MeasuredSize(remainingWidth, remainingHeight);
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
