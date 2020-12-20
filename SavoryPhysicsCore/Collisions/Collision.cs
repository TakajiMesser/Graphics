using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SavoryPhysicsCore.Collisions
{
    public class Collision : ICollision
    {
        private const float PENETRATION_REDUCTION_PERCENTAGE = 0.4f; // usually 20% to 80%
        private const float SLOP = 0.05f; // usually 0.01 to 0.1

        public Vector3 ContactPoint { get; set; }
        public Vector3 ContactNormal { get; set; }
        public float PenetrationDepth { get; set; }
    }
}
