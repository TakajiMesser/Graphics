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
    public abstract class CompositeNode : INode
    {
        public BehaviorStatuses Status { get; protected set; }

        [DataMember]
        public List<INode> Children { get; protected set; } = new List<INode>();

        public CompositeNode(IEnumerable<INode> nodes)
        {
            Children.AddRange(nodes);
        }

        public abstract void Tick(Dictionary<string, object> variablesByName);

        public void Reset()
        {
            Status = BehaviorStatuses.Dormant;

            foreach (var node in Children)
            {
                node.Reset();
            }
        }
    }
}
