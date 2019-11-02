using SpiceEngineCore.Entities;
using SpiceEngineCore.Game.Loading.Builders;

namespace SpiceEngineCore.Maps
{
    public interface IMapVolume : IMapEntity3D, IShapeBuilder
    {
        void UpdateFrom(IVolume volume);
    }
}
