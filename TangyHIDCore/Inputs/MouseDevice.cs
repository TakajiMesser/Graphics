using SpiceEngineCore.Geometry;
using System;

namespace TangyHIDCore.Inputs
{
    public class MouseDevice : InputDevice<MouseState>
    {
        public MouseDevice(IInputStateProvider stateProvider, IInputTracker inputTracker) : base(stateProvider, DeviceTypes.Mouse)
        {
            inputTracker.MouseButton += InputTracker_MouseButton;
            inputTracker.CursorPositionChanged += InputTracker_CursorPositionChanged;
            inputTracker.Scrolled += InputTracker_Scrolled;
            inputTracker.CursorEntered += InputTracker_CursorEntered;
            inputTracker.CursorExited += InputTracker_CursorExited;
        }

        private void InputTracker_MouseButton(object sender, MouseButtonEventArgs e) => _state.Update(e.Button, e.State);

        private void InputTracker_CursorPositionChanged(object sender, CursorEventArgs e) => _state.Position = new Vector2((float)e.X, (float)e.Y);

        private void InputTracker_Scrolled(object sender, CursorEventArgs e) => _state.Wheel = (int)e.Y;

        private void InputTracker_CursorEntered(object sender, EventArgs e) => _state.IsInWindow = true;

        private void InputTracker_CursorExited(object sender, EventArgs e) => _state.IsInWindow = false;
    }
}
