using SpiceEngine.Physics.Collision;
using SpiceEngine.Physics.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceEngine.Physics
{
    public abstract class PhysicsBody
    {
        public int EntityID { get; }
        public IShape Shape { get; }

        public PhysicsBody(int entityID, IShape shape)
        {
            EntityID = entityID;
            Shape = shape;
        }
    }
}
