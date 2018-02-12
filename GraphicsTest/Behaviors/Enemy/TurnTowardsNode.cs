using Graphics.Scripting.BehaviorTrees.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphics.Scripting.BehaviorTrees;
using Graphics.GameObjects;
using Graphics.Physics.Raycasting;
using OpenTK;
using System.Runtime.Serialization;

namespace GraphicsTest.Behaviors.Enemy
{
    [DataContract]
    public class TurnTowardsNode : LeafNode
    {
        public override BehaviorStatuses Behavior(BehaviorContext context)
        {
            if (context.Translation != Vector3.Zero)
            {
                float turnAngle = (float)Math.Atan2(context.Translation.Y, context.Translation.X);

                context.QRotation = new Quaternion(turnAngle, 0.0f, 0.0f);
                context.Rotation = new Vector3(turnAngle, context.Rotation.Y, context.Rotation.Z);
            }

            return BehaviorStatuses.Success;
        }
    }
}
