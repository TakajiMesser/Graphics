using OpenTK;
using SpiceEngine.Physics.Shapes;

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
