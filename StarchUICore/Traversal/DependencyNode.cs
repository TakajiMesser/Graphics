using StarchUICore.Attributes.Positions;
using StarchUICore.Attributes.Units;
using StarchUICore.Groups;
using System.Collections.Generic;
using System.Linq;

namespace StarchUICore.Traversal
{
    public class DependencyNode
    {
        public DependencyNode(IElement element, LayoutSteps step)
        {
            Element = element;
            Key = new LayoutDependency(element.EntityID, step);
            Dependencies.AddRange(GetDependencies());
        }

        public IElement Element { get; }
        public LayoutDependency Key { get; }

        public List<LayoutDependency> Dependencies { get; } = new List<LayoutDependency>();

        private IEnumerable<LayoutDependency> GetDependencies()
        {
            switch (Key.Step)
            {
                case LayoutSteps.X:
                    return GetXDependencies(Element);
                case LayoutSteps.Y:
                    return GetYDependencies(Element);
                case LayoutSteps.Width:
                    return GetWidthDependencies(Element);
                case LayoutSteps.Height:
                    return GetHeightDependencies(Element);
            }

            return Enumerable.Empty<LayoutDependency>();
        }

        private IEnumerable<LayoutDependency> GetXDependencies(IElement element)
        {
            var anchorID = GetReferenceID(element, element.HorizontalAnchor.RelativeElement);

            if (!(element.Position.X is AutoUnits) || !(element.Position.MinimumX is AutoUnits) || !(element.Position.MaximumX is AutoUnits))
            {
                if (element.HorizontalAnchor.SelfAnchorType == AnchorTypes.Center || element.HorizontalAnchor.SelfAnchorType == AnchorTypes.End)
                {
                    yield return LayoutDependency.Width(element.EntityID);
                }

                if (anchorID.HasValue)
                {
                    yield return LayoutDependency.X(anchorID.Value);

                    if (element.HorizontalAnchor.RelativeAnchorType == AnchorTypes.Center || element.HorizontalAnchor.RelativeAnchorType == AnchorTypes.End
                    || element.Position.X is PercentUnits || element.Position.MinimumX is PercentUnits || element.Position.MaximumX is PercentUnits)
                    {
                        yield return LayoutDependency.Width(anchorID.Value);
                    }
                }
            }
        }

        private IEnumerable<LayoutDependency> GetYDependencies(IElement element)
        {
            var anchorID = GetReferenceID(element, element.VerticalAnchor.RelativeElement);

            if (!(element.Position.Y is AutoUnits) || !(element.Position.MinimumY is AutoUnits) || !(element.Position.MaximumY is AutoUnits))
            {
                if (element.VerticalAnchor.SelfAnchorType == AnchorTypes.Center || element.VerticalAnchor.SelfAnchorType == AnchorTypes.End)
                {
                    yield return LayoutDependency.Height(element.EntityID);
                }

                if (anchorID.HasValue)
                {
                    yield return LayoutDependency.Y(anchorID.Value);

                    if (element.VerticalAnchor.RelativeAnchorType == AnchorTypes.Center || element.VerticalAnchor.RelativeAnchorType == AnchorTypes.End
                    || element.Position.Y is PercentUnits || element.Position.MinimumY is PercentUnits || element.Position.MaximumY is PercentUnits)
                    {
                        yield return LayoutDependency.Height(anchorID.Value);
                    }
                }
            }
        }

        private IEnumerable<LayoutDependency> GetWidthDependencies(IElement element)
        {
            var dockID = GetReferenceID(element, element.HorizontalDock.RelativeElement);

            if (element is Group group && element.Size.Width is AutoUnits)
            {
                // In this case, this group's width is reliant on the width of all of its children...
                foreach (var child in group.Children)
                {
                    yield return LayoutDependency.Width(child.EntityID);
                }
            }

            if (dockID.HasValue && (element.Size.Width is PercentUnits || element.Size.MinimumWidth is PercentUnits || element.Size.MaximumWidth is PercentUnits))
            {
                yield return LayoutDependency.Width(dockID.Value);
            }
        }

        private IEnumerable<LayoutDependency> GetHeightDependencies(IElement element)
        {
            var dockID = GetReferenceID(element, element.VerticalDock.RelativeElement);

            if (element is Group group && element.Size.Height is AutoUnits)
            {
                // In this case, this group's height is reliant on the height of all of its children...
                foreach (var child in group.Children)
                {
                    yield return LayoutDependency.Height(child.EntityID);
                }
            }

            if (dockID.HasValue && (element.Size.Height is PercentUnits || element.Size.MinimumHeight is PercentUnits || element.Size.MaximumHeight is PercentUnits))
            {
                yield return LayoutDependency.Height(dockID.Value);
            }
        }

        private int? GetReferenceID(IElement element, IElement relativeElement)
        {
            if (relativeElement != null)
            {
                return relativeElement.EntityID;
            }
            else if (element.Parent != null)
            {
                return element.Parent.EntityID;
            }

            return null;
        }
    }
}
