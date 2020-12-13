using SavoryPhysicsCore.Partitioning;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

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
