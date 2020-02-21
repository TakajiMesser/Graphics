using StarchUICore.Attributes.Positions;
using StarchUICore.Attributes.Sizes;

namespace StarchUICore.Groups
{
    public class LayerGroup : Group
    {
        protected override LayoutResult OnLayout(LayoutInfo layoutInfo)
        {
            throw new System.NotImplementedException();
        }

        protected override MeasuredSize OnMeasure(MeasuredSize availableSize)
        {
            var width = Size.Width.ToDimensionPixels(availableSize.Width, availableSize.ContainingWidth);
            var height = Size.Height.ToDimensionPixels(availableSize.Height, availableSize.ContainingHeight);

            var remainingWidth = width - Padding.GetWidth(availableSize.Width, availableSize.ContainingWidth);
            var remainingHeight = height - Padding.GetHeight(availableSize.Height, availableSize.ContainingHeight);

            foreach (var child in Children)
            {
                var remainingsize = new MeasuredSize(remainingWidth, remainingHeight, width, height);
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
