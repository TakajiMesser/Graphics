using SpiceEngine.GLFWBindings.Inputs;
using System;

namespace TangyHIDCore.Inputs
{
    public interface IInputState
    {
        bool IsDown(InputBinding inputBinding);
        void UpdateFrom(IInputState state);
    }

    public interface IInputState<T> : IInputState where T : Enum
    {
        DeviceTypes DeviceType { get; }

        bool IsDown(T input);
        void Update(T input, InputStates state);
    }
}
