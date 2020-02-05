using System;

namespace StarchUICore.Attributes.Units
{
    public class PercentUnits : IUnits
    {
        internal PercentUnits(float value) => Value = value;
        public float Value { get; }

        public int GetValue(int containingValue) => (int)Math.Ceiling(containingValue / 100 * Value);
        public int Constrain(int value, int containingValue)
        {
            var percentValue = GetValue(containingValue);
            return percentValue < value ? percentValue : value;
        }
    }
}
