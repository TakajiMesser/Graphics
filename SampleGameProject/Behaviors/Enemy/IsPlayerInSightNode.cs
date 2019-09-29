using SpiceEngine.Entities.Actors;
using SpiceEngine.Scripting;
using SpiceEngine.Scripting.Nodes;
using SpiceEngine.Scripting.Nodes.Decorators;
using SpiceEngineCore.Physics.Bodies;
using SpiceEngineCore.Physics.Raycasting;
using SpiceEngineCore.Utilities;
using System;
using System.Runtime.Serialization;

namespace SampleGameProject.Behaviors.Enemy
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
            var player = context.GetActor("Player");

            if (player != null)
            {
                var playerBody = context.GetBody(player.ID) as RigidBody3D;
                var playerPosition = playerBody.Position;

                var playerDirection = playerPosition - context.Position;
                float playerAngle = (float)Math.Atan2(playerDirection.Y, playerDirection.X);

                var angleDifference = (playerAngle - context.EulerRotation.X + MathExtensions.PI) % MathExtensions.TWO_PI - MathExtensions.PI;
                if (angleDifference < -MathExtensions.PI)
                {
                    angleDifference += MathExtensions.TWO_PI;
                }

                if (Math.Abs(angleDifference) <= ViewAngle / 2.0f)
                {
                    // Perform a raycast to see if any other colliders obstruct our view of the player
                    // TODO - Filter colliders by their ability to obstruct vision
                    var colliders = context.GetColliderBodies();

                    if (Raycast.TryRaycast(new Ray3(context.Position, playerDirection, ViewDistance), colliders, context.GetEntityProvider(), out RaycastHit hit))
                    {
                        var hitEntity = context.GetEntity(hit.EntityID);

                        if (hitEntity.GetType() == typeof(Actor) && ((Actor)hitEntity).Name == "Player")
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
