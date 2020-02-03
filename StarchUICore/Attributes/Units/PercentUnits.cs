using System;

namespace StarchUICore.Attributes.Units
{
    public class PercentUnits : IUnits
    {
        internal PercentUnits(float value) => Value = value;
        public float Value { get; }

        public int Constrain(int value) => (int)Math.Ceiling(value / 100 * Value);
    }
}
