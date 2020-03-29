using SpiceEngineCore.Utilities;

namespace UmamiScriptingCore.Behaviors.Nodes.Decorators
{
    public abstract class ConditionNode : DecoratorNode
    {
        public ConditionNode(Node child) : base(child) { }

        public override BehaviorStatus Tick(BehaviorContext context) => Condition(context)
            ? Child.Tick(context)
            : BehaviorStatus.Failure;

        protected abstract bool Condition(BehaviorContext context);
    }
}
