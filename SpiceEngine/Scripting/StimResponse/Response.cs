using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Entities.Actors;
using SpiceEngine.Physics;
using SpiceEngine.Physics.Raycasting;
using SpiceEngine.Physics.Shapes;
using SpiceEngine.Scripting.Behaviors;
using SpiceEngine.Scripting.Meters;
using SpiceEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

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
                var stimuliColliders = context.GetBodies().Where(b => context.HasStimuli(b.EntityID, Stimulus));

                if (TriggerOnContact && HasContactStimulus(context.Actor, context.Body.Shape, stimuliColliders, context.GetEntityProvider()))
                {
                    Triggered?.Invoke(this, new StimulusTriggeredEventArgs(Stimulus));
                }
                else if (TriggerOnProximity && HasProximityStimulus(context.Actor, stimuliColliders, context.GetEntityProvider()))
                {
                    Triggered?.Invoke(this, new StimulusTriggeredEventArgs(Stimulus));
                }
                /*else if (TriggerOnSight && HasSightStimulus(context.Actor, context.EulerRotation, stimuliColliders, context.ColliderBodies, context.EntityProvider))
                {
                    Triggered?.Invoke(this, new StimulusTriggeredEventArgs(Stimulus));
                }*/
            }
        }

        private bool HasContactStimulus(Actor actor, IShape shape, IEnumerable<Body> stimuliColliders, IEntityProvider entityProvider)
        {
            foreach (var collider in stimuliColliders)
            {
                var position = entityProvider.GetEntity(collider.EntityID).Position;

                if (Shape3D.Collides(actor.Position, (Shape3D)shape, position, (Shape3D)collider.Shape))
                {
                    return true;
                }
            }
            
            return false;
        }

        private bool HasProximityStimulus(Actor actor, IEnumerable<Body> stimuliColliders, IEntityProvider entityProvider)
        {
            foreach (var collider in stimuliColliders)
            {
                var distance = (actor.Position - entityProvider.GetEntity(collider.EntityID).Position).Length;
                if (distance <= Radius)
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasSightStimulus(Actor actor, Vector3 eulerRotation, IEnumerable<Body> stimuliColliders, IEnumerable<Body> colliders, IEntityProvider entityProvider)
        {
            foreach (var collider in stimuliColliders)
            {
                var stimulusDirection = (entityProvider.GetEntity(collider.EntityID).Position - actor.Position).Normalized();
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
                    var ray = new Ray3(actor.Position, stimulusDirection, (float)SightDistance);
                    if (Raycast.TryRaycast(ray, colliders, entityProvider, out RaycastHit hit))
                    {
                        if (hit.EntityID == collider.EntityID)
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
