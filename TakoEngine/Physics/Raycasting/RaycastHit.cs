using OpenTK;
using TakoEngine.Physics.Collision;

namespace TakoEngine.Physics.Raycasting
{
    public class RaycastHit
    {
        public Bounds Collider { get; set; }
        public Vector3 Intersection { get; set; }
    }
}
