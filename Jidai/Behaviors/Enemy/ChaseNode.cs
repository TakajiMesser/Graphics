using OpenTK;
using System;
using System.Linq;
using TakoEngine.Entities;
using TakoEngine.Physics.Raycasting;
using TakoEngine.Scripting.Behaviors;

namespace Jidai.Behaviors.Enemy
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
            var player = context.Colliders.FirstOrDefault(c => c.AttachedEntity.GetType() == typeof(Actor) && ((Actor)c.AttachedEntity).Name == "Player");

            if (player != null)
            {
                var playerPosition = ((Actor)player.AttachedEntity).Position;
                var difference = playerPosition - context.Actor.Position;

                if (difference.Length < 3.0f)// == Vector3.Zero)
                {
                    //return BehaviorStatus.Success;
                }
                else if (difference.Length < Speed)
                {
                    context.Translation = difference;
                }
                else
                {
                    context.Translation = difference.Normalized() * Speed;
                }

                if (context.Translation != Vector3.Zero)
                {
                    float turnAngle = (float)Math.Atan2(context.Translation.Y, context.Translation.X);

                    context.Actor.Rotation = new Quaternion(turnAngle, 0.0f, 0.0f);
                    context.EulerRotation = new Vector3(turnAngle, context.EulerRotation.Y, context.EulerRotation.Z);
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
