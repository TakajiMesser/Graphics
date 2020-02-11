using SpiceEngineCore.Physics;

namespace UmamiPhysicsCore.Collisions
{
    public interface ICollidable
    {
        Bounds Bounds { get; set; }
        bool HasCollision { get; set; }
    }
}
