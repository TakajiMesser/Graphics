using OpenTK;
using SpiceEngine.Entities.Brushes;
using SpiceEngine.Physics.Raycasting;
using SpiceEngine.Scripting;
using SpiceEngine.Scripting.Nodes;
using SpiceEngine.Utilities;
using System;
using System.Linq;

namespace SampleGameProject.Behaviors.Player
{
    public class TakeCoverNode : Node
    {
        public float CoverSpeed { get; set; }
        public float EnterCoverSpeed { get; set; }
        public float CoverDistance { get; set; }

        public TakeCoverNode(float coverSpeed, float enterCoverSpeed, float coverDistance)
        {
            CoverSpeed = coverSpeed;
            EnterCoverSpeed = enterCoverSpeed;
            CoverDistance = coverDistance;
        }

        public override BehaviorStatus Tick(BehaviorContext context)
        {
            // TODO - Filter gameobjects and brushes based on "coverable" property
            var filteredColliders = context.GetColliderBodies().Where(c => context.GetEntity(c.EntityID) is Brush);

            if (Raycast.TryCircleCast(new RayCircle(context.Position, CoverDistance), filteredColliders, context.GetEntityProvider(), out RaycastHit hit))
            {
                var vectorBetween = hit.Intersection - context.Position;
                context.SetVariable("coverDirection", vectorBetween.Xy);
                context.SetVariable("coverDistance", vectorBetween.Length);

                float turnAngle = (float)Math.Atan2(vectorBetween.Y, vectorBetween.X);
                context.Actor.Rotation = new Quaternion(0.0f, 0.0f, turnAngle + MathExtensions.PI);
                context.EulerRotation = new Vector3(0.0f, context.EulerRotation.Y, turnAngle + MathExtensions.PI);

                return BehaviorStatus.Success;
            }

            return BehaviorStatus.Failure;
        }

        public override void Reset() { }
    }
}
