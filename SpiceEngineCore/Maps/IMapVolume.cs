using SpiceEngineCore.Components.Builders;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Game.Loading.Builders;

namespace SpiceEngineCore.Maps
{
    public interface IMapVolume : IMapEntity, IShapeBuilder, IRenderableBuilder
    {
        void UpdateFrom(IVolume volume);
    }
}
