using Graphics.Maps;
using Graphics.Scripting.BehaviorTrees;
using Graphics.Scripting.BehaviorTrees.Composites;
using Graphics.Scripting.BehaviorTrees.Decorators;
using Graphics.Scripting.BehaviorTrees.Leaves;
using GraphicsTest.Behaviors;
using GraphicsTest.Behaviors.Enemy;
using GraphicsTest.Helpers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsTest.GameObjects
{
    public class Enemy : MapGameObject
    {
        public const string NAME = "Enemy";

        public const float WALK_SPEED = 0.1f;
        public const float VIEW_ANGLE = 1.0472f;
        public const float VIEW_DISTANCE = 5.0f;

        public Enemy()
        {
            Name = NAME;
            Position = new Vector3(5.0f, 5.0f, -1.0f);
            Scale = Vector3.One;
            Rotation = Quaternion.Identity;
            ModelFilePath = FilePathHelper.ENEMY_MESH_PATH;
            BehaviorFilePath = FilePathHelper.ENEMY_PATROL_BEHAVIOR_PATH;

            SaveBehaviorTree();
        }

        private void SaveBehaviorTree()
        {
            var rootNode = new SelectorNode(
                new RepeaterNode(
                    new SequenceNode(
                        new PlayerInSightNode(VIEW_ANGLE, VIEW_DISTANCE,
                            new InlineNode(c => BehaviorStatuses.Success)
                        )
                    /*new LeafNode()
                    {
                        Behavior = (c) =>
                        {
                            // Check if we are at full alertness
                            return BehaviorStatuses.Success;
                        }
                    },
                    new LeafNode()
                    {
                        Behavior = (c) =>
                        {
                            // TODO - Chase Player. For now, this will return success and cause the AI to stop
                            return BehaviorStatuses.Success;
                        }
                    }*/
                    )
                ),
                new SequenceNode(
                    new ParallelNode(
                        new MoveToNode(new Vector3(5.0f, 5.0f, -1.0f), WALK_SPEED),
                        new TurnTowardsNode()
                    ),
                    new ParallelNode(
                        new MoveToNode(new Vector3(5.0f, -5.0f, -1.0f), WALK_SPEED),
                        new TurnTowardsNode()
                    ),
                    new ParallelNode(
                        new MoveToNode(new Vector3(-5.0f, -5.0f, -1.0f), WALK_SPEED),
                        new TurnTowardsNode()
                    ),
                    new ParallelNode(
                        new MoveToNode(new Vector3(-5.0f, 5.0f, -1.0f), WALK_SPEED),
                        new TurnTowardsNode()
                    )
                )
            );

            rootNode.Save(FilePathHelper.ENEMY_PATROL_BEHAVIOR_PATH);
        }
    }
}
