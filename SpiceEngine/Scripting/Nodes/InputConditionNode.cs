using System.Linq;
using TangyHIDCore;
using TangyHIDCore.Inputs;

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

    public class InputConditionNode : ConditionNode
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
            var inputProvider = context.SystemProvider.GetGameSystem<IInputProvider>();

            switch (InputType)
            {
                case InputTypes.Down:
                    return Inputs.All(i => inputProvider.IsDown(i));
                case InputTypes.Up:
                    return Inputs.All(i => inputProvider.IsUp(i));
                case InputTypes.Pressed:
                    return Inputs.All(i => inputProvider.IsPressed(i));
                case InputTypes.Released:
                    return Inputs.All(i => inputProvider.IsReleased(i));
                case InputTypes.Held:
                    return Inputs.All(i => inputProvider.IsHeld(i, FramesHeldCount));
            }

            return false;
        }
    }
}
