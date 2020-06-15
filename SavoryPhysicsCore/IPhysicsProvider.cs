using SavoryPhysicsCore.Collisions;
using SpiceEngineCore.Game;
using System.Collections.Generic;

namespace SavoryPhysicsCore
{
    public interface IPhysicsProvider : IGameSystem, IComponentProvider<IBody>
    {
        IEnumerable<CollisionResult> GetCollisions();
        IEnumerable<CollisionResult> GetCollisions(int entityID);

        IEnumerable<int> GetCollisionIDs();
        IEnumerable<int> GetCollisionIDs(int entityID);

        IEnumerable<IBody> GetCollisionBodies(int entityID);
    }
}
