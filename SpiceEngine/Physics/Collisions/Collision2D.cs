using OpenTK;
using SpiceEngine.Physics.Bodies;
using System;
using System.Collections.Generic;

namespace SpiceEngine.Physics.Collisions
{
    public class Collision2D : ICollision
    {
        private const float PENETRATION_REDUCTION_PERCENTAGE = 0.2f; // usually 20% to 80%
        private const float SLOP = 0.01f; // usually 0.01 to 0.1

        public Body2D FirstBody { get; }
        public Body2D SecondBody { get; }

        public List<Vector2> ContactPoints { get; } = new List<Vector2>();
        public Vector2 ContactNormal { get; set; }
        public float PenetrationDepth { get; set; }

        public bool HasCollision => ContactPoints.Count > 0;

        public Collision2D(Body2D firstBody, Body2D secondBody)
        {
            FirstBody = firstBody;
            SecondBody = secondBody;
        }

        public void Resolve()
        {
            /*switch (FirstBody)
            {
                case StaticBody3D bodyA when SecondBody is RigidBody3D bodyB:
                    return Resolve(bodyA, bodyB);
                case RigidBody3D bodyA when SecondBody is StaticBody3D bodyB:
                    return Resolve(bodyB, bodyA);
                case RigidBody3D bodyA when SecondBody is RigidBody3D bodyB:
                    return Resolve(bodyA, bodyB);
            }

            throw new NotImplementedException();*/
        }

        /*private void Resolve(StaticBody3D staticBody, RigidBody3D rigidBody)
        {
            float velocityAlongNormal = Vector3.Dot(rigidBody.Velocity, ContactNormal);

            // Do not resolve if velocities are separating (?)
            if (velocityAlongNormal <= 0)
            {
                float restitution = Math.Min(staticBody.Restitution, rigidBody.Restitution);
                float impulseScalar = -(1 + restitution) * velocityAlongNormal;
                impulseScalar /= rigidBody.InverseMass;

                var impulse = impulseScalar * ContactNormal;

                rigidBody.Velocity += rigidBody.InverseMass * impulse;
            }
        }

        private void Resolve(RigidBody3D rigidBodyA, RigidBody3D rigidBodyB)
        {
            var relativeVelocity = rigidBodyB.Velocity - rigidBodyA.AngularVelocity;
            float velocityAlongNormal = Vector3.Dot(relativeVelocity, ContactNormal);

            // Do not resolve if velocities are separating (?)
            if (velocityAlongNormal <= 0)
            {
                float restitution = Math.Min(rigidBodyA.Restitution, rigidBodyB.Restitution);
                float impulseScalar = -(1 + restitution) * velocityAlongNormal;
                impulseScalar /= rigidBodyA.InverseMass + rigidBodyB.InverseMass;

                var impulse = impulseScalar * ContactNormal;

                rigidBodyA.Velocity -= rigidBodyA.InverseMass * impulse;
                rigidBodyB.Velocity += rigidBodyB.InverseMass * impulse;

                // Intelligently distribute impulse scalar over the two objects
                /*var combinedMass = rigidBodyA.Mass + rigidBodyB.Mass;
                rigidBodyA.Velocity -= (rigidBodyA.Mass / combinedMass) * impulse;
                rigidBodyB.Velocity += (rigidBodyB.Mass / combinedMass) * impulse;*
            }
        }

        private void PositionalCorrection(Body bodyA, Body bodyB)
        {
            // Correct floating point errors that add up over time and cause object sinking -> Use linear projection to reduce object penetration by a small percentage
            // Slop is to prevent object jitter back and forth when they are resting upon one another -> Only perform correction if penetration is above arbitrary slop threshold
            var correction = (Math.Max(PenetrationDepth - SLOP, 0.0f) / (bodyA.InverseMass + bodyB.InverseMass)) * ContactNormal * PENETRATION_REDUCTION_PERCENTAGE;
            bodyA.Position -= bodyA.InverseMass * correction;
            bodyB.Position += bodyB.InverseMass * correction;
        }*/
    }
}
