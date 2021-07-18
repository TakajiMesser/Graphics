using SpiceEngineCore.Entities;

namespace SpiceEngineCore.Maps
{
    public interface IMapLight : IMapEntity
    {
        void UpdateFrom(ILight light);
    }
}
