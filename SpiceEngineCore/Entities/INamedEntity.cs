namespace SpiceEngineCore.Entities
{
    public interface INamedEntity : IEntity
    {
        string Name { get; set; }
    }
}
