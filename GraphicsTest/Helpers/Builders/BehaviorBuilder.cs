using Graphics.GameObjects;
using Graphics.Physics.Raycasting;
using Graphics.Scripting.BehaviorTrees;
using Graphics.Scripting.BehaviorTrees.Composites;
using Graphics.Scripting.BehaviorTrees.Decorators;
using Graphics.Scripting.BehaviorTrees.Leaves;
using Graphics.Utilities;
using GraphicsTest.Behaviors;
using GraphicsTest.Behaviors.Enemy;
using GraphicsTest.Behaviors.Player;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsTest.Helpers.Builders
{
    public static class BehaviorBuilder
    {
        public static void CreateTestEnemyBehavior()
        {
            var rootNode = new SelectorNode(
                new RepeaterNode(
                    new SequenceNode(
                        new PlayerInSightNode()
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
                        new MoveToNode(new Vector3(5.0f, 5.0f, -1.0f), 0.1f),
                        new TurnTowardsNode()
                    ),
                    new ParallelNode(
                        new MoveToNode(new Vector3(5.0f, -5.0f, -1.0f), 0.1f),
                        new TurnTowardsNode()
                    ),
                    new ParallelNode(
                        new MoveToNode(new Vector3(-5.0f, -5.0f, -1.0f), 0.1f),
                        new TurnTowardsNode()
                    ),
                    new ParallelNode(
                        new MoveToNode(new Vector3(-5.0f, 5.0f, -1.0f), 0.1f),
                        new TurnTowardsNode()
                    )
                )
            );

            rootNode.Save(FilePathHelper.ENEMY_PATROL_BEHAVIOR_PATH);
        }

        public static void CreateTestPlayerBehavior()
        {
            var rootNode = new SelectorNode(
                new SelectorNode(
                    new SelectorNode(
                        new EvadeNode(),
                        new CoverNode()
                    ),
                    new SequenceNode(
                        new MoveNode(),
                        new TurnNode()
                    )
                )
            );

            rootNode.Save(FilePathHelper.PLAYER_INPUT_BEHAVIOR_PATH);
        }
    }
}
