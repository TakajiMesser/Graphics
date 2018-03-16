using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TakoEngine.Scripting.BehaviorTrees.Composites
{
    [DataContract]
    public class ParallelNode : CompositeNode
    {
        public ParallelNode(IEnumerable<Node> nodes) : base(nodes) { }
        public ParallelNode(params Node[] nodes) : base(nodes) { }

        public override void Tick(BehaviorContext context)
        {
            if (!Status.IsComplete())
            {
                Status = BehaviorStatuses.Success;

                // Attempt to tick every child, and consider this node running until ALL children are complete
                foreach (var node in Children)
                {
                    node.Tick(context);

                    if (node.Status == BehaviorStatuses.Running)
                    {
                        Status = BehaviorStatuses.Running;
                    }
                }
            }
        }
    }
}
