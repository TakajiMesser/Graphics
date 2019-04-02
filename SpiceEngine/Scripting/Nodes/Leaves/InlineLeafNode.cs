using System;

namespace SpiceEngine.Scripting.Nodes.Leaves
{
    public class InlineLeafNode : Node
    {
        public Func<BehaviorContext, BehaviorStatus> Action { get; set; }

        public InlineLeafNode(Func<BehaviorContext, BehaviorStatus> action)
        {
            Action = action;
        }

        public override BehaviorStatus Tick(BehaviorContext context) => Action(context);

        public override void Reset() { }
    }
}
