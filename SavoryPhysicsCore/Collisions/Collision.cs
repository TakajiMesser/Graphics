using SpiceEngineCore.Geometry.Vectors;

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
