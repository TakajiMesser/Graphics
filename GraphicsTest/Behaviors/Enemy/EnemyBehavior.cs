using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoEngine.Scripting.Behaviors;
using TakoEngine.Scripting.Behaviors.Composites;
using TakoEngine.Scripting.Properties;

namespace GraphicsTest.Behaviors.Enemy
{
    public class EnemyBehavior : Behavior, IAlertProperty
    {
        public const float WALK_SPEED = 0.1f;
        public const float VIEW_ANGLE = 1.0472f;
        public const float VIEW_DISTANCE = 5.0f;
        public const int FULL_ALERT_TICKS = 120;
        
        public int Alertness
        {
            get => _alertness;
            set
            {
                _alertness = value;

                if (_alertness > 100)
                {
                    Alerted?.Invoke(this, new BehaviorInterruptedEventArgs(new ChaseNode(VIEW_ANGLE, VIEW_DISTANCE, "Player")));
                }
            }
        }
        public event EventHandler<BehaviorInterruptedEventArgs> Alerted;

        private int _alertness = 0;

        public EnemyBehavior()
        {
            var rootNode = new SelectorNode(
                /*new RepeaterNode(
                    new IsPlayerInSightNode(VIEW_ANGLE, VIEW_DISTANCE,
                        new IsAtFullAlert(FULL_ALERT_TICKS,
                            // TODO - Chase Player
                            new InlineLeafNode(c => BehaviorStatus.Success)
                        )
                    )
                ),*/
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

            RootStack.Push(rootNode);

            Alerted += EnemyBehavior_Alerted;
        }

        private void EnemyBehavior_Alerted(object sender, BehaviorInterruptedEventArgs e)
        {
            // When alerted, replace the root node with another node, in order to "interrupt" our current action
            // HOWEVER, we need to keep a reference to the node that we WERE on before the interruption so that we can gracefully return
            RootStack.Push(e.NewRootNode);
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
