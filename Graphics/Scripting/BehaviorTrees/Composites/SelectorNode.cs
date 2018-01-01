using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Scripting.BehaviorTrees.Composites
{
    [DataContract]
    public class SelectorNode : CompositeNode
    {
        public SelectorNode(IEnumerable<Node> nodes) : base(nodes) { }
        public SelectorNode(params Node[] nodes) : base(nodes) { }

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
                    }

                    if (node.Status == BehaviorStatuses.Success)
                    {
                        Status = BehaviorStatuses.Success;
                        return;
                    }
                    else if (node.Status == BehaviorStatuses.Running)
                    {
                        return;
                    }
                }

                Status = BehaviorStatuses.Failure;
            }
        }
    }
}
