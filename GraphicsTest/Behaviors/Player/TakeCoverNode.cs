using TakoEngine.Scripting.BehaviorTrees.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoEngine.Scripting.BehaviorTrees;
using TakoEngine.Physics.Raycasting;
using OpenTK;
using TakoEngine.Entities;
using System.Runtime.Serialization;

namespace GraphicsTest.Behaviors.Player
{
    [DataContract]
    public class TakeCoverNode : LeafNode
    {
        [DataMember]
        public float CoverSpeed { get; set; }

        [DataMember]
        public float EnterCoverSpeed { get; set; }

        [DataMember]
        public float CoverDistance { get; set; }

        public TakeCoverNode(float coverSpeed, float enterCoverSpeed, float coverDistance)
        {
            CoverSpeed = coverSpeed;
            EnterCoverSpeed = enterCoverSpeed;
            CoverDistance = coverDistance;
        }

        public override BehaviorStatuses Behavior(BehaviorContext context)
        {
            // TODO - Filter gameobjects and brushes based on "coverable" property
            var filteredColliders = context.Colliders.Where(c => c.AttachedEntity.GetType() == typeof(Brush));

            if (Raycast.TryCircleCast(new Circle(context.Position, CoverDistance), filteredColliders, out RaycastHit hit))
            {
                var vectorBetween = hit.Intersection - context.Position;
                context.SetVariable("coverDirection", vectorBetween.Xy);
                context.SetVariable("coverDistance", vectorBetween.Length);

                float turnAngle = (float)Math.Atan2(vectorBetween.Y, vectorBetween.X);
                context.QRotation = new Quaternion(turnAngle + (float)Math.PI, 0.0f, 0.0f);
                context.Rotation = new Vector3(turnAngle + (float)Math.PI, context.Rotation.Y, context.Rotation.Z);

                return BehaviorStatuses.Success;
            }

            return BehaviorStatuses.Failure;
        }
    }
}
