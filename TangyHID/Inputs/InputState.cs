using SpiceEngine.GLFWBindings.Inputs;
using SpiceEngineCore.Utilities;
using System;
using System.Collections.Generic;

namespace TangyHIDCore.Inputs
{
    public abstract class InputState<T> : IInputState<T> where T : Enum
    {
        private Dictionary<T, int> _indexByInput = new Dictionary<T, int>();
        private bool[] _inputPresses;

        public InputState(DeviceTypes deviceType)
        {
            DeviceType = deviceType;

            var index = 0;

            foreach (var value in Enum.GetValues(typeof(T)))
            {
                var input = (T)value;

                if (!_indexByInput.ContainsKey(input))
                {
                    _indexByInput.Add(input, index);
                    index++;
                }
            }

            _inputPresses = ArrayExtensions.Initialize(index, false);
        }

        public DeviceTypes DeviceType { get; }

        public bool IsDown(T input)
        {
            var index = _indexByInput[input];
            return _inputPresses[index];
        }

        public abstract bool IsDown(InputBinding inputBinding);

        public void Update(T input, InputStates state)
        {
            var index = _indexByInput[input];

            if (state == InputStates.Press)
            {
                _inputPresses[index] = true;
            }
            else if (state == InputStates.Release)
            {
                _inputPresses[index] = false;
            }
        }

        public virtual void UpdateFrom(IInputState state)
        {
            if (state is InputState<T> inputState)
            {
                for (var i = 0; i < _inputPresses.Length; i++)
                {
                    _inputPresses[i] = inputState._inputPresses[i];
                }
            }
        }

        public void Clear()
        {
            for (var i = 0; i < _inputPresses.Length; i++)
            {
                _inputPresses[i] = false;
            }
        }
    }
}
