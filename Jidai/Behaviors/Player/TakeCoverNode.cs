using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiceEngine.Scripting.Behaviors;
using SpiceEngine.Physics.Raycasting;
using OpenTK;
using SpiceEngine.Entities;
using System.Runtime.Serialization;
using SpiceEngine.Entities.Brushes;

namespace Jidai.Behaviors.Player
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
            var filteredColliders = context.CollisionProvider.GetCollisions(context.Actor.ID)
                .Select(c => context.CollisionProvider.GetBody(c))
                .Where(c => context.EntityProvider.GetEntity(c.EntityID) is Brush);

            if (Raycast.TryCircleCast(new RayCircle(context.Actor.Position, CoverDistance), filteredColliders, context.EntityProvider, out RaycastHit hit))
            {
                var vectorBetween = hit.Intersection - context.Actor.Position;
                context.SetVariable("coverDirection", vectorBetween.Xy);
                context.SetVariable("coverDistance", vectorBetween.Length);

                float turnAngle = (float)Math.Atan2(vectorBetween.Y, vectorBetween.X);
                context.Actor.Rotation = new Quaternion(turnAngle + (float)Math.PI, 0.0f, 0.0f);
                context.EulerRotation = new Vector3(turnAngle + (float)Math.PI, context.EulerRotation.Y, context.EulerRotation.Z);

                return BehaviorStatus.Success;
            }

            return BehaviorStatus.Failure;
        }

        public override void Reset() { }
    }
}
