using SavoryPhysicsCore.Bodies;
using SavoryPhysicsCore.Collisions;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SavoryPhysicsCore.Constraints
{
    public class PenetrationConstraint// : IConstraint
    {
        public static void Resolve(CollisionResult collisionResult)
        {
            // Steps (See ContactPenetrationConstraint.Update() and ContactPenetrationConstraint.SolveIteration())
            // Compute relative velocity
            // Track and clamp accumulated impulse
            // Apply linear & angular impulses to both bodies
            //var bodyA = collision.BodyA;
            //var bodyB = collision.BodyB;

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

            var relativeVelocity = GetRelativeVelocity(collisionResult.CollisionInfo, collisionResult.Collision.ContactPoint);
            float velocityAlongNormal = Vector3.Dot(relativeVelocity, collisionResult.Collision.ContactNormal);

            if (velocityAlongNormal > 0)
            {
                return;
            }

            var restitution = GetRestitution(collisionResult.CollisionInfo.BodyA, collisionResult.CollisionInfo.BodyB);
            float impulseScalar = -(1 + restitution) * velocityAlongNormal;

            var combinedInverseMass = GetInverseMass(collisionResult.CollisionInfo.BodyA, collisionResult.CollisionInfo.BodyB);
            if (combinedInverseMass > 0)
            {
                impulseScalar /= combinedInverseMass;
            }

            // TODO - Because penetrations will be resolved in an unknown order, we can't be sure that the correct impulse will take precedence
            // We should be determining the impulse based on the penetration depth. The further the penetration, the stronger the impulse
            var impulse = impulseScalar * collisionResult.Collision.ContactNormal;
            collisionResult.CollisionInfo.BodyA.ApplyImpulse(impulse);
            collisionResult.CollisionInfo.BodyB.ApplyImpulse(-impulse);

            // Intelligently distribute impulse scalar over the two objects
            /*var combinedMass = rigidBodyA.Mass + rigidBodyB.Mass;
            rigidBodyA.Velocity -= (rigidBodyA.Mass / combinedMass) * impulse;
            rigidBodyB.Velocity += (rigidBodyB.Mass / combinedMass) * impulse;*

            PositionalCorrection(rigidBodyA, rigidBodyB);*/
        }

        private static Vector3 GetRelativeVelocity(CollisionInfo collisionInfo)
        {
            var velocity = new Vector3();

            if (collisionInfo.BodyA is RigidBody rigidBodyA)
            {
                velocity += rigidBodyA.LinearVelocity;
            }

            if (collisionInfo.BodyB is RigidBody rigidBodyB)
            {
                velocity -= rigidBodyB.LinearVelocity;
            }

            return velocity;
        }

        private static Vector3 GetRelativeVelocity(CollisionInfo collisionInfo, Vector3 contactPoint)
        {
            var velocity = new Vector3();

            if (collisionInfo.BodyA is RigidBody rigidBodyA)
            {
                //velocity += rigidBodyA.LinearVelocity;
                var rb = contactPoint - collisionInfo.EntityB.Position;
                velocity += rigidBodyA.LinearVelocity + Vector3.Cross(rigidBodyA.AngularVelocity, rb);
            }

            if (collisionInfo.BodyB is RigidBody rigidBodyB)
            {
                //velocity -= rigidBodyB.LinearVelocity;
                var ra = contactPoint - collisionInfo.EntityA.Position;
                velocity -= rigidBodyB.LinearVelocity - Vector3.Cross(rigidBodyB.AngularVelocity, ra);
            }

            return velocity;
        }

        private static float GetRestitution(IBody bodyA, IBody bodyB)
        {
            var restitution = 1.0f;

            if (bodyA is RigidBody rigidBodyA && rigidBodyA.Restitution < restitution)
            {
                restitution = rigidBodyA.Restitution;
            }

            if (bodyB is RigidBody rigidBodyB && rigidBodyB.Restitution < restitution)
            {
                restitution = rigidBodyB.Restitution;
            }

            return restitution;
        }

        private static float GetInverseMass(IBody bodyA, IBody bodyB)
        {
            var inverseMass = 0.0f;

            if (bodyA is RigidBody rigidBodyA)
            {
                inverseMass += rigidBodyA.InverseMass;
            }

            if (bodyB is RigidBody rigidBodyB)
            {
                inverseMass += rigidBodyB.InverseMass;
            }

            return inverseMass;
        }

        /*private void PositionalCorrection(RigidBody bodyA, RigidBody bodyB)
        {
            // Correct floating point errors that add up over time and cause object sinking -> Use linear projection to reduce object penetration by a small percentage
            // Slop is to prevent object jitter back and forth when they are resting upon one another -> Only perform correction if penetration is above arbitrary slop threshold
            var correction = (Math.Max(PenetrationDepth - SLOP, 0.0f) / (bodyA.InverseMass + bodyB.InverseMass)) * ContactNormal * PENETRATION_REDUCTION_PERCENTAGE;
            bodyA.Position -= bodyA.InverseMass * correction;
            bodyB.Position += bodyB.InverseMass * correction;
        }*/
    }
}
