namespace TangyHIDCore.Inputs
{
    public class KeyDevice : InputDevice<KeyState>
    {
        public KeyDevice(IInputStateProvider stateProvider, IInputTracker inputTracker) : base(stateProvider, DeviceTypes.Key)
        {
            inputTracker.KeyPress += InputTracker_KeyPress;
            inputTracker.KeyRelease += InputTracker_KeyRelease;
            //inputTracker.KeyRepeat += InputTracker_KeyRepeat;
            //inputTracker.KeyAction += InputTracker_KeyAction;
            //inputTracker.CharacterInput += InputTracker_CharacterInput;
        }

        private void InputTracker_KeyPress(object sender, KeyEventArgs e) => _state.Update(e.Key, e.State);

        private void InputTracker_KeyRelease(object sender, KeyEventArgs e) => _state.Update(e.Key, e.State);

        //private void InputTracker_KeyRepeat(object sender, KeyEventArgs e) => _state.Update(e.Key, e.State);

        //private void InputTracker_KeyAction(object sender, KeyEventArgs e) => _state.Update(e.Key, e.State);

        //private void InputTracker_CharacterInput(object sender, CharacterEventArgs e)
    }
}
