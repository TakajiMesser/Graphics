namespace StarchUICore.Attributes.Units
{
    public class AutoUnits : IUnits
    {
        internal AutoUnits() { }

        public int ToOffsetPixels(int nRelative = 0) => nRelative;

        public int? ToOffsetPixels(int value, int? referenceValue) => value;
        public int? ConstrainAsMinimum(int value, int? referenceValue) => value;
        public int? ConstrainAsMaximum(int value, int? referenceValue) => value;

        public int ToDimensionPixels(int nAvailable, int nRelative = 0)
        {
            var nPixels = ToOffsetPixels(nRelative);
            return nPixels < nAvailable ? nPixels : nAvailable;
        }
    }
}
