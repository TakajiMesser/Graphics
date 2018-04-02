using System;
using System.Linq;
using TakoEngine.Entities;
using TakoEngine.Physics.Raycasting;
using TakoEngine.Scripting.Behaviors;

namespace GraphicsTest.Behaviors.Enemy
{
    public class ChaseNode : Node
    {
        public float ViewAngle { get; set; }
        public float ViewDistance { get; set; }
        public string Target { get; set; }

        public ChaseNode(float viewAngle, float viewDistance, string target)
        {
            ViewAngle = viewAngle;
            ViewDistance = viewDistance;
            Target = target;
        }

        public override BehaviorStatus Tick(BehaviorContext context)
        {
            var player = context.Colliders.FirstOrDefault(c => c.AttachedEntity.GetType() == typeof(Actor) && ((Actor)c.AttachedEntity).Name == "Player");

            if (player != null)
            {
                var playerPosition = ((Actor)player.AttachedEntity).Model.Position;

                var playerDirection = playerPosition - context.Actor.Position;
                float playerAngle = (float)Math.Atan2(playerDirection.Y, playerDirection.X);

                var angleDifference = (playerAngle - context.Rotation.X + Math.PI) % (2 * Math.PI) - Math.PI;
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
                }
            }

            if (context.ContainsVariable("nAlertTicks"))
            {
                int nAlertTicks = context.GetVariable<int>("nAlertTicks");

                if (nAlertTicks > 0)
                {
                    nAlertTicks--;
                    context.SetVariable("nAlertTicks", nAlertTicks);
                }
            }

            return BehaviorStatus.Failure;
        }
    }
}
