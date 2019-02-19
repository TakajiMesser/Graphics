using OpenTK;
using SpiceEngine.Physics.Collisions;

namespace SpiceEngine.Physics.Shapes
{
    public interface IShape
    {
        IPartition ToPartition(Vector3 position);
        IShape Duplicate();
        float CalculateInertia(float mass);
    }
}
