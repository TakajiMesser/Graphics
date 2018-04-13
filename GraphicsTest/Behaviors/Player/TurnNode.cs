using OpenTK;
using System;
using System.Runtime.Serialization;
using TakoEngine.Scripting.Behaviors;

namespace GraphicsTest.Behaviors.Player
{
    public class TurnNode : Node
    {
        public override BehaviorStatus Tick(BehaviorContext context)
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

                context.Actor.Rotation = new Quaternion(turnAngle, 0.0f, 0.0f);
                context.EulerRotation = new Vector3(turnAngle, context.EulerRotation.Y, context.EulerRotation.Z);
            }

            return BehaviorStatus.Success;
        }
    }
}
