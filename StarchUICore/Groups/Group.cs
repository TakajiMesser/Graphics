using SpiceEngineCore.Rendering;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Attributes.Units;
using StarchUICore.Traversal;
using System.Collections.Generic;

namespace StarchUICore.Groups
{
    public abstract class Group : Element, IGroup
    {
        private List<IElement> _children = new List<IElement>();

        public Group(int entityID) : base(entityID) { }

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

        public override void Load(IRenderContext renderContext)
        {
            foreach (var child in Children)
            {
                child.Load(renderContext);
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

        public override IEnumerable<LayoutDependency> GetWidthDependencies()
        {
            if (Size.Width is AutoUnits)
            {
                // In this case, this group's width is reliant on the width of all of its children...
                foreach (var child in Children)
                {
                    yield return LayoutDependency.Width(child.EntityID);
                }
            }

            foreach (var dependency in base.GetWidthDependencies())
            {
                yield return dependency;
            }
        }

        public override IEnumerable<LayoutDependency> GetHeightDependencies()
        {
            if (Size.Height is AutoUnits)
            {
                // In this case, this group's height is reliant on the height of all of its children...
                foreach (var child in Children)
                {
                    yield return LayoutDependency.Height(child.EntityID);
                }
            }

            foreach (var dependency in base.GetHeightDependencies())
            {
                yield return dependency;
            }
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

        public abstract IGroup Duplicate();
    }
}
