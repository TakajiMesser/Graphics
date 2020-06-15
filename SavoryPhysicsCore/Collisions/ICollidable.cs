using SavoryPhysicsCore.Partitioning;

namespace SavoryPhysicsCore.Collisions
{
    public interface ICollidable
    {
        Bounds Bounds { get; set; }
        bool HasCollision { get; set; }
    }
}
