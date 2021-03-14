using SpiceEngine.GLFWBindings.Inputs;

namespace TangyHIDCore.Inputs
{
    public class KeyState : InputState<Keys>
    {
        public KeyState() : base(DeviceTypes.Key) { }

        public override bool IsDown(InputBinding inputBinding) => IsDown(inputBinding.Key);
    }
}
