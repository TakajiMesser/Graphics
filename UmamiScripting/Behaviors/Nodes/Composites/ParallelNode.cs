using System.Collections.Generic;

namespace UmamiScriptingCore.Behaviors.Nodes.Composites
{
    /// <summary>
    /// Ticks all children forward simultaneously. Returns failure as soon as one child fails. Returns success once all children have succeeded.
    /// </summary>
    public class ParallelNode : CompositeNode
    {
        public ParallelNode(IEnumerable<Node> nodes) : base(nodes) { }
        public ParallelNode(params Node[] nodes) : base(nodes) { }

        public override BehaviorStatus Tick(BehaviorContext context)
        {
            var status = BehaviorStatus.Success;

            foreach (var child in Children)
            {
                switch (child.Tick(context))
                {
                    case BehaviorStatus.Failure:
                        return BehaviorStatus.Failure;
                    case BehaviorStatus.Running:
                        status = BehaviorStatus.Running;
                        break;
                }
            }

            return status;
        }
    }
}
