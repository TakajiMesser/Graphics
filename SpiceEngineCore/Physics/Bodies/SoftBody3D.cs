using OpenTK;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Physics.Shapes;

namespace SpiceEngineCore.Physics.Bodies
{
    public class SoftBody3D : Body3D
    {
        public SoftBody3D(IEntity entity, Shape3D shape) : base(entity, shape)
        {
            
        }

        public void Update(int nTicks)
        {

        }

        // An impulse is an instantaneous change in velocity
        public void ApplyImpulse(Vector3 impulse)
        {
            
        }

        public void ApplyVelocity(Vector3 velocity)
        {
            
        }

        // Assume the force here is applied directly to the center of mass
        public void ApplyForce(Vector3 force)
        {
            
        }

        public void ApplyForce(Vector3 force, Vector3 point)
        {
            
        }
    }
}
