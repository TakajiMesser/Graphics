using OpenTK;
using SpiceEngineCore.Physics.Collisions;

namespace SpiceEngineCore.Physics.Shapes
{
    public interface IShape
    {
        IPartition ToPartition(Vector3 position);
        //IShape Duplicate();
        float CalculateInertia(float mass);
    }
}
