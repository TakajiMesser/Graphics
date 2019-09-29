using SpiceEngineCore.Physics.Bodies;
using System.Collections.Generic;

namespace SpiceEngineCore.Physics.Collisions
{
    public interface ICollisionProvider
    {
        IBody GetBody(int entityID);

        IEnumerable<Collision3D> GetCollisions();
        IEnumerable<Collision3D> GetCollisions(int entityID);

        IEnumerable<int> GetCollisionIDs();
        IEnumerable<int> GetCollisionIDs(int entityID);

        //void ApplyImpulse(int entityID, Vector3 translation);
        //void ApplyForce(int entityID, Vector3 translation);
    }
}
