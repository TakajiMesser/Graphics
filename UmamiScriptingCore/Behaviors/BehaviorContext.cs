using SpiceEngineCore.Commands;
using SpiceEngineCore.Components;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Game;
using SpiceEngineCore.Geometry;
using System.Collections.Generic;
using System.Linq;
using UmamiScriptingCore.StimResponse;

namespace UmamiScriptingCore.Behaviors
{
    public class BehaviorContext
    {
        private IBehavior _behavior;
        private IStimulusProvider _stimulusProvider;
        private Dictionary<string, object> _propertiesByName = new Dictionary<string, object>();
        private Dictionary<string, object> _variablesByName = new Dictionary<string, object>();

        public BehaviorContext(IBehavior behavior) => _behavior = behavior;

        public ISystemProvider SystemProvider { get; internal set; }
        public ICommander Commander { get; internal set; }

        public IEntity GetEntity() => SystemProvider.EntityProvider.GetEntity(_behavior.EntityID);
        public IEntity GetEntity(int id) => SystemProvider.EntityProvider.GetEntity(id);
        public IEntity GetEntity(string name) => SystemProvider.EntityProvider.GetEntity(name);

        public T GetComponent<T>() where T : IComponent => SystemProvider.GetComponent<T>(_behavior.EntityID);
        public T GetComponent<T>(int id) where T : IComponent => SystemProvider.GetComponent<T>(id);

        public Vector3 GetPosition() => GetEntity().Position;

        public void SetStimulusProvider(IStimulusProvider stimulusProvider) => _stimulusProvider = stimulusProvider;

        /*public Vector3 Position
        {
            get => _collisionProvider.GetBody(Entity.ID).Position;
            set => _collisionProvider.GetBody(Entity.ID).Position = value;
        }*/

        //public IBody Body => _collisionProvider.GetBody(Entity.ID);

        public bool HasStimuli(int entityID, IStimulus stimulus) => _stimulusProvider.GetStimuli(entityID).Contains(stimulus);

        /*public IBody GetBody(int entityID) => _collisionProvider.GetBody(entityID);
        public IEnumerable<IBody> GetBodies() => _collisionProvider.GetCollisionIDs().Select(c => _collisionProvider.GetBody(c));
        public IEnumerable<IBody> GetColliderBodies() => _collisionProvider.GetCollisionIDs(Entity.ID).Select(c => _collisionProvider.GetBody(c));*/

        //public IUIElement GetUIElement(int entityID) => _uiProvider.GetUIElement(entityID);
        //public IInputProvider InputProvider { get; internal set; }

        public Vector3 EulerRotation { get; set; }
        //public Vector3 Translation { get; set; }

        public bool ContainsProperty(string name) => _propertiesByName.ContainsKey(name);
        public void AddProperty(string name, object value) => _propertiesByName.Add(name, value);
        public T GetProperty<T>(string name) => (T)_propertiesByName[name];
        public void SetProperty(string name, object value) => _propertiesByName[name] = value;

        public bool ContainsVariable(string name) => _variablesByName.ContainsKey(name);
        public void AddVariable(string name, object value) => _variablesByName.Add(name, value);
        public void RemoveVariable(string name) => _variablesByName.Remove(name);
        public T GetVariable<T>(string name) => (T)_variablesByName[name];
        public T GetVariableOrDefault<T>(string name) => _variablesByName.ContainsKey(name) ? (T)_variablesByName[name] : default(T);
        public void SetVariable(string name, object value) => _variablesByName[name] = value;

        public void RemoveVariableIfExists(string name)
        {
            if (_variablesByName.ContainsKey(name))
            {
                _variablesByName.Remove(name);
            }
        }

        /*public int GetEntityIDFromMouseCoordinates()
        {
            if (InputProvider != null && _selectionTracker != null && InputProvider.IsMouseInWindow && InputProvider.RelativeCoordinates.HasValue)
            {
                return _selectionTracker.GetEntityIDFromSelection(InputProvider.RelativeCoordinates.Value);
            }

            return 0;
        }*/
    }
}
