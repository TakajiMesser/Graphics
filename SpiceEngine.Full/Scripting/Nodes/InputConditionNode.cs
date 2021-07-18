using TangyHIDCore;

namespace UmamiScriptingCore.Behaviors.Nodes.Decorators
{
    public class InputConditionNode : ConditionNode
    {
        public InputConditionNode(Node child, params InputCondition[] conditions) : base(child) => Conditions = conditions;

        public InputCondition[] Conditions { get; }

        protected override bool Condition(BehaviorContext context)
        {
            var inputProvider = context.SystemProvider.GetGameSystem<IInputProvider>();

            foreach (var condition in Conditions)
            {
                if (!condition.Condition(inputProvider))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
