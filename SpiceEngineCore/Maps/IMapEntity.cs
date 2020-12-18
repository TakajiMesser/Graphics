using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Geometry.Vectors;

namespace SpiceEngineCore.Maps
{
    public interface IMapEntity : IEntityBuilder//, IComponentBuilder
    {
        //Vector3 Position { get; set; }
        Vector3 Rotation { get; set; }
        Vector3 Scale { get; set; }
    }
}
