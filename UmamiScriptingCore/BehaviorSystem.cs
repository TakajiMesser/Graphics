using SpiceEngineCore.Components;
using System.Collections.Generic;
using System.Linq;
using UmamiScriptingCore.Props;
using UmamiScriptingCore.Scripts;
using UmamiScriptingCore.StimResponse;

namespace UmamiScriptingCore
{
    public class BehaviorSystem : ComponentSystem<IBehavior, IBehaviorBuilder>, IStimulusProvider
    {
        private IScriptCompiler _scriptCompiler;

        private Dictionary<int, PropertyCollection> _propertiesByEntityID = new Dictionary<int, PropertyCollection>();
        private Dictionary<int, StimulusCollection> _stimuliByEntityID = new Dictionary<int, StimulusCollection>();

        public BehaviorSystem(IScriptCompiler scriptCompiler) => _scriptCompiler = scriptCompiler;

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
            _scriptCompiler.AddScripts(builder.GetScripts());

            AddStimuli(entityID, builder.GetStimuli());
            AddProperties(entityID, builder.GetProperties());
        }

        protected override void LoadComponents()
        {
            base.LoadComponents();
            _scriptCompiler.CompileScripts();

            foreach (var behavior in _components)
            {
                behavior.SetSystemProvider(_systemProvider);
                behavior.SetCommander(_commander);
                behavior.SetStimulusProvider(this);

                foreach (var property in _propertiesByEntityID[behavior.EntityID].ConstantProperties)
                {
                    behavior.Context.AddProperty(property.Name, property);
                }
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
