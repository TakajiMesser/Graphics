namespace StarchUICore.Attributes.Units
{
    public interface IUnits
    {
        //int Constrain(int value);
        int Constrain(int value, int containingValue);
        //int ConstrainMinimum(int value);
        int GetValue(int containingValue);
    }
}
