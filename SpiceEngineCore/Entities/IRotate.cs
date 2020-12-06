using SpiceEngineCore.Geometry.Quaternions;

namespace SpiceEngineCore.Entities
{
    public interface IRotate
    {
        Quaternion Rotation { get; set; }
    }
}
