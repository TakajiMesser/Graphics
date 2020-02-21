using System;

namespace UmamiScriptingCore.Behaviors.Nodes.Decorators
{
    public class InlineConditionNode : ConditionNode
    {
        public Predicate<BehaviorContext> Predicate { get; set; }

        public InlineConditionNode(Predicate<BehaviorContext> predicate, Node node) : base(node) => Predicate = predicate;

        public override bool Condition(BehaviorContext context) => Predicate(context);
    }
}
