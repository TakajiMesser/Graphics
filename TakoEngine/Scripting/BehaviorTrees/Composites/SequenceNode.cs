using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TakoEngine.Scripting.BehaviorTrees.Composites
{
    [DataContract]
    public class SequenceNode : CompositeNode
    {
        public SequenceNode(IEnumerable<Node> nodes) : base(nodes) { }
        public SequenceNode(params Node[] nodes) : base(nodes) { }

        public override void Tick(BehaviorContext context)
        {
            if (!Status.IsComplete())
            {
                Status = BehaviorStatuses.Running;

                foreach (var node in Children)
                {
                    node.Tick(context);

                    if (node.Status == BehaviorStatuses.Failure)
                    {
                        Status = BehaviorStatuses.Failure;
                        return;
                    }
                    else if (node.Status == BehaviorStatuses.Running)
                    {
                        return;
                    }
                }

                Status = BehaviorStatuses.Success;
            }
        }
    }
}
