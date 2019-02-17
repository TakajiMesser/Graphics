using OpenTK;
using SpiceEngine.Physics.Collision;
using SpiceEngine.Physics.Shapes;
using System.Collections.Generic;

namespace SpiceEngine.Physics
{
    public interface ICollisionProvider
    {
        Body GetBody(int entityID);
        IEnumerable<int> GetCollisions(int entityID);
        IEnumerable<int> GetCollisions();
        void ApplyForce(int entityID, Vector3 translation);
    }
}
