using SpiceEngineCore.Geometry.Vectors;

namespace SpiceEngine.Entities.Selection
{
    public interface IDirectional
    {
        Vector3 XDirection { get; }
        Vector3 YDirection { get; }
        Vector3 ZDirection { get; }
    }
}
