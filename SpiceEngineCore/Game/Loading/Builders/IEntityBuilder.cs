using SpiceEngineCore.Entities;
using SpiceEngineCore.Geometry.Vectors;

namespace SpiceEngineCore.Game.Loading.Builders
{
    public interface IEntityBuilder
    {
        Vector3 Position { get; set; }

        IEntity ToEntity();
    }
}
