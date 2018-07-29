using OpenTK;
using SpiceEngine.Physics.Collision;

namespace SpiceEngine.Physics.Raycasting
{
    public class RaycastHit
    {
        public Bounds Collider { get; set; }
        public Vector3 Intersection { get; set; }
    }
}
