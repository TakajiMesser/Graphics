using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Scripting.BehaviorTrees.Composites
{
    [DataContract]
    public class OrderedNode : CompositeNode
    {
        public OrderedNode(IEnumerable<INode> nodes) : base(nodes) { }

        public override void Tick(Dictionary<string, object> variablesByName)
        {
            if (!Status.IsComplete())
            {
                Status = BehaviorStatuses.Running;

                foreach (var node in Children)
                {
                    if (!node.Status.IsComplete())
                    {
                        node.Tick(variablesByName);
                        return;
                    }
                }

                Status = BehaviorStatuses.Success;
            }
        }
    }
}
