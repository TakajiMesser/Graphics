using System.Collections.Generic;

namespace UmamiScriptingCore.Behaviors.Nodes.Composites
{
    public abstract class CompositeNode : Node
    {
        public List<Node> Children { get; private set; } = new List<Node>();

        public CompositeNode(params Node[] children) => AddChildren(children);
        public CompositeNode(IEnumerable<Node> children) => AddChildren(children);

        private void AddChildren(IEnumerable<Node> children)
        {
            foreach (var child in children)
            {
                if (child is ScriptNode scriptNode)
                {
                    // Insert the script-node as a placeholder, to be overwritten upon compilation
                    var childIndex = Children.Count;
                    Children.Add(child);
                    scriptNode.Compiled += (s, args) => Children[childIndex] = args.Node;
                }
                else
                {
                    Children.Add(child);
                }
            }
        }

        public override void Reset()
        {
            foreach (var child in Children)
            {
                child.Reset();
            }
        }
    }
}
