using SpiceEngine.Entities;
using SpiceEngine.Entities.Cameras;
using SpiceEngine.Game;
using SpiceEngine.Inputs;
using SpiceEngine.Physics;
using SpiceEngine.Scripting.Nodes;
using SpiceEngine.Scripting.Properties;
using SpiceEngine.Scripting.StimResponse;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Scripting
{
    public class BehaviorManager : UpdateManager, IStimulusProvider
    {
        private bool _isLoaded = false;

        private Camera _camera;
        private IEntityProvider _entityProvider;
        private ICollisionProvider _collisionProvider;
        private IInputProvider _inputProvider;

        private Dictionary<int, Behavior> _behaviorsByEntityID = new Dictionary<int, Behavior>();
        private Dictionary<int, PropertyCollection> _propertiesByEntityID = new Dictionary<int, PropertyCollection>();
        private Dictionary<int, StimulusCollection> _stimuliByEntityID = new Dictionary<int, StimulusCollection>();

        public BehaviorManager(IEntityProvider entityProvider, ICollisionProvider collisionProvider)
        {
            _entityProvider = entityProvider;
            _collisionProvider = collisionProvider;
        }

        public void SetCamera(Camera camera)
        {
            _camera = camera;

            if (_isLoaded)
            {
                foreach (var actor in _entityProvider.Actors)
                {
                    if (_behaviorsByEntityID.ContainsKey(actor.ID))
                    {
                        var behavior = _behaviorsByEntityID[actor.ID];
                        behavior.Context.Camera = _camera;
                    }
                }
            }
        }

        public void SetInputProvider(IInputProvider inputProvider)
        {
            _inputProvider = inputProvider;

            if (_isLoaded)
            {
                foreach (var actor in _entityProvider.Actors)
                {
                    if (_behaviorsByEntityID.ContainsKey(actor.ID))
                    {
                        var behavior = _behaviorsByEntityID[actor.ID];
                        behavior.Context.SetInputProvider(inputProvider);
                    }
                }
            }
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
                    behavior.Context.Camera = _camera;
                    behavior.Context.SetEntityProvider(_entityProvider);
                    behavior.Context.SetCollisionProvider(_collisionProvider);
                    behavior.Context.SetInputProvider(_inputProvider);
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

            _isLoaded = true;
        }

        protected override void Update()
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
                }
            }
        }
    }
}
