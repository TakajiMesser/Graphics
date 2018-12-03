using OpenTK;
using SpiceEngine.Physics.Collision;
using SpiceEngine.Physics.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceEngine.Physics
{
    public class RigidBody : PhysicsBody
    {
        public Vector3 LinearVelocity { get; private set; }
        public Vector2 AngularVelocity { get; private set; }
        public Vector3 Force { get; private set; }
        public int Torque { get; private set; }

        public RigidBody(int entityID, IShape shape) : base(entityID, shape) { }

        public void ApplyForce(Vector3 force)
        {
            
        }

        public void ApplyForce(Vector3 force, int torque)
        {

        }
    }
}
