using SavoryPhysicsCore.Partitioning;
using SpiceEngineCore.Geometry.Vectors;

namespace SavoryPhysicsCore.Shapes
{
    public interface IShape
    {
        IShape Duplicate();

        IPartition ToPartition(Vector3 position);
        Vector3 GetFurthestPointInDirection(Vector3 direction);
        float CalculateInertia(float mass);
    }
}
