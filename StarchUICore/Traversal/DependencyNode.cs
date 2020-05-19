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
                    return Element.GetXDependencies();
                case LayoutSteps.Y:
                    return Element.GetYDependencies();
                case LayoutSteps.Width:
                    return Element.GetWidthDependencies();
                case LayoutSteps.Height:
                    return Element.GetHeightDependencies();
            }

            return Enumerable.Empty<LayoutDependency>();
        }
    }
}
