namespace TangyHIDCore.Inputs
{
    public interface IInputStateProvider
    {
        IInputState GetNextAvailableState(DeviceTypes deviceType);
    }
}
