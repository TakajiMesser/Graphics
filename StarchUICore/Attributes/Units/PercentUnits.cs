using System;

namespace StarchUICore.Attributes.Units
{
    public class PercentUnits : IUnits
    {
        internal PercentUnits(float value) => Value = value;
        public float Value { get; }

        public int ToOffsetPixels(int nRelative = 0) => (int)Math.Ceiling(nRelative / 100 * Value);

        public int ToDimensionPixels(int nAvailable, int nRelative = 0)
        {
            var nPixels = ToOffsetPixels(nRelative);
            return nPixels < nAvailable ? nPixels : nAvailable;
        }
    }
}
