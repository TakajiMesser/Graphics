using SpiceEngine.Physics.Collision;
using SpiceEngine.Physics.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceEngine.Physics
{
    public class EntityCollision
    {
        public int EntityID { get; private set; }

        public IShape Shape { get; set; }
        public Bounds Bounds { get; set; }
        public IEnumerable<Bounds> Colliders { get; set; }
        public IEnumerable<PhysicsBody> Bodies { get; set; }

        public EntityCollision(int entityID)
        {
            EntityID = entityID;
        }
    }
}
