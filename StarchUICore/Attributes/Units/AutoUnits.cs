namespace StarchUICore.Attributes.Units
{
    public class AutoUnits : IUnits
    {
        internal AutoUnits() { }

        //public int Constrain(int value) => value;
        public int Constrain(int value, int containingValue) => value;
    }
}
