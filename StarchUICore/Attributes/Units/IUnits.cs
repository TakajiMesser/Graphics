namespace StarchUICore.Attributes.Units
{
    public interface IUnits
    {
        int ToOffsetPixels(int availableValue, int referenceValue = 0);
        int ToDimensionPixels(int availableValue, int referenceValue = 0);
        
        int ConstrainAsMinimum(int availableValue, int referenceValue = 0);
        int ConstrainAsMaximum(int availableValue, int referenceValue = 0);

        /* TODO - Determine all of the different ways that we use IUnits
        
        AUTO UNITS
        public int Constrain(int referenceValue, int desiredValue = 0) => desiredValue;

        PIXEL UNITS
        public int GetValue(int referenceValue) => Value;
        public int Constrain(int value, int referenceValue) => Value < value ? Value : value;

        PERCENT UNITS
        public int GetValue(int referenceValue) => (int)Math.Ceiling(referenceValue / 100 * Value);
        public int Constrain(int value, int referenceValue)
        {
            var percentValue = GetValue(referenceValue);
            return percentValue < value ? percentValue : value;
        }
        
        CALCULATING POSITION
        var constrainedX = X.GetValue(referenceWidth);
        if (!(MinimumX is AutoUnits))
        {
            constrainedX = constrainedX.ClampBottom(MinimumX.Constrain(relativeX, referenceWidth));
        }

        CALCULATING SIZE
        Width.Constrain(availableWidth, referenceWidth);
        if (!(MinimumWidth is AutoUnits))
        {
            constrainedWidth = constrainedWidth.ClampBottom(MinimumWidth.Constrain(availableWidth, referenceWidth));
        }

        POSITION
            -AutoUnits -> Center in reference element
            -PixelUnits -> Offset from anchor of reference element by this value
            -PercentUnits -> Offset from anchor of reference element by this % of reference size

        SIZE
            -AutoUnits -> Match size to contents
            -PixelUnits -> Set size to this value
            -PercentUnits -> Set size to this % of reference size
        */
    }
}
