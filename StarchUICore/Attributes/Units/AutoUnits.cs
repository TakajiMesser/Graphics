namespace StarchUICore.Attributes.Units
{
    public class AutoUnits : IUnits
    {
        internal AutoUnits() { }

        public int GetValue(int containingValue) => 0;
        public int Constrain(int value, int containingValue) => value;
    }
}
