using OpenTK;
using System;
using System.Runtime.Serialization;
using TakoEngine.Scripting.Behaviors;
using TakoEngine.Scripting.Behaviors.Composites;
using TakoEngine.Scripting.Behaviors.Decorators;
using TakoEngine.Scripting.Behaviors.Leaves;

namespace Jidai.Behaviors.Player
{
    public class PlayerBehavior : Behavior
    {
        public const float WALK_SPEED = 0.1f;
        public const float RUN_SPEED = 0.15f;
        public const float CREEP_SPEED = 0.04f;
        public const float EVADE_SPEED = 0.175f;
        public const float COVER_SPEED = 0.1f;
        public const float ENTER_COVER_SPEED = 0.12f;
        public const float COVER_DISTANCE = 5.0f;
        public const int EVADE_TICK_COUNT = 20;

        public PlayerBehavior()
        {
            var rootNode = new SelectorNode(
                new SelectorNode(
                    new EvadeNode(EVADE_SPEED, EVADE_TICK_COUNT),
                    new SelectorNode(
                        new InlineConditionNode(c => c.InputState.IsPressed(c.InputMapping.Cover),
                            new TakeCoverNode(COVER_SPEED, ENTER_COVER_SPEED, COVER_DISTANCE)
                        ),
                        new InlineConditionNode(c => c.InputState.IsHeld(c.InputMapping.Cover),
                            new CoverNode(COVER_SPEED, ENTER_COVER_SPEED, COVER_DISTANCE)
                        ),
                        new InlineConditionNode(c => c.InputState.IsReleased(c.InputMapping.Cover),
                            new InlineLeafNode(c =>
                            {
                                c.RemoveVariableIfExists("coverDirection");
                                c.RemoveVariableIfExists("coverDistance");

                                return BehaviorStatus.Success;
                            })
                        )
                    )
                ),
                new ParallelNode(
                    new MoveNode(RUN_SPEED, CREEP_SPEED, WALK_SPEED),
                    new TurnNode()
                )
            );

            RootStack.Push(rootNode);
        }

        public override BehaviorStatus Tick()
        {
            // Check if player is in line-of-sight
            // If yes, increment the Alertness Property. If this Property rises above a threshold, fire the Alerted event.

            // Continue patrolling
            return base.Tick();
            //var root = RootStack.Peek();
            //return root.Tick(Context);

            //return BehaviorStatus.Success;
        }
    }
}
