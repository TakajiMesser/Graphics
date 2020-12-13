using OpenTK.Input;
using System;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace TangyHIDCore.Inputs
{
    public class InputState
    {
        private KeyboardState _keyboardState = Keyboard.GetState();
        private MouseState _mouseState = Mouse.GetState();
        private GamePadState _gamePadState;

        public InputState()
        {
            // How to choose index for GamePad?
        }

        public Vector2 MousePosition => new Vector2(_mouseState.X, _mouseState.Y);
        public int MouseWheel => _mouseState.Wheel;

        public bool IsDown(Input input)
        {
            switch (input.Type)
            {
                case InputTypes.Key:
                    return _keyboardState.IsKeyDown((Key)input.PrimaryInput) || _keyboardState.IsKeyDown((Key)input.SecondaryInput);
                case InputTypes.Mouse:
                    return input.HasPrimaryMouseInput && _mouseState.IsButtonDown((MouseButton)input.PrimaryInput)
                        || input.HasSecondaryMouseInput && _mouseState.IsButtonDown((MouseButton)input.SecondaryInput);
                case InputTypes.GamePad:
                default:
                    throw new NotImplementedException("");
            }
        }

        public bool IsUp(Input input)
        {
            switch (input.Type)
            {
                case InputTypes.Key:
                    return _keyboardState.IsKeyUp((Key)input.PrimaryInput) && _keyboardState.IsKeyUp((Key)input.SecondaryInput);
                case InputTypes.Mouse:
                    return (!input.HasPrimaryMouseInput || _mouseState.IsButtonUp((MouseButton)input.PrimaryInput))
                        && (!input.HasSecondaryMouseInput || _mouseState.IsButtonUp((MouseButton)input.SecondaryInput));
                case InputTypes.GamePad:
                default:
                    throw new NotImplementedException("");
            }
        }
    }
}
