﻿using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Physics.Shapes;
using System;

namespace SpiceEngine.Physics.Bodies
{
    public class RigidBody3D : Body3D
    {
        private float _mass = 1.0f;

        public Quaternion Rotation { get; set; }

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

        //public Vector3 LinearAcceleration { get; private set; }
        //public Vector2 AngularAcceleration { get; private set; }

        public Vector3 Force { get; private set; }
        public Vector3 Torque { get; private set; }

        public event EventHandler<RigidBodyEventArgs> Moved;
        public event EventHandler<RigidBodyEventArgs> ForceApplied;

        public RigidBody3D(IEntity entity, Shape3D shape) : base(entity, shape)
        {
            
        }

        public void Update(int nTicks)
        {
            // TODO - Apply linear damping
            // TODO - Apply angular damping
            var linearAcceleration = Force / Mass;
            LinearVelocity += linearAcceleration * nTicks;
            Position += LinearVelocity * nTicks;

            var angularAcceleration = Torque / MomentOfInertia;
            AngularVelocity += angularAcceleration * nTicks;

            // Convert AngularVelocity into a Quaternion form, which will be used to integrate Orientation
            var angularVelocityQuaternion = new Quaternion()
            {
                X = 0,
                Y = AngularVelocity.X,
                Z = AngularVelocity.Y,
                W = AngularVelocity.Z
            };
            Rotation += (nTicks / 2) * angularVelocityQuaternion * Rotation;

            if (LinearVelocity != Vector3.Zero || AngularVelocity != Vector3.Zero)
            {
                Moved?.Invoke(this, new RigidBodyEventArgs(this));
            }

            Force = Vector3.Zero;
            Torque = Vector3.Zero;
        }

        // An impulse is an instantaneous change in velocity
        public void ApplyImpulse(Vector3 impulse)
        {
            LinearVelocity += InverseMass * impulse;
            ForceApplied?.Invoke(this, new RigidBodyEventArgs(this));
        }

        public void ApplyVelocity(Vector3 velocity)
        {
            LinearVelocity = velocity;
            ForceApplied?.Invoke(this, new RigidBodyEventArgs(this));
        }

        // Assume the force here is applied directly to the center of mass
        public void ApplyForce(Vector3 force)
        {
            if (force != Vector3.Zero)
            {
                Force = force;
                ForceApplied?.Invoke(this, new RigidBodyEventArgs(this));
            }
        }

        public void ApplyForce(Vector3 force, Vector3 point)
        {
            Force = force;
            Torque = Vector3.Cross(point - Position, force);

            if (force != Vector3.Zero)
            {
                Force = force;
                ForceApplied?.Invoke(this, new RigidBodyEventArgs(this));
            }
        }

        public class RigidBodyEventArgs : EventArgs
        {
            public RigidBody3D Body { get; }

            public RigidBodyEventArgs(RigidBody3D body)
            {
                Body = body;
            }
        }
    }
}
