using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Game.Loading.Builders;

namespace SpiceEngineCore.Maps
{
    public interface IMapCamera : IMapEntity, IBehaviorBuilder
    {
        string AttachedEntityName { get; }

        void UpdateFrom(ICamera camera);
    }
}
