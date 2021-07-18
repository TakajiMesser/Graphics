using SpiceEngineCore.Entities;
using SpiceEngineCore.Game.Loading.Builders;

namespace SpiceEngineCore.Maps
{
    public interface IMapVolume : IMapEntity, IRenderableBuilder//, IShapeBuilder
    {
        void UpdateFrom(IVolume volume);
    }
}
