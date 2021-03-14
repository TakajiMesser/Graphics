using SpiceEngine.GLFWBindings.Inputs;

namespace TangyHIDCore.Inputs
{
    public class GamePadState : InputState<GamePadButtons>
    {
        public GamePadState() : base(DeviceTypes.GamePad) { }

        public override bool IsDown(InputBinding inputBinding) => IsDown(inputBinding.GamePadButton);

        public override void UpdateFrom(IInputState state)
        {
            if (state is GamePadState gamePadState)
            {
                
            }
        }
    }
}
