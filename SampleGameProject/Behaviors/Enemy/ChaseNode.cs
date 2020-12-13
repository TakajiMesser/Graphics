using SavoryPhysicsCore;
using SavoryPhysicsCore.Bodies;
using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Utilities;
using System;
using UmamiScriptingCore;
using UmamiScriptingCore.Behaviors;
using UmamiScriptingCore.Behaviors.Nodes;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SampleGameProject.Behaviors.Enemy
{
    public class ChaseNode : Node
    {
        public float Speed { get; set; }
        public float ViewAngle { get; set; }
        public float ViewDistance { get; set; }
        public string Target { get; set; }

        public ChaseNode(float speed, float viewAngle, float viewDistance, string target)
        {
            Speed = speed;
            ViewAngle = viewAngle;
            ViewDistance = viewDistance;
            Target = target;
        }

        public override BehaviorStatus Tick(BehaviorContext context)
        {
            // TODO - This will throw if Player doesn't exist
            var player = context.GetEntity("Player");

            if (context.GetEntity() is IActor actor && player != null)
            {
                var body = context.GetComponent<IBody>() as RigidBody;
                var difference = player.Position - actor.Position;

                if (difference.Length < 3.0f)// == Vector3.Zero)
                {
                    //return BehaviorStatus.Success;
                }
                else if (difference.Length < Speed)
                {
                    body.ApplyVelocity(difference);
                }
                else
                {
                    body.ApplyVelocity(difference.Normalized() * Speed);
                }

                if (body.LinearVelocity.IsSignificant())
                {
                    float turnAngle = (float)Math.Atan2(body.LinearVelocity.Y, body.LinearVelocity.X);

                    actor.Rotation = Quaternion.FromEulerAngles(0.0f, 0.0f, turnAngle);
                    context.EulerRotation = new Vector3(context.EulerRotation.X, context.EulerRotation.Y, turnAngle);
                }

                return BehaviorStatus.Running;

                /*float playerAngle = (float)Math.Atan2(playerDirection.Y, playerDirection.X);

                var angleDifference = (playerAngle - context.EulerRotation.X + Math.PI) % (2 * Math.PI) - Math.PI;
                if (angleDifference < -Math.PI)
                {
                    angleDifference += (float)(2 * Math.PI);
                }

                if (Math.Abs(angleDifference) <= ViewAngle / 2.0f)
                {
                    // Perform a raycast to see if any other colliders obstruct our view of the player
                    // TODO - Filter colliders by their ability to obstruct vision
                    if (Raycast.TryRaycast(new Ray3(context.Actor.Position, playerDirection, ViewDistance), context.Colliders, out RaycastHit hit))
                    {
                        if (hit.Collider.AttachedEntity.GetType() == typeof(Actor) && ((Actor)hit.Collider.AttachedEntity).Name == "Player")
                        {
                            //return true;
                        }
                    }
                }*/
            }
            else
            {
                return BehaviorStatus.Failure;
            }
        }

        public override void Reset() { }
    }
}
