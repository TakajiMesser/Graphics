using OpenTK;

namespace SpiceEngine.Entities
{
    public interface IRotate
    {
        Quaternion Rotation { get; set; }

        void Rotate(Quaternion rotation);
    }
}
