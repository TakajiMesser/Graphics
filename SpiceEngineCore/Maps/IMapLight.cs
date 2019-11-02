using SpiceEngineCore.Entities;

namespace SpiceEngineCore.Maps
{
    public interface IMapLight : IMapEntity3D
    {
        void UpdateFrom(ILight light);
    }
}
