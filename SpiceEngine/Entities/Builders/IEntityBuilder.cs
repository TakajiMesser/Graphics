using OpenTK;

namespace SpiceEngine.Entities.Builders
{
    public interface IEntityBuilder
    {
        Vector3 Position { get; set; }

        IEntity ToEntity();
    }
}
