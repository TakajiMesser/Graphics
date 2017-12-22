using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;
using OpenTK;
using Graphics.Utilities;

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

        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }

        public Vector2 MouseCoordinates => new Vector2(_mouseDevice.X, _mouseDevice.Y);
        public bool IsMouseInWindow => _mouseDevice != null
            ? (_mouseDevice.X.IsBetween(0, WindowWidth) && _mouseDevice.Y.IsBetween(0, WindowHeight))
            : false;
        public int MouseWheelDelta => _mouseState == null
            ? 0
            : _previousMouseState == null
                ? _mouseState.Wheel
                : _previousMouseState.Wheel - _mouseState.Wheel;

        public void UpdateState(KeyboardState keyState, MouseState mouseState, GameWindow window)
        {
            _previousKeyState = _keyState;
            _keyState = keyState;

            _previousMouseState = _mouseState;
            _mouseState = mouseState;

            _previousMouseDevice = _mouseDevice;
            _mouseDevice = window.Mouse;

            WindowWidth = window.Width;
            WindowHeight = window.Height;
        }

        public bool IsPressed(Input input)
        {
            switch (input.Type)
            {
                case InputType.Key:
                    if (_keyState != null)
                    {
                        if (_previousKeyState != null)
                        {
                            return _previousKeyState.IsKeyUp((Key)input.PrimaryInput) && _previousKeyState.IsKeyUp((Key)input.SecondaryInput)
                                && (_keyState.IsKeyDown((Key)input.PrimaryInput) || _keyState.IsKeyDown((Key)input.SecondaryInput));
                        }
                        else
                        {
                            return _keyState.IsKeyDown((Key)input.PrimaryInput) || _keyState.IsKeyDown((Key)input.SecondaryInput);
                        }
                    }
                    else
                    {
                        return false;
                    }

                case InputType.Mouse:
                    if (_keyState != null)
                    {
                        if (_previousKeyState != null)
                        {
                            return _previousMouseState.IsButtonUp((MouseButton)input.PrimaryInput) && _previousMouseState.IsButtonUp((MouseButton)input.SecondaryInput)
                                && (_mouseState.IsButtonDown((MouseButton)input.PrimaryInput) || _mouseState.IsButtonDown((MouseButton)input.SecondaryInput));
                        }
                        else
                        {
                            return _mouseState.IsButtonDown((MouseButton)input.PrimaryInput) || _mouseState.IsButtonDown((MouseButton)input.SecondaryInput);
                        }
                    }
                    else
                    {
                        return false;
                    }

                default:
                    throw new NotImplementedException("Cannot handle InputType " + Enum.GetName(typeof(Type), input.Type));
            }
        }

        public bool IsHeld(Input input)
        {
            switch (input.Type)
            {
                case InputType.Key:
                    return (_keyState != null)
                        ? _keyState.IsKeyDown((Key)input.PrimaryInput) || _keyState.IsKeyDown((Key)input.SecondaryInput)
                        : false;

                case InputType.Mouse:
                    return (_mouseState != null)
                        ? _mouseState.IsButtonDown((MouseButton)input.PrimaryInput) || _mouseState.IsButtonDown((MouseButton)input.SecondaryInput)
                        : false;

                default:
                    throw new NotImplementedException("Cannot handle InputType " + Enum.GetName(typeof(Type), input.Type));
            }
        }
    }
}
