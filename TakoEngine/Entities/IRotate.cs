using OpenTK;

namespace TakoEngine.Entities
{
    public interface IRotate
    {
        Vector3 OriginalRotation { get; set; }
        Quaternion Rotation { get; set; }
    }
}
