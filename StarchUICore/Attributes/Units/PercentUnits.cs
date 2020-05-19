using SpiceEngineCore.Utilities;
using System;

namespace StarchUICore.Attributes.Units
{
    public class PercentUnits : IUnits
    {
        internal PercentUnits(float value) => Value = value;

        public float Value { get; }

        public int ToOffsetPixels(int availableValue, int referenceValue = 0) =>
            (int)Math.Ceiling(referenceValue / 100 * Value);

        public int ToDimensionPixels(int availableValue, int referenceValue = 0)
        {
            var nPixels = (int)Math.Ceiling(referenceValue / 100 * Value);
            return nPixels < availableValue ? nPixels : availableValue;
        }

        public int ConstrainAsMinimum(int availableValue, int referenceValue = 0) => availableValue.ClampBottom((int)Math.Round(referenceValue * Value));
        public int ConstrainAsMaximum(int availableValue, int referenceValue = 0) => availableValue.ClampTop((int)Math.Round(referenceValue * Value));
    }
}
