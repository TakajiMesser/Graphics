using SpiceEngineCore.Entities;
using SpiceEngineCore.Game.Loading.Builders;

namespace SpiceEngineCore.Maps
{
    public interface IMapVolume : IMapEntity, IShapeBuilder
    {
        void UpdateFrom(IVolume volume);
    }
}
