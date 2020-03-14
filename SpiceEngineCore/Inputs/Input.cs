using OpenTK.Input;
using System;

namespace SpiceEngineCore.Inputs
{
    public enum InputTypes
    {
        Key,
        Mouse,
        GamePad
    }

    public class Input
    {
        private Key _primaryKey = Key.Unknown;
        private Key _secondaryKey = Key.Unknown;
        private MouseButton? _primaryMouse = null;
        private MouseButton? _secondaryMouse = null;
        private GamePad _primaryGamePadButton;
        private GamePadButtons _secondaryGamePadButton;

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
                }

                throw new NotImplementedException("Cannot handle InputType " + Enum.GetName(typeof(Type), Type));
            }
        }

        public bool HasPrimaryMouseInput => _primaryMouse.HasValue;
        public bool HasSecondaryMouseInput => _secondaryMouse.HasValue;

        public Input(Key primaryKey)
        {
            Type = InputTypes.Key;
            _primaryKey = primaryKey;
        }

        public Input(MouseButton primaryMouseButton)
        {
            Type = InputTypes.Mouse;
            _primaryMouse = primaryMouseButton;
        }
    }
}
