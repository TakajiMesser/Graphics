using SpiceEngineCore.Utilities;

namespace UmamiScriptingCore.Behaviors.Nodes.Decorators
{
    public class RepeaterNode : DecoratorNode
    {
        public RepeaterNode(Node node) : base(node) { }

        public override BehaviorStatus Tick(BehaviorContext context)
        {
            var childStatus = Child.Tick(context);
            if (childStatus == BehaviorStatus.Success || childStatus == BehaviorStatus.Failure)
            {
                Reset();
            }

            return BehaviorStatus.Running;
        }
    }
}
