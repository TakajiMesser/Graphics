using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoEngine.Scripting.Behaviors;
using TakoEngine.Physics.Raycasting;
using OpenTK;
using TakoEngine.Entities;
using System.Runtime.Serialization;
using TakoEngine.Physics.Collision;
using TakoEngine.Helpers;

namespace Jidai.Behaviors.Player
{
    public class CoverNode : Node
    {
        public float CoverSpeed { get; set; }
        public float EnterCoverSpeed { get; set; }
        public float CoverDistance { get; set; }

        public CoverNode(float coverSpeed, float enterCoverSpeed, float coverDistance)
        {
            CoverSpeed = coverSpeed;
            EnterCoverSpeed = enterCoverSpeed;
            CoverDistance = coverDistance;
        }

        public override BehaviorStatus Tick(BehaviorContext context)
        {
            if (context.ContainsVariable("coverDirection"))
            {
                if (context.ContainsVariable("coverDistance"))
                {
                    if (context.GetVariable<float>("coverDistance") > 0.0f)
                    {
                        context.SetVariable("coverDistance", context.GetVariable<float>("coverDistance") - EnterCoverSpeed);

                        var coverDirection = context.GetVariable<Vector2>("coverDirection");
                        context.Translation = new Vector3(coverDirection.X, coverDirection.Y, 0) * EnterCoverSpeed;
                    }

                    if (context.GetVariable<float>("coverDistance") < 0.1f)
                    {
                        // Handle movement while in cover here
                        var translation = GeometryHelper.GetHeldTranslation(context.Camera, CoverSpeed, context.InputState, context.InputMapping);

                        if (translation != Vector3.Zero)
                        {
                            var filteredColliders = context.Colliders.Where(c => c.AttachedEntity.GetType() == typeof(Brush));

                            // Calculate the furthest point along the bounds of our object, since we should attempt to raycast from there
                            var borderPoint = context.Actor.Bounds.GetBorder(translation);
                            
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

                return BehaviorStatus.Success;
            }

            return BehaviorStatus.Failure;
        }

        public override void Reset() { }
    }
}
