using SpiceEngine.GLFW.Inputs;
using System;

namespace TangyHIDCore.Inputs
{
    public enum InputTypes
    {
        Key,
        Mouse,
        GamePad
    }

    public class Input
    {
        private Keys _primaryKey = Keys.Unknown;
        private Keys _secondaryKey = Keys.Unknown;
        private MouseButtons _primaryMouse = MouseButtons.Unknown;
        private MouseButtons _secondaryMouse = MouseButtons.Unknown;
        private GamePadButtons _primaryGamePadButton = GamePadButtons.Unknown;
        private GamePadButtons _secondaryGamePadButton = GamePadButtons.Unknown;

        public Input(Keys primaryKey)
        {
            Type = InputTypes.Key;
            _primaryKey = primaryKey;
        }

        public Input(MouseButtons primaryMouseButton)
        {
            Type = InputTypes.Mouse;
            _primaryMouse = primaryMouseButton;
        }

        public InputTypes Type { get; set; }

        public int PrimaryInput
        {
            get
            {
                switch (Type)
                {
                    case InputTypes.Key:
                        return (int)_primaryKey;
                    case InputTypes.Mouse:
                        return (int)_primaryMouse;
                    case InputTypes.GamePad:
                        return (int)_primaryGamePadButton;
                }

                throw new NotImplementedException("Cannot handle InputType " + Enum.GetName(typeof(Type), Type));
            }
        }

        public int SecondaryInput
        {
            get
            {
                switch (Type)
                {
                    case InputTypes.Key:
                        return (int)_secondaryKey;
                    case InputTypes.Mouse:
                        return (int)_secondaryMouse;
                    case InputTypes.GamePad:
                        return (int)_secondaryGamePadButton;
                }

                throw new NotImplementedException("Cannot handle InputType " + Enum.GetName(typeof(Type), Type));
            }
        }
    }
}
