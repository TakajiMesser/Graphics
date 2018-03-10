using TakoEngine.Scripting.BehaviorTrees.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoEngine.Scripting.BehaviorTrees;
using TakoEngine.GameObjects;
using TakoEngine.Physics.Raycasting;
using OpenTK;
using System.Runtime.Serialization;

namespace GraphicsTest.Behaviors.Enemy
{
    [DataContract]
    public class ChaseNode : LeafNode
    {
        [DataMember]
        public float ViewAngle { get; set; }

        [DataMember]
        public float ViewDistance { get; set; }

        [DataMember]
        public string Target { get; set; }

        public ChaseNode(float viewAngle, float viewDistance, string target)
        {
            ViewAngle = viewAngle;
            ViewDistance = viewDistance;
            Target = target;
        }

        public override BehaviorStatuses Behavior(BehaviorContext context)
        {
            var player = context.Colliders.FirstOrDefault(c => c.AttachedObject.GetType() == typeof(GameObject) && ((GameObject)c.AttachedObject).Name == "Player");

            if (player != null)
            {
                var playerPosition = ((GameObject)player.AttachedObject).Model.Position;

                var playerDirection = playerPosition - context.Position;
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
                    if (Raycast.TryRaycast(new Ray3(context.Position, playerDirection, ViewDistance), context.Colliders, out RaycastHit hit))
                    {
                        if (hit.Collider.AttachedObject.GetType() == typeof(GameObject) && ((GameObject)hit.Collider.AttachedObject).Name == "Player")
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

            return BehaviorStatuses.Failure;
        }
    }
}
