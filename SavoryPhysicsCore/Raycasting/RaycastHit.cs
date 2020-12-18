using OpenTK;

namespace SavoryPhysicsCore.Raycasting
{
    public class RaycastHit
    {
        public int EntityID { get; set; }
        public Vector3 Intersection { get; set; }
    }
}
