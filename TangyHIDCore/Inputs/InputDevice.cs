namespace TangyHIDCore.Inputs
{
    public enum DeviceTypes
    {
        None,
        Key,
        Mouse,
        GamePad
    }

    public abstract class InputDevice<T> where T : IInputState
    {
        private IInputStateProvider _stateProvider;
        protected T _state;

        public InputDevice(IInputStateProvider stateProvider, DeviceTypes deviceType)
        {
            _stateProvider = stateProvider;
            _state = (T)_stateProvider.GetNextAvailableState(deviceType);
            DeviceType = deviceType;
        }

        public DeviceTypes DeviceType { get; }

        public T GetState()
        {
            var a = 3;
            if (this is KeyDevice)
            {
                a = 4;
            }

            var state = _state;
            _state = (T)_stateProvider.GetNextAvailableState(DeviceType);
            _state.UpdateFrom(state);
            return state;
        }
    }
}
