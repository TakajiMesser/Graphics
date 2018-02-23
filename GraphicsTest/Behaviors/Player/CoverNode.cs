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
        [DataMember]
        public float CoverSpeed { get; set; }

        [DataMember]
        public float EnterCoverSpeed { get; set; }

        [DataMember]
        public float CoverDistance { get; set; }

        public CoverNode(float coverSpeed, float enterCoverSpeed, float coverDistance)
        {
            CoverSpeed = coverSpeed;
            EnterCoverSpeed = enterCoverSpeed;
            CoverDistance = coverDistance;
        }

        public override BehaviorStatuses Behavior(BehaviorContext context)
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
                        var translation = context.GetTranslation(CoverSpeed);

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

            return BehaviorStatuses.Failure;
        }
    }
}
