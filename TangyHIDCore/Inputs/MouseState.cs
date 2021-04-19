using SpiceEngine.GLFWBindings.Inputs;

namespace TangyHIDCore.Inputs
{
    public class MouseState : InputState<MouseButtons>
    {
        public MouseState() : base(DeviceTypes.Mouse) { }

        public override bool IsDown(InputBinding inputBinding) => IsDown(inputBinding.MouseButton);
    }
}
