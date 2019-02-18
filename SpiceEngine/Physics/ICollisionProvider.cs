using OpenTK;
using SpiceEngine.Physics.Collisions;
using SpiceEngine.Physics.Shapes;
using System.Collections.Generic;

namespace SpiceEngine.Physics
{
    public interface ICollisionProvider
    {
        Body GetBody(int entityID);

        IEnumerable<Collision> GetCollisions();
        IEnumerable<Collision> GetCollisions(int entityID);

        IEnumerable<int> GetCollisionIDs();
        IEnumerable<int> GetCollisionIDs(int entityID);

        void ApplyForce(int entityID, Vector3 translation);
    }
}
