using SpiceEngine.Scripting.Scripts;
using SpiceEngineCore.Components.Builders;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Game;
using SpiceEngineCore.Inputs;
using SpiceEngineCore.Physics;
using SpiceEngineCore.Scripting;
using SpiceEngineCore.UserInterfaces;
using System.Collections.Generic;
using System.Linq;
using UmamiScriptingCore.Behaviors.Properties;
using UmamiScriptingCore.Behaviors.StimResponse;

namespace SpiceEngine.Scripting
{
    public class BehaviorSystem : ComponentSystem<IBehavior, IBehaviorBuilder>, IStimulusProvider
    {
        private ScriptManager _scriptManager = new ScriptManager();

        private ICamera _camera;
        private ICollisionProvider _collisionProvider;
        private IInputProvider _inputProvider;
        private ISelectionTracker _selectionTracker;
        private IUIProvider _uiProvider;

        private Dictionary<int, PropertyCollection> _propertiesByEntityID = new Dictionary<int, PropertyCollection>();
        private Dictionary<int, StimulusCollection> _stimuliByEntityID = new Dictionary<int, StimulusCollection>();

        public BehaviorSystem(IEntityProvider entityProvider, ICollisionProvider collisionProvider)
        {
            SetEntityProvider(entityProvider);
            _collisionProvider = collisionProvider;
        }

        public void SetCamera(ICamera camera)
        {
            _camera = camera;

            if (IsLoaded)
            {
                foreach (var actor in _entityProvider.Actors)
                {
                    if (_componentByID.ContainsKey(actor.ID))
                    {
                        var behavior = _componentByID[actor.ID];
                        behavior.SetCamera(_camera);
                    }
                }
            }
        }

        public void SetSelectionTracker(ISelectionTracker selectionTracker)
        {
            _selectionTracker = selectionTracker;

            if (IsLoaded)
            {
                foreach (var actor in _entityProvider.Actors)
                {
                    if (_componentByID.ContainsKey(actor.ID))
                    {
                        var behavior = _componentByID[actor.ID];
                        behavior.SetSelectionTracker(_selectionTracker);
                    }
                }

                foreach (var uiItem in _entityProvider.UIItems)
                {
                    if (_componentByID.ContainsKey(uiItem.ID))
                    {
                        var behavior = _componentByID[uiItem.ID];
                        behavior.SetSelectionTracker(_selectionTracker);
                    }
                }
            }
        }

        public void SetInputProvider(IInputProvider inputProvider)
        {
            _inputProvider = inputProvider;

            if (IsLoaded)
            {
                foreach (var actor in _entityProvider.Actors)
                {
                    if (_componentByID.ContainsKey(actor.ID))
                    {
                        var behavior = _componentByID[actor.ID];
                        behavior.SetInputProvider(inputProvider);
                    }
                }
            }
        }

        public void SetUIProvider(IUIProvider uiProvider)
        {
            _uiProvider = uiProvider;

            if (IsLoaded)
            {
                foreach (var actor in _entityProvider.Actors)
                {
                    if (_componentByID.ContainsKey(actor.ID))
                    {
                        var behavior = _componentByID[actor.ID];
                        behavior.SetUIProvider(_uiProvider);
                    }
                }

                foreach (var uiItem in _entityProvider.UIItems)
                {
                    if (_componentByID.ContainsKey(uiItem.ID))
                    {
                        var behavior = _componentByID[uiItem.ID];
                        behavior.SetUIProvider(_uiProvider);
                    }
                }
            }
        }

        public void AddProperties(int entityID, IEnumerable<IProperty> properties)
        {
            var propertyCollection = new PropertyCollection();
            propertyCollection.AddProperties(properties);

            _propertiesByEntityID.Add(entityID, propertyCollection);
        }

        public void AddStimuli(int entityID, IEnumerable<IStimulus> stimuli)
        {
            var stimulusCollection = new StimulusCollection();
            stimulusCollection.AddStimuli(stimuli);

            _stimuliByEntityID.Add(entityID, stimulusCollection);
        }

        public IEnumerable<IStimulus> GetStimuli(int entityID) => _stimuliByEntityID.ContainsKey(entityID)
            ? _stimuliByEntityID[entityID].Stimuli
            : Enumerable.Empty<IStimulus>();

        public override void LoadBuilderSync(int entityID, IBehaviorBuilder builder)
        {
            base.LoadBuilderSync(entityID, builder);
            _scriptManager.AddScripts(builder.GetScripts());

            AddStimuli(entityID, builder.GetStimuli());
            AddProperties(entityID, builder.GetProperties());
        }

        protected override void LoadComponents()
        {
            base.LoadComponents();
            _scriptManager.CompileScripts();

            foreach (var behavior in _components)
            {
                var entity = _entityProvider.GetEntity(behavior.EntityID);

                behavior.SetEntity(entity);
                behavior.SetCamera(_camera);
                behavior.SetEntityProvider(_entityProvider);
                behavior.SetCollisionProvider(_collisionProvider);
                behavior.SetInputProvider(_inputProvider);
                behavior.SetStimulusProvider(this);
                behavior.SetSelectionTracker(_selectionTracker);
                behavior.SetUIProvider(_uiProvider);

                /*foreach (var property in Properties)
                {
                    if (property.Value.IsConstant)
                    {
                        Behaviors.Context.AddProperty(property.Key, property.Value.Value);
                    }
                }*/
            }
        }

        /*protected override Task LoadInitial() => Task.Run(() =>
        {
            foreach (var componentAndID in _componentsAndIDs)
            {
                var behavior = componentAndID.Item1;
                var entity = _entityProvider.GetEntity(componentAndID.Item2);

                behavior.SetEntity(entity);
                behavior.SetCamera(_camera);
                behavior.SetEntityProvider(_entityProvider);
                behavior.SetCollisionProvider(_collisionProvider);
                behavior.SetInputProvider(_inputProvider);
                behavior.SetStimulusProvider(this);

                /*foreach (var property in Properties)
                {
                    if (property.Value.IsConstant)
                    {
                        Behaviors.Context.AddProperty(property.Key, property.Value.Value);
                    }
                }*
            }
        });*/

        protected override void Update()
        {
            foreach (var behavior in _components)
            {
                foreach (var property in _propertiesByEntityID[behavior.EntityID].VariableProperties)
                {
                    behavior.SetProperty(property.Name, property);
                }

                behavior.Tick();
            }
        }
    }
}
