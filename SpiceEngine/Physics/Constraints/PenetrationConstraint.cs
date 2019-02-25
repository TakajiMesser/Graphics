using OpenTK;
using SpiceEngine.Physics.Bodies;
using SpiceEngine.Physics.Collisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceEngine.Physics.Constraints
{
    public class PenetrationConstraint : Constraint
    {
        public static void Resolve(Collision3D collision)
        {
            // Steps (See ContactPenetrationConstraint.Update() and ContactPenetrationConstraint.SolveIteration())
            // Compute relative velocity
            // Track and clamp accumulated impulse
            // Apply linear & angular impulses to both bodies
            var bodyA = collision.FirstBody;
            var bodyB = collision.SecondBody;

            /*var relativeVelocity = GetRelativeVelocity(bodyA, bodyB);
            float velocityAlongNormal = Vector3.Dot(relativeVelocity, collision.ContactNormal);

            // Do not resolve if velocities are separating (?)
            if (velocityAlongNormal <= 0)
            {
                float restitution = GetRestitution(bodyA, bodyB);
                float impulseScalar = -(1 + restitution) * velocityAlongNormal;

                var combinedInverseMass = GetInverseMass(bodyA, bodyB);
                if (combinedInverseMass > 0)
                {
                    impulseScalar /= combinedInverseMass;
                }

                var impulse = impulseScalar * collision.ContactNormal;
                ApplyImpulse(bodyA, impulse);
                ApplyImpulse(bodyB, -impulse);

                //Frequency.ToString("0.##")
                /*LogManager.LogToScreen("Sphere = (" + rigidBodyA.LinearVelocity.X.ToString("0.##")
                    + ", " + rigidBodyA.LinearVelocity.Y.ToString("0.##")
                    + ", " + rigidBodyA.LinearVelocity.Z.ToString("0.##")
                    + "    Box = (" + rigidBodyB.LinearVelocity.X.ToString("0.##")
                    + ", " + rigidBodyB.LinearVelocity.Y.ToString("0.##")
                    + ", " + rigidBodyB.LinearVelocity.Z.ToString("0.##"));*
            }*/

            foreach (var contactPoint in collision.ContactPoints)
            {
                var relativeVelocity = GetRelativeVelocity(contactPoint, bodyA, bodyB);

                float velocityAlongNormal = Vector3.Dot(relativeVelocity, collision.ContactNormal);

                if (velocityAlongNormal > 0)
                {
                    return;
                }

                var restitution = GetRestitution(bodyA, bodyB);
                float impulseScalar = -(1 + restitution) * velocityAlongNormal;

                var combinedInverseMass = GetInverseMass(bodyA, bodyB);
                if (combinedInverseMass > 0)
                {
                    impulseScalar /= combinedInverseMass;
                }

                var impulse = impulseScalar * collision.ContactNormal;
                ApplyImpulse(bodyA, impulse);
                ApplyImpulse(bodyB, -impulse);

                // Intelligently distribute impulse scalar over the two objects
                /*var combinedMass = rigidBodyA.Mass + rigidBodyB.Mass;
                rigidBodyA.Velocity -= (rigidBodyA.Mass / combinedMass) * impulse;
                rigidBodyB.Velocity += (rigidBodyB.Mass / combinedMass) * impulse;*

                PositionalCorrection(rigidBodyA, rigidBodyB);*/
            }
        }

        private static Vector3 GetRelativeVelocity(Body3D bodyA, Body3D bodyB)
        {
            var velocity = new Vector3();

            if (bodyA is RigidBody3D rigidBodyA)
            {
                velocity += rigidBodyA.LinearVelocity;
            }

            if (bodyB is RigidBody3D rigidBodyB)
            {
                velocity -= rigidBodyB.LinearVelocity;
            }

            return velocity;
        }

        private static Vector3 GetRelativeVelocity(Vector3 contactPoint, Body3D bodyA, Body3D bodyB)
        {
            var velocity = new Vector3();

            if (bodyA is RigidBody3D rigidBodyA)
            {
                //velocity += rigidBodyA.LinearVelocity;
                var rb = contactPoint - bodyB.Position;
                velocity += rigidBodyA.LinearVelocity + Vector3.Cross(rigidBodyA.AngularVelocity, rb);
            }

            if (bodyB is RigidBody3D rigidBodyB)
            {
                //velocity -= rigidBodyB.LinearVelocity;
                var ra = contactPoint - bodyA.Position;
                velocity -= rigidBodyB.LinearVelocity - Vector3.Cross(rigidBodyB.AngularVelocity, ra);
            }

            return velocity;
        }

        private static float GetRestitution(Body3D bodyA, Body3D bodyB)
        {
            var restitution = 1.0f;

            if (bodyA is RigidBody3D rigidBodyA && rigidBodyA.Restitution < restitution)
            {
                restitution = rigidBodyA.Restitution;
            }

            if (bodyB is RigidBody3D rigidBodyB && rigidBodyB.Restitution < restitution)
            {
                restitution = rigidBodyB.Restitution;
            }

            return restitution;
        }

        private static float GetInverseMass(Body3D bodyA, Body3D bodyB)
        {
            var inverseMass = 0.0f;

            if (bodyA is RigidBody3D rigidBodyA)
            {
                inverseMass += rigidBodyA.InverseMass;
            }

            if (bodyB is RigidBody3D rigidBodyB)
            {
                inverseMass += rigidBodyB.InverseMass;
            }

            return inverseMass;
        }

        private static void ApplyImpulse(Body3D body, Vector3 impulse)
        {
            if (body is RigidBody3D rigidBody)
            {
                rigidBody.ApplyImpulse(impulse);
            }
        }
    }
}
