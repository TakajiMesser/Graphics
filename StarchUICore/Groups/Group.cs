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

        /*protected int ConstrainWidth(MeasuredSize availableSize)
        {
            var width = Size.Width.Constrain(availableSize.Width, availableSize.ContainingWidth);
            var paddingWidth = Padding.GetWidth(availableSize.Width, availableSize.ContainingWidth);

            return width - paddingWidth;
        }

        protected int ConstrainHeight(MeasuredSize availableSize)
        {
            var height = Size.Height.Constrain(availableSize.Height, availableSize.ContainingHeight);
            var paddingHeight = Padding.GetHeight(availableSize.Height, availableSize.ContainingHeight);

            return height - paddingHeight;
        }*/

        /*protected override ISize GetMeasurement(ISize availableSize)
        {
            var width = 0;
            var height = 0;

            if (!IsGone)
            {
                if (availableSize is UnitSize availableUnitSize)
                {
                    var availableWidth = availableUnitSize.Width;
                    var availableHeight = availableUnitSize.Height;

                    if (Size is UnitSize unitSize)
                    {
                        if (unitSize.Width < availableWidth)
                        {
                            availableWidth = unitSize.Width;
                        }
                        
                        if (unitSize.Height < availableHeight)
                        {
                            availableHeight = unitSize.Height;
                        }
                    }

                    foreach (var child in GetChildren())
                    {
                        child.Measure(new UnitSize(availableWidth, availableHeight));

                        availableWidth -= child.Measurement.Width;
                        availableHeight -= child.Measurement.Height;

                        width += child.Measurement.Width;
                        height += child.Measurement.Height;
                    }

                    if (LayoutMode == LayoutModes.Fill)
                    {
                        width = availableUnitSize.Width;
                        height = availableUnitSize.Height;
                    }
                }
            }

            return new UnitSize(width, height);
        }

        protected override IPosition GetLocation(IPosition position)
        {
            var x = 0;
            var y = 0;

            if (position is UnitPosition unitPosition)
            {
                x = unitPosition.X;
                y = unitPosition.Y;
            }

            return new UnitPosition(x, y);
        }*/

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

        public abstract IGroup Duplicate();
    }
}
