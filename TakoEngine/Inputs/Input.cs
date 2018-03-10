using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace TakoEngine.Inputs
{
    public enum InputType
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
        //private GamePad _primaryGamePadButton;
        //private GamePadButtons _secondaryGamePadButton;

        public InputType Type { get; set; }

        public int PrimaryInput
        {
            get
            {
                switch (Type)
                {
                    case InputType.Key:
                        return (int)_primaryKey;
                    case InputType.Mouse:
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
                    case InputType.Key:
                        return (int)_secondaryKey;
                    case InputType.Mouse:
                        return (int)_secondaryMouse;
                }

                throw new NotImplementedException("Cannot handle InputType " + Enum.GetName(typeof(Type), Type));
            }
        }

        public bool HasPrimaryMouseInput => _primaryMouse.HasValue;
        public bool HasSecondaryMouseInput => _secondaryMouse.HasValue;

        public Input(Key primaryKey)
        {
            Type = InputType.Key;
            _primaryKey = primaryKey;
        }

        public Input(MouseButton primaryMouseButton)
        {
            Type = InputType.Mouse;
            _primaryMouse = primaryMouseButton;
        }
    }
}
