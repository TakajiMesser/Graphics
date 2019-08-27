using OpenTK;
using SpiceEngine.Entities;

namespace SpiceEngine.Maps.Builders
{
    public interface IEntityBuilder
    {
        Vector3 Position { get; set; }

        IEntity ToEntity();
    }
}
