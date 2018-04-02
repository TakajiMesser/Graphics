using OpenTK;
using OpenTK.Input;
using System;
using System.Drawing;
using TakoEngine.Utilities;

namespace TakoEngine.Inputs
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

        public Vector2 MouseDelta => _mouseState == null || _previousMouseState == null
            ? Vector2.Zero
            : new Vector2(_mouseState.X, _mouseState.Y) - new Vector2(_previousMouseState.X, _previousMouseState.Y);

        public bool IsMouseInWindow => _mouseDevice != null
            ? (_mouseDevice.X.IsBetween(0, WindowWidth) && _mouseDevice.Y.IsBetween(0, WindowHeight))
            : false;

        public int MouseWheelDelta => _mouseState == null
            ? 0
            : _previousMouseState == null
                ? _mouseState.Wheel
                : _previousMouseState.Wheel - _mouseState.Wheel;

        public void UpdateState(Point mouseLocation)
        {
            _previousMouseDevice = _mouseDevice;
            /*_mouseDevice = new MouseDevice()
            {
                a
            };*/
        }

        public void UpdateState(GameWindow window)
        {
            _previousMouseDevice = _mouseDevice;
            _mouseDevice = window.Mouse;
            WindowWidth = window.Width;
            WindowHeight = window.Height;
        }

        public void UpdateState(KeyboardState keyState, MouseState mouseState)
        {
            _previousKeyState = _keyState;
            _keyState = keyState;

            _previousMouseState = _mouseState;
            _mouseState = mouseState;
        }

        /// <summary>
        /// Determines if this input was triggered this frame but was NOT triggered last frame.
        /// </summary>
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
                    if (_mouseState != null)
                    {
                        if (_previousMouseState != null)
                        {
                            return (!input.HasPrimaryMouseInput || _previousMouseState.IsButtonUp((MouseButton)input.PrimaryInput))
                                && (!input.HasSecondaryMouseInput || _previousMouseState.IsButtonUp((MouseButton)input.SecondaryInput))
                                && (input.HasPrimaryMouseInput && (_mouseState.IsButtonDown((MouseButton)input.PrimaryInput))
                                    || (input.HasSecondaryMouseInput && _mouseState.IsButtonDown((MouseButton)input.SecondaryInput)));
                        }
                        else
                        {
                            return (input.HasPrimaryMouseInput && _mouseState.IsButtonDown((MouseButton)input.PrimaryInput))
                                || (input.HasSecondaryMouseInput && _mouseState.IsButtonDown((MouseButton)input.SecondaryInput));
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

        /// <summary>
        /// Determines if this input was triggered this frame.
        /// </summary>
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
                        ? (input.HasPrimaryMouseInput && _mouseState.IsButtonDown((MouseButton)input.PrimaryInput))
                            || (input.HasSecondaryMouseInput && _mouseState.IsButtonDown((MouseButton)input.SecondaryInput))
                        : false;

                default:
                    throw new NotImplementedException("Cannot handle InputType " + Enum.GetName(typeof(Type), input.Type));
            }
        }

        public bool IsReleased(Input input)
        {
            switch (input.Type)
            {
                case InputType.Key:
                    if (_keyState != null && _previousKeyState != null)
                    {
                        return (_previousKeyState.IsKeyDown((Key)input.PrimaryInput) || _previousKeyState.IsKeyDown((Key)input.SecondaryInput))
                                && _keyState.IsKeyUp((Key)input.PrimaryInput)
                                && _keyState.IsKeyUp((Key)input.SecondaryInput);
                    }
                    else
                    {
                        return false;
                    }

                case InputType.Mouse:
                    if (_mouseState != null && _previousMouseState != null)
                    {
                        return ((input.HasPrimaryMouseInput && _previousMouseState.IsButtonDown((MouseButton)input.PrimaryInput))
                            || (input.HasSecondaryMouseInput && _previousMouseState.IsButtonDown((MouseButton)input.SecondaryInput)))
                                && (!input.HasPrimaryMouseInput || _mouseState.IsButtonUp((MouseButton)input.PrimaryInput))
                                && (!input.HasSecondaryMouseInput || _mouseState.IsButtonUp((MouseButton)input.SecondaryInput));
                    }
                    else
                    {
                        return false;
                    }

                default:
                    throw new NotImplementedException("Cannot handle InputType " + Enum.GetName(typeof(Type), input.Type));
            }
        }
    }
}
