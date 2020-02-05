namespace StarchUICore.Attributes.Units
{
    public class PixelUnits : IUnits
    {
        internal PixelUnits(int value) => Value = value;
        public int Value { get; }

        public int GetValue(int containingValue) => Value;
        public int Constrain(int value, int containingValue) => Value < value ? Value : value;
    }
}
