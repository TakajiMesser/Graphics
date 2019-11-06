using SpiceEngineCore.Entities.Cameras;

namespace SpiceEngineCore.Maps
{
    public interface IMapCamera : IMapEntity3D
    {
        string AttachedEntityName { get; }

        void UpdateFrom(ICamera camera);
    }
}
