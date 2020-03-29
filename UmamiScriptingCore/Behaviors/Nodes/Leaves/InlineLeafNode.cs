using System;

namespace UmamiScriptingCore.Behaviors.Nodes.Leaves
{
    public class InlineLeafNode : LeafNode
    {
        public Action<BehaviorContext> Action { get; set; }

        public InlineLeafNode(Action<BehaviorContext> action) => Action = action;

        protected override void Execute(BehaviorContext context) => Action(context);
    }
}
