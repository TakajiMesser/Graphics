using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;
using OpenTK;

namespace Graphics.Inputs
{
    public class InputState
    {
        private KeyboardState _keyState;
        private KeyboardState _previousKeyState;
        private MouseState _mouseState;
        private MouseState _previousMouseState;
        private MouseDevice _mouseDevice;
        private MouseDevice _previousMouseDevice;

        public Vector2 MouseCoordinates => new Vector2(_mouseDevice.X, _mouseDevice.Y);
        public int MouseWheelDelta
        {
            get
            {
                if (_mouseState == null)
                {
                    return 0;
                }
                else if (_previousMouseState == null)
                {
                    return _mouseState.Wheel;
                }
                else
                {
                    return _previousMouseState.Wheel - _mouseState.Wheel;
                }
            }
        }

        public void UpdateState(KeyboardState keyState, MouseState mouseState, MouseDevice mouseDevice)
        {
            _previousKeyState = _keyState;
            _keyState = keyState;

            _previousMouseState = _mouseState;
            _mouseState = mouseState;

            _previousMouseDevice = _mouseDevice;
            _mouseDevice = mouseDevice;
        }

        public bool IsPressed(Input input)
        {
            switch (input.Type)
            {
                case InputType.Key:
                    if (_keyState == null) return false;
                    return _keyState.IsKeyDown((Key)input.PrimaryInput) || _keyState.IsKeyDown((Key)input.SecondaryInput);
                    case InputType.Mouse:
                    if (_mouseState == null) return false;
                    return _mouseState.IsButtonDown((MouseButton)input.PrimaryInput) || _mouseState.IsButtonDown((MouseButton)input.SecondaryInput);
                default:
                    throw new NotImplementedException("Cannot handle InputType " + Enum.GetName(typeof(Type), input.Type));
            }
        }
    }
}
