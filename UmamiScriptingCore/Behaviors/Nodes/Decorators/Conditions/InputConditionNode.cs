using SpiceEngineCore.Inputs;
using System.Linq;

namespace UmamiScriptingCore.Behaviors.Nodes.Decorators
{
    public enum InputTypes
    {
        Down,
        Up,
        Pressed,
        Released,
        Held
    }

    public abstract class InputConditionNode : ConditionNode
    {
        public Input[] Inputs { get; }
        public InputTypes InputType { get; }
        public int FramesHeldCount { get; }

        public InputConditionNode(Node child, InputTypes inputType, params Input[] inputs) : base(child)
        {
            InputType = inputType;
            Inputs = inputs;
        }

        public InputConditionNode(Node child, InputTypes inputType, int nFramesHeld, params Input[] inputs) : base(child)
        {
            InputType = inputType;
            Inputs = inputs;
            FramesHeldCount = nFramesHeld;
        }

        protected override bool Condition(BehaviorContext context)
        {
            switch (InputType)
            {
                case InputTypes.Down:
                    return Inputs.All(i => context.InputProvider.IsDown(i));
                case InputTypes.Up:
                    return Inputs.All(i => context.InputProvider.IsUp(i));
                case InputTypes.Pressed:
                    return Inputs.All(i => context.InputProvider.IsPressed(i));
                case InputTypes.Released:
                    return Inputs.All(i => context.InputProvider.IsReleased(i));
                case InputTypes.Held:
                    return Inputs.All(i => context.InputProvider.IsHeld(i, FramesHeldCount));
            }
            
            return false;
        }
    }
}
