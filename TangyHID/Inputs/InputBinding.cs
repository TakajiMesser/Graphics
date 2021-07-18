using SpiceEngine.GLFWBindings.Inputs;

namespace TangyHIDCore.Inputs
{
    public class InputBinding
    {
        private Keys _key = Keys.Unknown;
        private MouseButtons _mouseButton = MouseButtons.Unknown;
        private GamePadButtons _gamePadButton = GamePadButtons.Unknown;

        public InputBinding(string name) => Name = name;

        public string Name { get; }
        public DeviceTypes DeviceType { get; private set; } = DeviceTypes.None;

        public Keys Key
        {
            get => _key;
            set
            {
                DeviceType = DeviceTypes.Key;

                _key = value;
                _mouseButton = MouseButtons.Unknown;
                _gamePadButton = GamePadButtons.Unknown;
                
            }
        }

        public MouseButtons MouseButton
        {
            get => _mouseButton;
            set
            {
                DeviceType = DeviceTypes.Mouse;

                _key = Keys.Unknown;
                _mouseButton = value;
                _gamePadButton = GamePadButtons.Unknown;
            }
        }

        public GamePadButtons GamePadButton
        {
            get => _gamePadButton;
            set
            {
                DeviceType = DeviceTypes.GamePad;

                _key = Keys.Unknown;
                _mouseButton = MouseButtons.Unknown;
                _gamePadButton = value;
            }
        }

        public void Clear()
        {
            DeviceType = DeviceTypes.None;

            _key = Keys.Unknown;
            _mouseButton = MouseButtons.Unknown;
            _gamePadButton = GamePadButtons.Unknown;
        }
    }
}
