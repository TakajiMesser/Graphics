using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Scripting.BehaviorTrees.Composites
{
    /// <summary>
    /// A composite node is a node that can have one or more children.
    /// They will process one or more of these children in either a first to last sequence
    /// or random order depending on the particular composite node in question.
    /// </summary>
    [DataContract]
    public abstract class CompositeNode : Node
    {
        [DataMember]
        public List<Node> Children { get; protected set; } = new List<Node>();

        public CompositeNode(IEnumerable<Node> nodes) => Children.AddRange(nodes);
        public CompositeNode(params Node[] nodes) => Children.AddRange(nodes);

        public override void Reset()
        {
            base.Reset();

            foreach (var node in Children)
            {
                node.Reset();
            }
        }
    }
}
