using System.Collections.Generic;

namespace SavoryPhysicsCore.Collisions
{
    public interface ICollisionProvider
    {
        //IBody GetBody(int entityID);

        IEnumerable<CollisionResult> GetCollisions();
        IEnumerable<CollisionResult> GetCollisions(int entityID);

        IEnumerable<int> GetCollisionIDs();
        IEnumerable<int> GetCollisionIDs(int entityID);

        //void ApplyImpulse(int entityID, Vector3 translation);
        //void ApplyForce(int entityID, Vector3 translation);
    }
}
