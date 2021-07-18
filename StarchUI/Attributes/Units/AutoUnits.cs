namespace StarchUICore.Attributes.Units
{
    public class AutoUnits : IUnits
    {
        internal AutoUnits() { }

        public int ToOffsetPixels(int availableValue, int referenceValue = 0) => availableValue;
        public int ToDimensionPixels(int availableValue, int referenceValue = 0) => referenceValue < availableValue ? referenceValue : availableValue;

        public int ConstrainAsMinimum(int availableValue, int referenceValue = 0) => availableValue;
        public int ConstrainAsMaximum(int availableValue, int referenceValue = 0) => availableValue;
    }
}
