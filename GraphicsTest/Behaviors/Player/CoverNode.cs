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
    public class CoverNode : LeafNode
    {
        public override BehaviorStatuses Behavior(BehaviorContext context)
        {
            var enterCoverSpeed = context.GetProperty<float>("ENTER_COVER_SPEED");
            var coverSpeed = context.GetProperty<float>("COVER_SPEED");
            var coverDistance = context.GetProperty<float>("COVER_DISTANCE");

            if (context.InputState.IsPressed(context.InputMapping.Cover))
            {
                // TODO - Filter gameobjects and brushes based on "coverable" property
                var filteredColliders = context.Colliders.Where(c => c.AttachedObject.GetType() == typeof(Brush));

                if (Raycast.TryCircleCast(new Circle(context.Position, coverDistance), filteredColliders, out RaycastHit hit))
                {
                    var vectorBetween = hit.Intersection - context.Position;
                    context.SetVariable("coverDirection", vectorBetween.Xy);
                    context.SetVariable("coverDistance", vectorBetween.Length);

                    float turnAngle = (float)Math.Atan2(vectorBetween.Y, vectorBetween.X);
                    context.QRotation = new Quaternion(turnAngle + (float)Math.PI, 0.0f, 0.0f);
                    context.Rotation = new Vector3(turnAngle + (float)Math.PI, context.Rotation.Y, context.Rotation.Z);

                    return BehaviorStatuses.Success;
                }
            }
            else if (context.InputState.IsHeld(context.InputMapping.Cover))
            {
                if (context.ContainsVariable("coverDirection"))
                {
                    if (context.ContainsVariable("coverDistance"))
                    {
                        if (context.GetVariable<float>("coverDistance") > 0.0f)
                        {
                            context.SetVariable("coverDistance", context.GetVariable<float>("coverDistance") - enterCoverSpeed);

                            var coverDirection = context.GetVariable<Vector2>("coverDirection");
                            context.Translation = new Vector3(coverDirection.X, coverDirection.Y, 0) * enterCoverSpeed;
                        }

                        if (context.GetVariable<float>("coverDistance") < 0.1f)
                        {
                            // Handle movement while in cover here
                            var translation = context.GetTranslation(coverSpeed);

                            if (translation != Vector3.Zero)
                            {
                                var filteredColliders = context.Colliders.Where(c => c.AttachedObject.GetType() == typeof(Brush));

                                // Calculate the furthest point along the bounds of our object, since we should attempt to raycast from there
                                var borderPoint = context.Bounds.GetBorder(translation);

                                // TODO - Dynamically determine how far the raycast should be
                                var boundWidth = 2.0f;

                                var coverDirection = context.GetVariable<Vector2>("coverDirection");
                                if (Raycast.TryRaycast(new Ray3(borderPoint, new Vector3(coverDirection.X, coverDirection.Y, 0.0f), boundWidth), filteredColliders, out RaycastHit hit))
                                {
                                    var vectorBetween = hit.Intersection - borderPoint;

                                    translation.X += vectorBetween.X;
                                    translation.Y += vectorBetween.Y;

                                    context.Translation = translation;
                                }
                            }
                        }
                    }

                    return BehaviorStatuses.Success;
                }
            }
            else if (context.InputState.IsReleased(context.InputMapping.Cover))
            {
                context.RemoveVariableIfExists("coverDirection");
                context.RemoveVariableIfExists("coverDistance");

                return BehaviorStatuses.Success;
            }

            return BehaviorStatuses.Failure;
        }
    }
}
