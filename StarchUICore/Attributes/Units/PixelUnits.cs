using SpiceEngineCore.Utilities;

namespace StarchUICore.Attributes.Units
{
    public class PixelUnits : IUnits
    {
        internal PixelUnits(int value) => Value = value;

        public int Value { get; }

        public int ToOffsetPixels(int nRelative = 0) => Value;

        public int? ToOffsetPixels(int value, int? referenceValue) => Value;
        public int? ConstrainAsMinimum(int value, int? referenceValue) => value.ClampBottom(Value);
        public int? ConstrainAsMaximum(int value, int? referenceValue) => value.ClampTop(Value);

        public int ToDimensionPixels(int nAvailable, int nRelative = 0)
        {
            var nPixels = ToOffsetPixels(nRelative);
            return nPixels < nAvailable ? nPixels : nAvailable;
        }
    }
}
