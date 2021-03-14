using SpiceEngine.GLFWBindings.Inputs;
using SpiceEngineCore.Geometry;

namespace TangyHIDCore.Inputs
{
    public class MouseState : InputState<MouseButtons>
    {
        public MouseState() : base(DeviceTypes.Mouse) { }

        public Vector2 Position { get; set; }
        public int Wheel { get; set; }
        public bool IsInWindow { get; set; }

        public override bool IsDown(InputBinding inputBinding) => IsDown(inputBinding.MouseButton);

        public override void UpdateFrom(IInputState state)
        {
            if (state is MouseState mouseState)
            {
                Position = mouseState.Position;
                Wheel = mouseState.Wheel;
                IsInWindow = mouseState.IsInWindow;
            }
        }
    }
}
