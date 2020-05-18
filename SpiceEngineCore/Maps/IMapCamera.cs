using SpiceEngineCore.Components.Builders;
using SpiceEngineCore.Entities.Cameras;

namespace SpiceEngineCore.Maps
{
    public interface IMapCamera : IMapEntity, IBehaviorBuilder
    {
        string AttachedEntityName { get; }

        void UpdateFrom(ICamera camera);
    }
}
