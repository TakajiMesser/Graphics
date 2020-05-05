using SpiceEngineCore.Utilities;

namespace StarchUICore.Attributes.Units
{
    public class PixelUnits : IUnits
    {
        internal PixelUnits(int value) => Value = value;

        public int Value { get; }

        public int ToOffsetPixels(int availableValue, int referenceValue = 0) => Value;
        public int ToDimensionPixels(int availableValue, int referenceValue = 0) => Value < availableValue ? Value : availableValue;

        public int ConstrainAsMinimum(int availableValue, int referenceValue = 0) => availableValue.ClampBottom(Value);
        public int ConstrainAsMaximum(int availableValue, int referenceValue = 0) => availableValue.ClampTop(Value);
    }
}
