using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoEngine.Scripting.Behaviors;
using TakoEngine.Entities;
using TakoEngine.Physics.Raycasting;
using OpenTK;
using System.Runtime.Serialization;

namespace GraphicsTest.Behaviors.Enemy
{
    public class TurnTowardsNode : Node
    {
        public override BehaviorStatus Tick(BehaviorContext context)
        {
            if (context.Translation != Vector3.Zero)
            {
                float turnAngle = (float)Math.Atan2(context.Translation.Y, context.Translation.X);

                context.QRotation = new Quaternion(turnAngle, 0.0f, 0.0f);
                context.Rotation = new Vector3(turnAngle, context.Rotation.Y, context.Rotation.Z);
            }

            return BehaviorStatus.Success;
        }
    }
}
