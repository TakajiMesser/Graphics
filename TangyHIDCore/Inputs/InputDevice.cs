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

        public virtual T CaptureState()
        {
            var currentState = _state;
            var nextState = (T)_stateProvider.GetNextAvailableState(DeviceType);

            nextState.UpdateFrom(currentState);
            _state = nextState;

            return currentState;
        }
    }
}
