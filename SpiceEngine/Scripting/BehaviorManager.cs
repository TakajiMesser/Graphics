﻿using SpiceEngine.Scripting.Scripts;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Game.Loading;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Inputs;
using SpiceEngineCore.Physics;
using SpiceEngineCore.Scripting;
using System.Collections.Generic;
using System.Linq;
using UmamiScriptingCore.Behaviors.Properties;
using UmamiScriptingCore.Behaviors.StimResponse;

namespace SpiceEngine.Scripting
{
    public class BehaviorManager : ComponentLoader<IBehavior, IBehaviorBuilder>, IStimulusProvider
    {
        private ScriptManager _scriptManager = new ScriptManager();

        private ICamera _camera;
        private ICollisionProvider _collisionProvider;
        private IInputProvider _inputProvider;

        private Dictionary<int, PropertyCollection> _propertiesByEntityID = new Dictionary<int, PropertyCollection>();
        private Dictionary<int, StimulusCollection> _stimuliByEntityID = new Dictionary<int, StimulusCollection>();

        public BehaviorManager(IEntityProvider entityProvider, ICollisionProvider collisionProvider)
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
            foreach (var componentAndID in _componentsAndIDs)
            {
                var behavior = componentAndID.Item1;

                foreach (var property in _propertiesByEntityID[componentAndID.Item2].VariableProperties)
                {
                    behavior.SetProperty(property.Name, property);
                }

                behavior.Tick();
            }
        }
    }
}
