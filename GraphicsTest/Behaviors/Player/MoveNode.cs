using Graphics.Scripting.BehaviorTrees.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphics.Scripting.BehaviorTrees;
using Graphics.Physics.Raycasting;
using OpenTK;
using Graphics.GameObjects;
using System.Runtime.Serialization;

namespace GraphicsTest.Behaviors.Player
{
    [DataContract]
    public class MoveNode : LeafNode
    {
        public override BehaviorStatuses Behavior(BehaviorContext context)
        {
            var runSpeed = context.GetProperty<float>("RUN_SPEED");
            var creepSpeed = context.GetProperty<float>("CREEP_SPEED");
            var walkSpeed = context.GetProperty<float>("WALK_SPEED");

            var speed = context.InputState.IsHeld(context.InputMapping.Run)
                ? runSpeed
                : context.InputState.IsHeld(context.InputMapping.Crawl)
                    ? creepSpeed
                    : walkSpeed;

            var translation = context.GetTranslation(speed);

            if (context.InputState.IsHeld(context.InputMapping.In))
            {
                translation.Z += speed;
            }

            if (context.InputState.IsHeld(context.InputMapping.Out))
            {
                translation.Z -= speed;
            }

            if (context.InputState.IsHeld(context.InputMapping.ItemSlot1))
            {
                context.Rotation = new Vector3(context.Rotation.X, context.Rotation.Y + 0.1f, context.Rotation.Z);
            }

            if (context.InputState.IsHeld(context.InputMapping.ItemSlot2))
            {
                context.Rotation = new Vector3(context.Rotation.X, context.Rotation.Y - 0.1f, context.Rotation.Z);
            }

            if (context.InputState.IsHeld(context.InputMapping.ItemSlot3))
            {
                context.Rotation = new Vector3(context.Rotation.X, context.Rotation.Y, context.Rotation.Z + 0.1f);
            }

            if (context.InputState.IsHeld(context.InputMapping.ItemSlot4))
            {
                context.Rotation = new Vector3(context.Rotation.X, context.Rotation.Y, context.Rotation.Z - 0.1f);
            }

            context.Translation = translation;
            return BehaviorStatuses.Success;
        }
    }
}
