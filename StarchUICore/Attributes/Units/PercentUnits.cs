using System;

namespace StarchUICore.Attributes.Units
{
    public class PercentUnits : IUnits
    {
        internal PercentUnits(float value) => Value = value;
        public float Value { get; }

        //public int Constrain(int value) => (int)Math.Ceiling(value / 100 * Value);
        public int Constrain(int value, int containingValue)
        {
            var percentValue = (int)Math.Ceiling(containingValue / 100 * Value);
            return percentValue < value ? percentValue : value;
        }
    }
}
