using System.Collections.Generic;

namespace SpiceEngineCore.Physics
{
    public interface ICollisionProvider
    {
        IBody GetBody(int entityID);

        IEnumerable<ICollision> GetCollisions();
        IEnumerable<ICollision> GetCollisions(int entityID);

        IEnumerable<int> GetCollisionIDs();
        IEnumerable<int> GetCollisionIDs(int entityID);

        //void ApplyImpulse(int entityID, Vector3 translation);
        //void ApplyForce(int entityID, Vector3 translation);
    }
}
