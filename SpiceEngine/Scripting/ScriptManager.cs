using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Entities.Actors;
using SpiceEngine.Entities.Cameras;
using SpiceEngine.Inputs;
using SpiceEngine.Physics;
using SpiceEngine.Scripting.Behaviors;
using SpiceEngine.Scripting.Properties;
using SpiceEngine.Scripting.StimResponse;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Scripting
{
    /// <summary>
    /// A script is a set of sequential commands
    /// Each command can be something this object needs to communicate to another object, or performed by itself
    /// Each command either needs to be associated with 
    /// </summary>
    public class ScriptManager : IStimulusProvider
    {
        private IEntityProvider _entityProvider;
        private ICollisionProvider _collisionProvider;

        private Dictionary<int, Behavior> _behaviorsByEntityID = new Dictionary<int, Behavior>();
        private Dictionary<int, PropertyCollection> _propertiesByEntityID = new Dictionary<int, PropertyCollection>();
        private Dictionary<int, StimulusCollection> _stimuliByEntityID = new Dictionary<int, StimulusCollection>();

        public ScriptManager(IEntityProvider entityProvider, ICollisionProvider collisionProvider)
        {
            _entityProvider = entityProvider;
            _collisionProvider = collisionProvider;
        }

        public IEnumerable<Stimulus> GetStimuli(int entityID) => _stimuliByEntityID.ContainsKey(entityID)
            ? _stimuliByEntityID[entityID].Stimuli
            : Enumerable.Empty<Stimulus>();

        public void AddBehavior(int entityID, Behavior behavior)
        {
            _behaviorsByEntityID.Add(entityID, behavior);
        }

        public void AddProperties(int entityID, IEnumerable<Property> properties)
        {
            var propertyCollection = new PropertyCollection();
            propertyCollection.AddProperties(properties);

            _propertiesByEntityID.Add(entityID, propertyCollection);
        }

        public void AddStimuli(int entityID, IEnumerable<Stimulus> stimuli)
        {
            var stimulusCollection = new StimulusCollection();
            stimulusCollection.AddStimuli(stimuli);

            _stimuliByEntityID.Add(entityID, stimulusCollection);
        }

        public void Load()
        {
            foreach (var actor in _entityProvider.Actors)
            {
                if (_behaviorsByEntityID.ContainsKey(actor.ID))
                {
                    var behavior = _behaviorsByEntityID[actor.ID];
                    behavior.Context.Actor = actor;

                    //Behaviors.Context.Rotation = Rotation;
                    behavior.Context.SetEntityProvider(_entityProvider);
                    behavior.Context.SetCollisionProvider(_collisionProvider);
                    behavior.Context.SetStimulusProvider(this);

                    /*foreach (var property in Properties)
                    {
                        if (property.Value.IsConstant)
                        {
                            Behaviors.Context.AddProperty(property.Key, property.Value.Value);
                        }
                    }*/
                }
            }
        }

        /*public virtual void OnHandleInput(InputManager inputManager, Camera camera)
        {
            if (Behaviors != null)
            {
                Behaviors.Context.InputManager = inputManager;
                Behaviors.Context.InputMapping = InputMapping;
                Behaviors.Context.Camera = camera;
            }
        }*/

        public InputBinding InputMapping { get; set; } = new InputBinding();

        public void HandleInput(InputManager inputManager, Camera camera)
        {
            foreach (var actor in _entityProvider.Actors)
            {
                if (_behaviorsByEntityID.ContainsKey(actor.ID))
                {
                    var behavior = _behaviorsByEntityID[actor.ID];

                    behavior.Context.InputManager = inputManager;
                    behavior.Context.InputMapping = InputMapping;
                    behavior.Context.Camera = camera;
                }
            }
        }

        /*public void UpdateCollisions(IEnumerable<EntityCollision> entityCollisions)
        {
            foreach (var entityCollision in entityCollisions)
            {
                if (_behaviorsByEntityID.ContainsKey(entityCollision.EntityID))
                {
                    var behavior = _behaviorsByEntityID[entityCollision.EntityID];

                    behavior.Context.ActorShape = entityCollision.Shape;
                    //behavior.Context.ActorBounds = entityCollision.Bounds;
                    //behavior.Context.ColliderBounds = entityCollision.Colliders;
                    behavior.Context.ColliderBodies = entityCollision.Bodies;
                }
            }
        }*/

        public void Update()
        {
            foreach (var actor in _entityProvider.Actors)
            {
                if (_behaviorsByEntityID.ContainsKey(actor.ID))
                {
                    var behavior = _behaviorsByEntityID[actor.ID];

                    foreach (var property in _propertiesByEntityID[actor.ID].VariableProperties)
                    {
                        behavior.Context.SetProperty(property.Name, property);
                    }

                    behavior.Tick();

                    if (actor is AnimatedActor animatedActor)
                    {
                        animatedActor.UpdateAnimation();
                    }
                }
            }
        }
    }
}
