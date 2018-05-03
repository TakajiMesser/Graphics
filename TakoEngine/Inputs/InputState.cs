using OpenTK;
using OpenTK.Input;
using System;
using System.Drawing;
using System.Linq;
using TakoEngine.Utilities;

namespace TakoEngine.Inputs
{
    public class InputState
    {
        private KeyboardState? _keyState;
        private KeyboardState? _previousKeyState;
        private MouseState? _mouseState;
        private MouseState? _previousMouseState;
        private MouseDevice _mouseDevice;
        private MouseDevice _previousMouseDevice;

        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }

        public Vector2? MouseCoordinates => _mouseDevice != null
            ? new Vector2(_mouseDevice.X, _mouseDevice.Y)
            : (Vector2?)null;

        public Vector2 MouseDelta => _mouseState == null || _previousMouseState == null
            ? Vector2.Zero
            : new Vector2(_mouseState.Value.X, _mouseState.Value.Y) - new Vector2(_previousMouseState.Value.X, _previousMouseState.Value.Y);

        public bool IsMouseInWindow => _mouseDevice != null
            ? (_mouseDevice.X.IsBetween(0, WindowWidth) && _mouseDevice.Y.IsBetween(0, WindowHeight))
            : false;

        public int MouseWheelDelta => _mouseState == null
            ? 0
            : _previousMouseState == null
                ? _mouseState.Value.Wheel
                : _previousMouseState.Value.Wheel - _mouseState.Value.Wheel;

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

        public void ClearState()
        {
            _previousKeyState = null;
            _keyState = null;
            _previousMouseState = null;
            _mouseState = null;
            _mouseDevice = null;
            _previousMouseDevice = null;
        }

        public bool IsPressed(params Input[] inputs) => inputs.All(i => IsPressed(i));

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
                            return _previousKeyState.Value.IsKeyUp((Key)input.PrimaryInput) && _previousKeyState.Value.IsKeyUp((Key)input.SecondaryInput)
                                && (_keyState.Value.IsKeyDown((Key)input.PrimaryInput) || _keyState.Value.IsKeyDown((Key)input.SecondaryInput));
                        }
                        else
                        {
                            return _keyState.Value.IsKeyDown((Key)input.PrimaryInput) || _keyState.Value.IsKeyDown((Key)input.SecondaryInput);
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
                            return (!input.HasPrimaryMouseInput || _previousMouseState.Value.IsButtonUp((MouseButton)input.PrimaryInput))
                                && (!input.HasSecondaryMouseInput || _previousMouseState.Value.IsButtonUp((MouseButton)input.SecondaryInput))
                                && (input.HasPrimaryMouseInput && (_mouseState.Value.IsButtonDown((MouseButton)input.PrimaryInput))
                                    || (input.HasSecondaryMouseInput && _mouseState.Value.IsButtonDown((MouseButton)input.SecondaryInput)));
                        }
                        else
                        {
                            return (input.HasPrimaryMouseInput && _mouseState.Value.IsButtonDown((MouseButton)input.PrimaryInput))
                                || (input.HasSecondaryMouseInput && _mouseState.Value.IsButtonDown((MouseButton)input.SecondaryInput));
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

        public bool IsHeld(params Input[] inputs) => inputs.All(i => IsHeld(i));

        /// <summary>
        /// Determines if this input was triggered this frame.
        /// </summary>
        public bool IsHeld(Input input)
        {
            switch (input.Type)
            {
                case InputType.Key:
                    return (_keyState != null)
                        ? _keyState.Value.IsKeyDown((Key)input.PrimaryInput) || _keyState.Value.IsKeyDown((Key)input.SecondaryInput)
                        : false;

                case InputType.Mouse:
                    return (_mouseState != null)
                        ? (input.HasPrimaryMouseInput && _mouseState.Value.IsButtonDown((MouseButton)input.PrimaryInput))
                            || (input.HasSecondaryMouseInput && _mouseState.Value.IsButtonDown((MouseButton)input.SecondaryInput))
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
                        return (_previousKeyState.Value.IsKeyDown((Key)input.PrimaryInput) || _previousKeyState.Value.IsKeyDown((Key)input.SecondaryInput))
                                && _keyState.Value.IsKeyUp((Key)input.PrimaryInput)
                                && _keyState.Value.IsKeyUp((Key)input.SecondaryInput);
                    }
                    else
                    {
                        return false;
                    }

                case InputType.Mouse:
                    if (_mouseState != null && _previousMouseState != null)
                    {
                        return ((input.HasPrimaryMouseInput && _previousMouseState.Value.IsButtonDown((MouseButton)input.PrimaryInput))
                            || (input.HasSecondaryMouseInput && _previousMouseState.Value.IsButtonDown((MouseButton)input.SecondaryInput)))
                                && (!input.HasPrimaryMouseInput || _mouseState.Value.IsButtonUp((MouseButton)input.PrimaryInput))
                                && (!input.HasSecondaryMouseInput || _mouseState.Value.IsButtonUp((MouseButton)input.SecondaryInput));
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
