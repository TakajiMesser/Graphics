using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoEngine.Scripting.Behaviors;
using TakoEngine.Scripting.Behaviors.Composites;
using TakoEngine.Scripting.Behaviors.Decorators;
using TakoEngine.Scripting.Meters;
using TakoEngine.Scripting.Properties;
using TakoEngine.Scripting.StimResponse;

namespace Jidai.Behaviors.Enemy
{
    public class EnemyBehavior : Behavior
    {
        public const float WALK_SPEED = 0.1f;
        public const float CHASE_SPEED = 0.15f;
        public const float VIEW_ANGLE = 1.0472f;
        public const float VIEW_DISTANCE = 5.0f;
        public const int FULL_ALERT_TICKS = 120;
        
        private Meter _alertMeter;

        public EnemyBehavior()
        {
            var rootNode = new RepeaterNode(
                new SequenceNode(
                    new ParallelNode(
                        new MoveToNode(new Vector3(5.0f, 5.0f, -1.5f), WALK_SPEED),
                        new TurnTowardsNode()
                    ),
                    new ParallelNode(
                        new MoveToNode(new Vector3(5.0f, -5.0f, -1.5f), WALK_SPEED),
                        new TurnTowardsNode()
                    ),
                    new ParallelNode(
                        new MoveToNode(new Vector3(-5.0f, -5.0f, -1.5f), WALK_SPEED),
                        new TurnTowardsNode()
                    ),
                    new ParallelNode(
                        new MoveToNode(new Vector3(-5.0f, 5.0f, -1.5f), WALK_SPEED),
                        new TurnTowardsNode()
                    )
                )
            );

            RootStack.Push(rootNode);

            _alertMeter = new Meter()
            {
                ResetOnTrigger = false,
                TriggerValue = 120
            };
            _alertMeter.Triggered += AlertMeter_Triggered;

            var response = new Response(Stimulus.Player)
            {
                TriggerOnContact = true,
                TriggerOnSight = true,
                SightAngle = 50.0f,
                SightDistance = 20.0f
            };
            response.Triggered += Response_Triggered;
            Responses.Add(response);
        }

        private void AlertMeter_Triggered(object sender, MeterTriggeredEventArgs e)
        {
            var newRoot = new ChaseNode(CHASE_SPEED, VIEW_ANGLE, VIEW_DISTANCE, "Player");
            RootStack.Push(newRoot);
        }

        private void Response_Triggered(object sender, StimulusTriggeredEventArgs e)
        {
            if (_alertMeter.Value < _alertMeter.TriggerValue)
            {
                _alertMeter.Increment();
            }
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
