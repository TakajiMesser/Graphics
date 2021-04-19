﻿using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Utilities;
using System;
using TangyHIDCore;
using UmamiScriptingCore;
using UmamiScriptingCore.Behaviors;
using UmamiScriptingCore.Behaviors.Nodes;

namespace SampleGameProject.Resources.Behaviors.Nodes
{
    public class TurnNode : Node
    {
        public override BehaviorStatus Tick(BehaviorContext context)
        {
            if (context.GetEntity() is IActor actor)
            {
                var inputProvider = context.SystemProvider.GetGameSystem<IInputProvider>();
                var nEvadeTicks = context.ContainsVariable("nEvadeTicks") ? context.GetVariable<int>("nEvadeTicks") : 0;

                // Compare current position to location of mouse, and set rotation to face the mouse
                if (!inputProvider.IsDown("ItemWheel") && nEvadeTicks == 0 && inputProvider.IsMouseInWindow)
                {
                    var camera = context.ActiveCamera;

                    if (camera == null) throw new Exception("Bananas");

                    var clipSpacePosition = (camera.CurrentModelMatrix * camera.CurrentProjectionMatrix).Inverted() * new Vector4(0.0f, 0.0f, 0.0f, 1.0f);//new Vector4(context.Entity.Position, 1.0f);
                    var screenCoordinates = new Vector2()
                    {
                        X = ((clipSpacePosition.X + 1.0f) / 2.0f) * inputProvider.WindowSize.Width,
                        Y = ((1.0f - clipSpacePosition.Y) / 2.0f) * inputProvider.WindowSize.Height,
                    };

                    var vectorBetween = inputProvider.MouseCoordinates - screenCoordinates;
                    float turnAngle = -(float)Math.Atan2(vectorBetween.Y, vectorBetween.X);

                    // Need to add the angle that the camera's Up vector is turned from Vector3.UnitY
                    // TODO - This is mad suspect...
                    var flattenedUp = context.ActiveCamera is Camera cameraInstance ? cameraInstance.Up.Xy : Vector2.One;
                    turnAngle += (float)Math.Atan2(flattenedUp.Y, flattenedUp.X) - MathExtensions.HALF_PI;

                    actor.Rotation = new Quaternion(0.0f, 0.0f, turnAngle);
                    context.EulerRotation = new Vector3(context.EulerRotation.X, context.EulerRotation.Y, turnAngle);
                }
            }

            return BehaviorStatus.Success;
        }

        public override void Reset() { }
    }
}
