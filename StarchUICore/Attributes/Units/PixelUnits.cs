namespace StarchUICore.Attributes.Units
{
    public class PixelUnits : IUnits
    {
        internal PixelUnits(int value) => Value = value;
        public int Value { get; }

        public int Constrain(int value) => Value < value ? Value : value;
    }
}
