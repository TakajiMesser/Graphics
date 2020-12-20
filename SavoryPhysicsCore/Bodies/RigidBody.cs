using SavoryPhysicsCore.Shapes;
using SpiceEngineCore.Utilities;
using System;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SavoryPhysicsCore.Bodies
{
    public class RigidBody : Body
    {
        private float _mass = 1.0f;

        // For now, an impulse explicitly means that the body collided with another body and the penetration constraint had to be resolved
        // If this happens, as a cheap shortcut for now, ignore any forces this update cycle...
        private bool _isForcePreempted = false;
        private bool _isTorquePreempted = false;

        public RigidBody(int entityID, IShape shape) : base(entityID, shape) { }

        public Vector3 LinearVelocity { get; set; }
        public Vector3 AngularVelocity { get; set; }
        public Quaternion Spin { get; private set; }

        public float Mass
        {
            get => _mass;
            set
            {
                _mass = value;
                InverseMass = 1 / value;
                MomentOfInertia = Shape.CalculateInertia(value);
                InverseInertia = 1 / MomentOfInertia;
            }
        }
        public float InverseMass { get; private set; } = 1.0f;

        public float MomentOfInertia { get; private set; } = 1.0f;
        public float InverseInertia { get; private set; } = 1.0f;

        public Vector3 Force { get; private set; }
        public Vector3 Torque { get; private set; }

        public event EventHandler<RigidBodyEventArgs> Updated;
        public event EventHandler<RigidBodyEventArgs> Influenced;

        public override Vector3 UpdatePosition(Vector3 position, int nTicks)
        {
            // TODO - Apply linear damping
            if (!_isForcePreempted)
            {
                var linearAcceleration = Force / Mass;
                LinearVelocity += linearAcceleration * nTicks;
            }

            var positionDelta = LinearVelocity * nTicks;
            Force = Vector3.Zero;
            _isForcePreempted = false;

            return positionDelta.IsSignificant() ? position + positionDelta : position;
        }

        public override Quaternion UpdateRotation(Quaternion rotation, int nTicks)
        {
            // TODO - Apply angular damping
            if (!_isTorquePreempted)
            {
                var angularAcceleration = Torque / MomentOfInertia;
                AngularVelocity += angularAcceleration * nTicks;
            }
            
            // Convert AngularVelocity into a Quaternion form, which will be used to integrate Orientation
            var rotationDelta = (nTicks / 2) * new Quaternion(0.0f, AngularVelocity.X, AngularVelocity.Y, AngularVelocity.Z) * rotation;
            Torque = Vector3.Zero;
            _isTorquePreempted = false;

            return rotationDelta.IsSignificant() ? rotation + rotationDelta : rotation;
        }

        // An impulse is an instantaneous change in velocity
        public override void ApplyImpulse(Vector3 impulse)
        {
            _isForcePreempted = true;
            _isTorquePreempted = true;

            LinearVelocity += InverseMass * impulse;
            Influenced?.Invoke(this, new RigidBodyEventArgs(this));
        }

        public override void ApplyVelocity(Vector3 velocity)
        {
            LinearVelocity = velocity;
            Influenced?.Invoke(this, new RigidBodyEventArgs(this));
        }

        // Assume the force here is applied directly to the center of mass
        public override void ApplyForce(Vector3 force)
        {
            if (force.IsSignificant() && !_isForcePreempted)
            {
                Force += force;
                Influenced?.Invoke(this, new RigidBodyEventArgs(this));
            }
        }

        public override void ApplyPreciseForce(Vector3 force, Vector3 point)
        {
            if (force.IsSignificant() && !_isForcePreempted)
            {
                Force += force;

                if (!_isTorquePreempted)
                {
                    // TODO - FIX ME
                    //Torque += Vector3.Cross(point - Position, force);
                }
                
                Influenced?.Invoke(this, new RigidBodyEventArgs(this));
            }
        }

        public class RigidBodyEventArgs : EventArgs
        {
            public RigidBody Body { get; }

            public RigidBodyEventArgs(RigidBody body) => Body = body;
        }
    }
}
