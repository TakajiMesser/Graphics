using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiceEngine.Scripting.Behaviors;
using SpiceEngine.Entities;
using SpiceEngine.Physics.Raycasting;
using OpenTK;
using System.Runtime.Serialization;
using SpiceEngine.Utilities;

namespace Jidai.Behaviors.Enemy
{
    public class TurnTowardsNode : Node
    {
        public override BehaviorStatus Tick(BehaviorContext context)
        {
            if (context.Translation != Vector3.Zero)
            {
                float turnAngle = (float)Math.Atan2(context.Translation.Y, context.Translation.X);

                context.Actor.Rotation = new Quaternion(turnAngle, 0.0f, 0.0f);
                context.EulerRotation = new Vector3(turnAngle, context.EulerRotation.Y, context.EulerRotation.Z);
            }

            return BehaviorStatus.Success;
        }

        public override void Reset() { }
    }
}
