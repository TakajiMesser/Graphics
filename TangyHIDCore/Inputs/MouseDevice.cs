using SpiceEngineCore.Geometry;
using System;

namespace TangyHIDCore.Inputs
{
    public class MouseDevice : InputDevice<MouseState>
    {
        private IInputTracker _inputTracker;
        private Vector2 _previousPosition;
        private Vector2 _currentPosition;

        public MouseDevice(IInputStateProvider stateProvider, IInputTracker inputTracker) : base(stateProvider, DeviceTypes.Mouse)
        {
            _inputTracker = inputTracker;

            inputTracker.MouseButton += InputTracker_MouseButton;
            inputTracker.CursorPositionChanged += InputTracker_CursorPositionChanged;
            inputTracker.Scrolled += InputTracker_Scrolled;
            inputTracker.CursorEntered += InputTracker_CursorEntered;
            inputTracker.CursorExited += InputTracker_CursorExited;
        }

        public Vector2 Position { get; private set; }
        public Vector2 PositionDelta => _currentPosition - _previousPosition;
        public int Wheel { get; private set; }
        public bool IsInWindow { get; private set; }

        public override MouseState CaptureState()
        {
            _previousPosition = _currentPosition;
            _currentPosition = new Vector2((float)_inputTracker.CursorPosition.X, (float)_inputTracker.CursorPosition.Y);
            Wheel = 0;

            return base.CaptureState();
        }

        private void InputTracker_MouseButton(object sender, MouseButtonEventArgs e) => _state.Update(e.Button, e.State);

        private void InputTracker_CursorPositionChanged(object sender, CursorEventArgs e) => Position = new Vector2((float)e.X, (float)e.Y);

        private void InputTracker_Scrolled(object sender, CursorEventArgs e) => Wheel = (int)e.Y;

        private void InputTracker_CursorEntered(object sender, EventArgs e) => IsInWindow = true;

        private void InputTracker_CursorExited(object sender, EventArgs e) => IsInWindow = false;
    }
}
