using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Outputs;

namespace SpiceEngineCore.Maps
{
    public interface IMapCamera : IMapEntity3D
    {
        Resolution Resolution { get; set; }

        void UpdateFrom(ICamera camera);
    }
}
