using System;
using System.Linq;
using System.Runtime.Serialization;
using SpiceEngine.Entities;
using SpiceEngine.Physics.Raycasting;
using SpiceEngine.Scripting.Behaviors;
using SpiceEngine.Scripting.Behaviors.Decorators;

namespace Jidai.Behaviors.Enemy
{
    [DataContract]
    public class IsPlayerInSightNode : ConditionNode
    {
        [DataMember]
        public float ViewAngle { get; set; }

        [DataMember]
        public float ViewDistance { get; set; }

        public IsPlayerInSightNode(float viewAngle, float viewDistance, Node node) : base(node)
        {
            ViewAngle = viewAngle;
            ViewDistance = viewDistance;
        }

        public override bool Condition(BehaviorContext context)
        {
            var player = context.Colliders.FirstOrDefault(c => c.AttachedEntity.GetType() == typeof(Actor) && ((Actor)c.AttachedEntity).Name == "Player");

            if (player != null)
            {
                var playerPosition = ((Actor)player.AttachedEntity).Model.Position;

                var playerDirection = playerPosition - context.Actor.Position;
                float playerAngle = (float)Math.Atan2(playerDirection.Y, playerDirection.X);

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
                            return true;
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

            return false;
        }

        public override void Reset() { }
    }
}
