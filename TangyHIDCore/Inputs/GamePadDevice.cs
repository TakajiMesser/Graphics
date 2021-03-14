namespace TangyHIDCore.Inputs
{
    public class GamePadDevice : InputDevice<GamePadState>
    {
        public GamePadDevice(IInputStateProvider stateProvider, IInputTracker inputTracker) : base(stateProvider, DeviceTypes.GamePad)
        {
            
        }
    }
}
