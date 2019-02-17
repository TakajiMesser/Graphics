using SpiceEngine.Physics.Collision;
using SpiceEngine.Physics.Shapes;
using System.Collections.Generic;

namespace SpiceEngine.Physics
{
    public class EntityCollision
    {
        public int EntityID { get; private set; }

        public IShape Shape { get; set; }
        public Bounds Bounds { get; set; }
        public IEnumerable<Bounds> Colliders { get; set; }
        public IEnumerable<Body> Bodies { get; set; }

        public EntityCollision(int entityID)
        {
            EntityID = entityID;
        }
    }
}
