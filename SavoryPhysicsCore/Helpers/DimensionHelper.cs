using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SavoryPhysicsCore.Helpers
{
    public static class DimensionHelper
    {
        public static Vector3 Flattened(this Vector3 vector) => new Vector3(
            vector.X,
            vector.Y,
            0.0f
        );

        public static Vector3 ToVector3(this Vector2 vector) => new Vector3(
            vector.X,
            vector.Y,
            0.0f
        );
    }
}
