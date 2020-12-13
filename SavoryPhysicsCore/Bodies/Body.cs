using SavoryPhysicsCore.Shapes;
using SpiceEngineCore.Components;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SavoryPhysicsCore.Bodies
{
    public abstract class Body : Component, IBody
    {
        public Body(int entityID, IShape shape) : base(entityID) => Shape = shape;

        public IShape Shape { get; }
        public BodyStates State { get; set; }
        public float Restitution { get; set; }

        public bool IsMovable => this is RigidBody || this is SoftBody;
        public bool IsPhysical { get; set; }

        public virtual Vector3 UpdatePosition(Vector3 position, int nTicks) => position;
        
        public virtual Quaternion UpdateRotation(Quaternion rotation, int nTicks) => rotation;

        // An impulse is an instantaneous change in velocity
        public virtual void ApplyImpulse(Vector3 impulse) { }

        public virtual void ApplyVelocity(Vector3 velocity) { }

        // Assume the force here is applied directly to the center of mass
        public virtual void ApplyForce(Vector3 force) { }

        public virtual void ApplyPreciseForce(Vector3 force, Vector3 point) { }
    }
}
