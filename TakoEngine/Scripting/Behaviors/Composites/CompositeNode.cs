using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakoEngine.Scripting.Behaviors.Composites
{
    public abstract class CompositeNode : Node
    {
        public List<Node> Children { get; private set; } = new List<Node>();

        public CompositeNode(params Node[] children) => Children.AddRange(children);
        public CompositeNode(IEnumerable<Node> children) => Children.AddRange(children);

        public override void Reset()
        {
            foreach (var child in Children)
            {
                child.Reset();
            }
        }
    }
}
