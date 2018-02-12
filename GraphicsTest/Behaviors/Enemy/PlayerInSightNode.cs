using Graphics.Scripting.BehaviorTrees.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphics.Scripting.BehaviorTrees;
using Graphics.GameObjects;
using Graphics.Physics.Raycasting;
using System.Runtime.Serialization;

namespace GraphicsTest.Behaviors.Enemy
{
    [DataContract]
    public class PlayerInSightNode : LeafNode
    {
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

                var viewAngle = context.GetProperty<float>("VIEW_ANGLE");
                if (Math.Abs(angleDifference) <= viewAngle / 2.0f)
                {
                    // Perform a raycast to see if any other colliders obstruct our view of the player
                    // TODO - Filter colliders by their ability to obstruct vision
                    var viewDistance = context.GetProperty<float>("VIEW_DISTANCE");
                    if (Raycast.TryRaycast(new Ray3(context.Position, playerDirection, viewDistance), context.Colliders, out RaycastHit hit))
                    {
                        if (hit.Collider.AttachedObject.GetType() == typeof(GameObject) && ((GameObject)hit.Collider.AttachedObject).Name == "Player")
                        {
                            return BehaviorStatuses.Success;
                        }
                    }
                }
            }

            return BehaviorStatuses.Failure;
        }
    }
}
