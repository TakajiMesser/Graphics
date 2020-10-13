using SavoryPhysicsCore;
using SavoryPhysicsCore.Raycasting;
using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Utilities;
using System;
using System.Runtime.Serialization;
using UmamiScriptingCore.Behaviors;
using UmamiScriptingCore.Behaviors.Nodes;
using UmamiScriptingCore.Behaviors.Nodes.Decorators;

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

        protected override bool Condition(BehaviorContext context)
        {
            var player = context.GetEntity("Player");

            if (player != null)
            {
                var playerPosition = context.GetPosition();

                var playerDirection = playerPosition - context.GetPosition();
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
                    var colliders = context.SystemProvider.GetGameSystem<IPhysicsProvider>().GetCollisionBodies(context.GetEntity().ID);

                    if (Raycast.TryRaycast(new Ray3(context.GetPosition(), playerDirection, ViewDistance), colliders, context.SystemProvider.EntityProvider, out RaycastHit hit))
                    {
                        if (context.GetEntity(hit.EntityID) is IActor actor && actor.Name == "Player")
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
