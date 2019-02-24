using OpenTK;
using SpiceEngine.Physics.Bodies;
using SpiceEngine.Physics.Collisions;
using SpiceEngine.Physics.Shapes;
using System.Collections.Generic;

namespace SpiceEngine.Physics
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
