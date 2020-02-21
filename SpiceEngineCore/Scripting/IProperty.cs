namespace SpiceEngineCore.Scripting
{
    public interface IProperty
    {
        string Name { get; }
        bool IsConstant { get; }
    }
}
