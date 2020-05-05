using StarchUICore.Attributes.Sizes;
using StarchUICore.Attributes.Units;
using System.Collections.Generic;

namespace StarchUICore.Groups
{
    public abstract class Group : Element, IGroup
    {
        private List<IElement> _children = new List<IElement>();

        public IEnumerable<IElement> Children => _children;
        public int ChildCount => _children.Count;

        // TODO - Eventually, Spacing should be inherited either from the parent, or from a set theme/style
        public IUnits Spacing { get; set; } = Unit.Pixels(10);// Unit.Auto();

        public LayoutProgress LayoutProgress { get; set; } = new LayoutProgress();

        public void AddChild(IElement element)
        {
            _children.Add(element);
            element.Parent = this;
        }

        public IElement GetChildAt(int index) => _children[index];

        public override void Load()
        {
            foreach (var child in Children)
            {
                child.Load();
            }
        }

        public override void Update(int nTicks)
        {
            if (IsEnabled)
            {
                foreach (var child in Children)
                {
                    child.Update(nTicks);
                }
            }
        }

        public override void Draw()
        {
            if (IsVisible && Measurement.Width > 0 && Measurement.Height > 0)
            {
                foreach (var child in Children)
                {
                    child.Draw();
                }
            }
            // TODO - First, draw any group stuff (e.g. background, border, etc.)
            // Then, go through children and draw each one based on their previously measured sizes
        }

        protected override int GetRelativeX(LayoutInfo layoutInfo)
        {
            var anchorWidth = HorizontalAnchor.GetReferenceWidth(layoutInfo);

            // Apply our Position attribute to achieve this element's desired X
            var relativeX = Position.X.ToOffsetPixels(layoutInfo.AvailableValue, anchorWidth);

            // Pass this desired relative X back to the Anchor to reposition it appropriately
            relativeX = HorizontalAnchor.GetAnchorX(relativeX, Measurement, layoutInfo);

            // Apply the Minimum and Maximum constraints last, as these are HARD requirements
            relativeX = Position.MinimumX.ConstrainAsMinimum(relativeX, anchorWidth);
            relativeX = Position.MaximumX.ConstrainAsMaximum(relativeX, anchorWidth);

            LayoutProgress.SetX(Padding.Left.ToOffsetPixels(layoutInfo.AvailableValue, layoutInfo.ParentWidth));
            return relativeX;
        }

        protected override int GetRelativeY(LayoutInfo layoutInfo)
        {
            var anchorHeight = VerticalAnchor.GetReferenceHeight(layoutInfo);

            // Apply our Position attribute to achieve this element's desired Y
            var relativeY = Position.Y.ToOffsetPixels(layoutInfo.AvailableValue, anchorHeight);

            // Pass this desired relative Y back to the Anchor to reposition it appropriately
            relativeY = VerticalAnchor.GetAnchorY(relativeY, Measurement, layoutInfo);

            // Apply the Minimum and Maximum constraints last, as these are HARD requirements
            relativeY = Position.MinimumY.ConstrainAsMinimum(relativeY, anchorHeight);
            relativeY = Position.MaximumY.ConstrainAsMaximum(relativeY, anchorHeight);

            LayoutProgress.SetY(Padding.Top.ToOffsetPixels(layoutInfo.AvailableValue, layoutInfo.ParentHeight));
            return relativeY;
        }

        public override void ApplyCorrections(int widthChange, int heightChange, int xChange, int yChange)
        {
            foreach (var child in Children)
            {
                child.ApplyCorrections(widthChange, heightChange, xChange, yChange);
            }

            base.ApplyCorrections(widthChange, heightChange, xChange, yChange);
        }

        public abstract IGroup Duplicate();
    }
}
