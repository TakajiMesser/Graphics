using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiceEngine.Entities;
using SpiceEngine.Physics.Collision;
using SpiceEngine.Physics.Raycasting;
using SpiceEngine.Scripting.Behaviors;
using SpiceEngine.Scripting.Meters;
using SpiceEngine.Utilities;

namespace SpiceEngine.Scripting.StimResponse
{
    public class Response
    {
        public Stimulus Stimulus { get; private set; }

        public bool TriggerOnContact { get; set; }
        public bool TriggerOnProximity { get; set; }
        public bool TriggerOnSight { get; set; }

        /// <summary>
        /// The distance to check for proximity stimuli
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// The distance to check for sight stimuli
        /// </summary>
        public double SightDistance { get; set; }

        /// <summary>
        /// The angle to check for sight stimuli
        /// </summary>
        public double SightAngle { get; set; }

        /// <summary>
        /// How often to check for a received stimulus (in ticks)
        /// </summary>
        public int CheckFrequency
        {
            get => _tickMeter.TriggerValue;
            set => _tickMeter.TriggerValue = value;
        }

        public event EventHandler<StimulusTriggeredEventArgs> Triggered;

        private Meter _tickMeter = new Meter();

        public Response(Stimulus stimulus)
        {
            Stimulus = stimulus;
            CheckFrequency = 1;
            //_tickMeter.ResetOnTrigger = true;
        }

        public virtual void Tick(BehaviorContext context)
        {
            _tickMeter.Increment();

            if (_tickMeter.IsTriggered)
            {
                _tickMeter.Reset();

                // Filter colliders by those that are stimuli, and those that aren't
                var stimuliColliders = new List<Bounds>();

                foreach (var collider in context.Colliders)
                {
                    if (collider.AttachedEntity is IStimulate stimulator && stimulator.Stimuli.Contains(Stimulus))
                    {
                        stimuliColliders.Add(collider);
                    }
                }

                if (TriggerOnContact && HasContactStimulus(context.Actor, stimuliColliders))
                {
                    Triggered?.Invoke(this, new StimulusTriggeredEventArgs(Stimulus));
                }
                else if (TriggerOnProximity && HasProximityStimulus(context.Actor, stimuliColliders))
                {
                    Triggered?.Invoke(this, new StimulusTriggeredEventArgs(Stimulus));
                }
                else if (TriggerOnSight && HasSightStimulus(context.Actor, context.EulerRotation, stimuliColliders, context.Colliders))
                {
                    Triggered?.Invoke(this, new StimulusTriggeredEventArgs(Stimulus));
                }
            }
        }

        private bool HasContactStimulus(Actor actor, IEnumerable<Bounds> stimuliColliders)
        {
            foreach (var collider in stimuliColliders)
            {
                if (actor.Bounds.CollidesWith(collider))
                {
                    return true;
                }
            }
            
            return false;
        }

        private bool HasProximityStimulus(Actor actor, IEnumerable<Bounds> stimuliColliders)
        {
            foreach (var collider in stimuliColliders)
            {
                var distance = (actor.Bounds.Center - collider.Center).Length;
                if (distance <= Radius)
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasSightStimulus(Actor actor, Vector3 eulerRotation, IEnumerable<Bounds> stimuliColliders, IEnumerable<Bounds> colliders)
        {
            foreach (var collider in stimuliColliders)
            {
                var stimulusDirection = (collider.Center - actor.Bounds.Center).Normalized();
                //var stimulusQuaternion = Quaternion.FromEulerAngles(stimulusDirection);

                stimulusDirection.Z = 0.0f;
                //var actorDirection = actor.Rotation * Vector3.UnitX;
                var actorDirection = Quaternion.FromEulerAngles(eulerRotation) * Vector3.UnitX;
                var angleDifference = actorDirection.AngleBetween(stimulusDirection);
                var angleDegrees = UnitConversions.ToDegrees(Math.Abs(angleDifference));
                //var angleDifference = eulerRotation.AngleBetween(stimulusDirection);

                //var angleDifference = actor.Rotation.AngleBetween(stimulusQuaternion);

                if (angleDegrees <= SightAngle / 2.0f)
                {
                    // Perform a raycast to see if any other colliders obstruct our view of the stimulus
                    // TODO - Filter colliders by their ability to obstruct vision
                    if (Raycast.TryRaycast(new Ray3(actor.Position, stimulusDirection, (float)SightDistance), colliders, out RaycastHit hit))
                    {
                        if (hit.Collider == collider)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
