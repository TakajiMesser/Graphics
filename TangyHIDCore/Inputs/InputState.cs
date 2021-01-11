using OpenTK.Input;
using System;
using TangyHIDCore.Utilities;
using Vector2 = SpiceEngineCore.Geometry.Vector2;

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
                    return _keyboardState.IsKeyDown(input.PrimaryInput.ConvertToOpenTKKey()) || _keyboardState.IsKeyDown(input.SecondaryInput.ConvertToOpenTKKey());
                case InputTypes.Mouse:
                    var primaryMouseButton = input.PrimaryInput.ConvertToOpenTKMouseButton();
                    var secondaryMouseButton = input.SecondaryInput.ConvertToOpenTKMouseButton();

                    return primaryMouseButton.HasValue && _mouseState.IsButtonDown(primaryMouseButton.Value)
                        || secondaryMouseButton.HasValue && _mouseState.IsButtonDown(secondaryMouseButton.Value);
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
                    var primaryMouseButton = input.PrimaryInput.ConvertToOpenTKMouseButton();
                    var secondaryMouseButton = input.SecondaryInput.ConvertToOpenTKMouseButton();

                    return primaryMouseButton.HasValue && _mouseState.IsButtonUp(primaryMouseButton.Value)
                        || secondaryMouseButton.HasValue && _mouseState.IsButtonUp(secondaryMouseButton.Value);
                default:
                    throw new NotImplementedException("");
            }
        }
    }
}
