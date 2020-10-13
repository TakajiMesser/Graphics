using SavoryPhysicsCore;
using SpiceEngineCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using UmamiScriptingCore.Behaviors;
using UmamiScriptingCore.Meters;
using UmamiScriptingCore.StimResponse;

namespace SpiceEngine.Scripting
{
    public class Response : IResponse
    {
        public IStimulus Stimulus { get; set; }

        public bool TriggerOnContact { get; set; }
        public bool TriggerOnProximity { get; set; }
        public bool TriggerOnSight { get; set; }

        /// <summary>
        /// The distance to check for proximity stimuli
        /// </summary>
        public double ProximityRadius { get; set; }

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
        /*public int CheckFrequency
        {
            get => _tickMeter.MinimumTriggerValue;
            set => _tickMeter.MinimumTriggerValue = value;
        }*/

        //[IgnoreDataMember]
        public event EventHandler<StimulusTriggeredEventArgs> Triggered;

        //[IgnoreDataMember]
        private Meter _tickMeter = new Meter();

        public Response(IStimulus stimulus)
        {
            Stimulus = stimulus;

            _tickMeter.AddTrigger(new Trigger("Response", 1)
            {
                ResetOnTrigger = true
            });

            //CheckFrequency = 1;
            //_tickMeter.ResetOnTrigger = true;
        }

        public virtual void Tick(BehaviorContext context)
        {
            if (_tickMeter.Increment())
            {
                // Filter colliders by those that are stimuli, and those that aren't
                var bodyProvider = context.SystemProvider.GetComponentProvider<IBody>();
                var physicsProvider = context.SystemProvider.GetGameSystem<IPhysicsProvider>();

                if (TriggerOnContact && HasContactStimulus(context, context.GetEntity(), physicsProvider))
                {
                    Triggered?.Invoke(this, new StimulusTriggeredEventArgs(Stimulus));
                }
                else
                {
                    var stimuliBodies = physicsProvider.GetCollisionIDs().Where(i => context.HasStimuli(i, Stimulus)).Select(i => physicsProvider.GetComponent(i));

                    if (TriggerOnProximity && HasProximityStimulus(context.GetEntity(), stimuliBodies, context.SystemProvider.EntityProvider))
                    {
                        Triggered?.Invoke(this, new StimulusTriggeredEventArgs(Stimulus));
                    }
                }
                /*else if (TriggerOnSight && HasSightStimulus(context.Actor, context.EulerRotation, stimuliColliders, context.ColliderBodies, context.EntityProvider))
                {
                    Triggered?.Invoke(this, new StimulusTriggeredEventArgs(Stimulus));
                }*/
            }
        }

        private bool HasContactStimulus(BehaviorContext context, IEntity entity, IPhysicsProvider physicsProvider)
        {
            foreach (var collisionID in physicsProvider.GetCollisionIDs(entity.ID))
            {
                if (context.HasStimuli(collisionID, Stimulus))
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasProximityStimulus(IEntity entity, IEnumerable<IBody> stimuliColliders, IEntityProvider entityProvider)
        {
            foreach (var collider in stimuliColliders)
            {
                var distance = (entity.Position - entityProvider.GetEntity(collider.EntityID).Position).Length;
                if (distance <= ProximityRadius)
                {
                    return true;
                }
            }

            return false;
        }

        /*private bool HasSightStimulus(IEntity entity, Vector3 eulerRotation, IEnumerable<IBody> stimuliColliders, IEnumerable<IBody> colliders, IEntityProvider entityProvider)
        {
            foreach (var collider in stimuliColliders)
            {
                var stimulusDirection = (entityProvider.GetEntity(collider.EntityID).Position - entity.Position).Normalized();
                //var stimulusQuaternion = Quaternion.FromEulerAngles(stimulusDirection);

                stimulusDirection.Z = 0.0f;
                //var actorDirection = actor.Rotation * Vector3.UnitX;
                var actorDirection = Quaternion.FromEulerAngles(eulerRotation.ToRadians()) * Vector3.UnitX;
                var angleDifference = actorDirection.AngleBetween(stimulusDirection);
                var angleDegrees = UnitConversions.ToDegrees(Math.Abs(angleDifference));
                //var angleDifference = eulerRotation.AngleBetween(stimulusDirection);

                //var angleDifference = actor.Rotation.AngleBetween(stimulusQuaternion);

                if (angleDegrees <= SightAngle / 2.0f)
                {
                    // Perform a raycast to see if any other colliders obstruct our view of the stimulus
                    // TODO - Filter colliders by their ability to obstruct vision
                    var ray = new Ray3(entity.Position, stimulusDirection, (float)SightDistance);
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
        }*/
    }
}
