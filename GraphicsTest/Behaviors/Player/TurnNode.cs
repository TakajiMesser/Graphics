using TakoEngine.Scripting.BehaviorTrees.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoEngine.Scripting.BehaviorTrees;
using TakoEngine.Physics.Raycasting;
using OpenTK;
using TakoEngine.GameObjects;
using System.Runtime.Serialization;

namespace GraphicsTest.Behaviors.Player
{
    [DataContract]
    public class TurnNode : LeafNode
    {
        public override BehaviorStatuses Behavior(BehaviorContext context)
        {
            var nEvadeTicks = context.ContainsVariable("nEvadeTicks") ? context.GetVariable<int>("nEvadeTicks") : 0;

            // Compare current position to location of mouse, and set rotation to face the mouse
            if (!context.InputState.IsHeld(context.InputMapping.ItemWheel) && nEvadeTicks == 0 && context.InputState.IsMouseInWindow)
            {
                var clipSpacePosition = context.Camera.ViewProjectionMatrix.Inverted() * new Vector4(0.0f, 0.0f, 0.0f, 1.0f);//new Vector4(context.Position, 1.0f);
                var screenCoordinates = new Vector2()
                {
                    X = ((clipSpacePosition.X + 1.0f) / 2.0f) * context.InputState.WindowWidth,
                    Y = ((1.0f - clipSpacePosition.Y) / 2.0f) * context.InputState.WindowHeight,
                };

                var vectorBetween = context.InputState.MouseCoordinates - screenCoordinates;
                float turnAngle = -(float)Math.Atan2(vectorBetween.Y, vectorBetween.X);

                // Need to add the angle that the camera's Up vector is turned from Vector3.UnitY
                turnAngle += (float)Math.Atan2(context.Camera._viewMatrix.Up.Y, context.Camera._viewMatrix.Up.X) - (float)Math.PI / 2.0f;

                context.QRotation = new Quaternion(turnAngle, 0.0f, 0.0f);
                context.Rotation = new Vector3(turnAngle, context.Rotation.Y, context.Rotation.Z);
            }

            return BehaviorStatuses.Success;
        }
    }
}
