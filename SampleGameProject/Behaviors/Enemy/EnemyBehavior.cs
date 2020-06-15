using UmamiScriptingCore.Behaviors;
using UmamiScriptingCore.Meters;
using UmamiScriptingCore.StimResponse;

namespace SampleGameProject.Behaviors.Enemy
{
    public class EnemyBehavior : Behavior
    {
        public const float WALK_SPEED = 0.1f;
        public const float CHASE_SPEED = 0.15f;
        public const float VIEW_ANGLE = 1.0472f;
        public const float VIEW_DISTANCE = 5.0f;
        public const int FULL_ALERT_TICKS = 120;
        
        private Meter _alertMeter = new Meter();

        public EnemyBehavior(int entityID) : base(entityID)
        {
            _alertMeter.AddTrigger(new Trigger("Alert", 120)
            {
                ResetOnTrigger = false
            });
            _alertMeter.Triggered += AlertMeter_Triggered;
        }

        /*protected override void SetRootNodes()
        {
            var rootNode = new RepeaterNode(
                new SequenceNode(
                    new ParallelNode(
                        new MoveToNode(new Vector3(5.0f, 5.0f, -0.5f), WALK_SPEED),
                        new TurnTowardsNode()
                    ),
                    new ParallelNode(
                        new MoveToNode(new Vector3(5.0f, -5.0f, -0.5f), WALK_SPEED),
                        new TurnTowardsNode()
                    ),
                    new ParallelNode(
                        new MoveToNode(new Vector3(-5.0f, -5.0f, -0.5f), WALK_SPEED),
                        new TurnTowardsNode()
                    ),
                    new ParallelNode(
                        new MoveToNode(new Vector3(-5.0f, 5.0f, -0.5f), WALK_SPEED),
                        new TurnTowardsNode()
                    )
                )
            );

            RootStack.Push(rootNode);
        }

        protected override void SetResponses()
        {
            var playerResponse = new Response(Stimulus.Player)
            {
                TriggerOnContact = true,
                TriggerOnSight = true,
                SightAngle = 50.0f,
                SightDistance = 20.0f
            };
            playerResponse.Triggered += PlayerResponse_Triggered;
            Responses.Add(playerResponse);
        }*/

        private void AlertMeter_Triggered(object sender, TriggeredEventArgs e)
        {
            //RootStack.Push(new ChaseNode(CHASE_SPEED, VIEW_ANGLE, VIEW_DISTANCE, "Player"));
        }

        private void PlayerResponse_Triggered(object sender, StimulusTriggeredEventArgs e)
        {
            if (_alertMeter.Value < 120)
            {
                _alertMeter.Increment();
            }
        }
    }
}
