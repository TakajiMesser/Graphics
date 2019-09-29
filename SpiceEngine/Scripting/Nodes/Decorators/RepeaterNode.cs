using SpiceEngineCore.Utilities;

namespace SpiceEngine.Scripting.Nodes.Decorators
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
