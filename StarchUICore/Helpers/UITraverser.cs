using SpiceEngineCore.Outputs;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Groups;
using StarchUICore.Traversal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StarchUICore.Helpers
{
    public class UITraverser
    {
        public UITraverser(Resolution resolution) => _resolution = resolution;

        // ANOTHER OPTION
        // The Group sends the available bounds to the child, and the child reports back its min-max location and measurement
        // The Group then takes these min-maxs of ALL of its children into account and assigns them each their final values
        private Resolution _resolution;

        private Dictionary<LayoutDependency, DependencyNode> _nodeByDependency = new Dictionary<LayoutDependency, DependencyNode>();
        private List<LayoutDependency> _dependencyOrder = new List<LayoutDependency>();

        private HashSet<LayoutDependency> _resolvedDependencies = new HashSet<LayoutDependency>();
        private HashSet<LayoutDependency> _pendingDependencies = new HashSet<LayoutDependency>();

        private bool _needsLayout = true;

        private void AddNode(DependencyNode node)
        {
            _nodeByDependency.Add(node.Key, node);
            _dependencyOrder.Add(node.Key);
        }

        public void GatherDependencyNodes(IElement element)
        {
            AddNode(new DependencyNode(element, LayoutSteps.X));
            AddNode(new DependencyNode(element, LayoutSteps.Y));
            AddNode(new DependencyNode(element, LayoutSteps.Width));
            AddNode(new DependencyNode(element, LayoutSteps.Height));

            if (element is Group group)
            {
                for (var i = 0; i < group.ChildCount; i++)
                {
                    var child = group.GetChildAt(i);
                    GatherDependencyNodes(child);

                    // Regardless of the type of Group, this child will be reliant on its parent location
                    var childXNode = _nodeByDependency[LayoutDependency.X(child.EntityID)];
                    var childYNode = _nodeByDependency[LayoutDependency.Y(child.EntityID)];

                    childXNode.Dependencies.Add(LayoutDependency.X(group.EntityID));
                    childYNode.Dependencies.Add(LayoutDependency.Y(group.EntityID));

                    if (group is RowGroup)
                    {
                        if (i > 0)
                        {
                            var siblingID = group.GetChildAt(i - 1).EntityID;

                            childXNode.Dependencies.Add(LayoutDependency.X(siblingID));
                            childXNode.Dependencies.Add(LayoutDependency.Width(siblingID));
                        }

                        childXNode.Dependencies.Add(LayoutDependency.Width(group.EntityID));
                    }
                    else if (group is ColumnGroup)
                    {
                        if (i > 0)
                        {
                            var siblingID = group.GetChildAt(i - 1).EntityID;

                            childYNode.Dependencies.Add(LayoutDependency.Y(siblingID));
                            childYNode.Dependencies.Add(LayoutDependency.Height(siblingID));
                        }

                        childYNode.Dependencies.Add(LayoutDependency.Height(group.EntityID));
                    }
                }
            }
        }

        public void UpdateLayoutOrder()
        {
            _resolvedDependencies.Clear();
            _pendingDependencies.Clear();

            var updatedOrder = GetLayoutOrder(_dependencyOrder).ToList();
            _dependencyOrder = updatedOrder;
        }

        public IEnumerable<LayoutDependency> GetLayoutOrder(IEnumerable<LayoutDependency> dependencies)
        {
            foreach (var dependency in dependencies)
            {
                if (!_resolvedDependencies.Contains(dependency))
                {
                    if (_pendingDependencies.Contains(dependency)) throw new InvalidOperationException("Encountered a dependency cycle");
                    _pendingDependencies.Add(dependency);

                    foreach (var layout in GetLayoutOrder(_nodeByDependency[dependency].Dependencies))
                    {
                        yield return layout;
                    }

                    if (_pendingDependencies.Contains(dependency))
                    {
                        _pendingDependencies.Remove(dependency);
                    }

                    _resolvedDependencies.Add(dependency);
                    yield return dependency;
                }
            }

            //_resolvedDependencies.Add(node.Key);
            //yield return node.Key;
        }

        public void Traverse(IElement root)
        {
            // We need to gather ALL dependencies for every element in the UI tree,
            // then we need to determine an appropriate layout order for each attribute type
            if (_needsLayout)
            {
                GatherDependencyNodes(root);
                UpdateLayoutOrder();

                foreach (var dependency in _dependencyOrder)
                {
                    var node = _nodeByDependency[dependency];
                    LayoutElement(node.Element, dependency.Step);
                }

                _needsLayout = false;
            }
        }

        private int GetRemainingWidth(Group group)
        {
            if (group.LayoutProgress.NeedsWidthMeasuring)
            {
                if (group.Parent is Group parentGroup)
                {
                    return GetRemainingWidth(parentGroup);
                }
                else
                {
                    return _resolution.Width;
                }
            }
            else
            {
                return group.LayoutProgress.RemainingWidth;
            }
        }

        private int GetRemainingHeight(Group group)
        {
            if (group.LayoutProgress.NeedsHeightMeasuring)
            {
                if (group.Parent is Group parentGroup)
                {
                    return GetRemainingHeight(parentGroup);
                }
                else
                {
                    return _resolution.Height;
                }
            }
            else
            {
                return group.LayoutProgress.RemainingHeight;
            }
        }

        private LayoutInfo GetLayoutInfo(IElement element, LayoutSteps attributeType)
        {
            var availableValue = 0;

            if (element.Parent is Group parentGroup)
            {
                switch (attributeType)
                {
                    case LayoutSteps.X:
                        availableValue = parentGroup.LayoutProgress.CurrentX;
                        break;
                    case LayoutSteps.Y:
                        availableValue = parentGroup.LayoutProgress.CurrentY;
                        break;
                    case LayoutSteps.Width:
                        availableValue = GetRemainingWidth(parentGroup);
                        break;
                    case LayoutSteps.Height:
                        availableValue = GetRemainingHeight(parentGroup);
                        break;
                }
            }
            else
            {
                switch (attributeType)
                {
                    case LayoutSteps.X:
                        availableValue = 0;
                        break;
                    case LayoutSteps.Y:
                        availableValue = 0;
                        break;
                    case LayoutSteps.Width:
                        availableValue = _resolution.Width;
                        break;
                    case LayoutSteps.Height:
                        availableValue = _resolution.Height;
                        break;
                }
            }

            var parentX = element.Parent != null ? element.Parent.Measurement.X : 0;
            var parentY = element.Parent != null ? element.Parent.Measurement.Y : 0;
            var parentWidth = element.Parent != null ? element.Parent.Measurement.Width : _resolution.Width;
            var parentHeight = element.Parent != null ? element.Parent.Measurement.Height : _resolution.Height;

            return new LayoutInfo(availableValue, parentX, parentY, parentWidth, parentHeight);
        }

        private void LayoutElement(IElement element, LayoutSteps attributeType)
        {
            var layoutInfo = GetLayoutInfo(element, attributeType);

            switch (attributeType)
            {
                case LayoutSteps.X:
                    element.MeasureX(layoutInfo);
                    break;
                case LayoutSteps.Y:
                    element.MeasureY(layoutInfo);
                    break;
                case LayoutSteps.Width:
                    element.MeasureWidth(layoutInfo);

                    if (element.Parent is RowGroup parentRowGroup)
                    {
                        parentRowGroup.LayoutProgress.UpdateWidth(element.Measurement.Width);
                    }
                    break;
                case LayoutSteps.Height:
                    element.MeasureHeight(layoutInfo);

                    if (element.Parent is ColumnGroup parentColumnGroup)
                    {
                        parentColumnGroup.LayoutProgress.UpdateHeight(element.Measurement.Height);
                    }
                    break;
            }
        }
    }
}
