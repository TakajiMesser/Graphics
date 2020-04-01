using OpenTK;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Inputs;
using SpiceEngineCore.Physics;
using SpiceEngineCore.Scripting;
using SpiceEngineCore.UserInterfaces;
using System.Collections.Generic;
using System.Linq;

namespace UmamiScriptingCore.Behaviors
{
    public class BehaviorContext
    {
        private IEntityProvider _entityProvider;
        private ICollisionProvider _collisionProvider;
        private IStimulusProvider _stimulusProvider;
        private ISelectionTracker _selectionTracker;
        private IUIProvider _uiProvider;

        public IEntity Entity { get; internal set; }

        public Vector3 Position
        {
            get => _collisionProvider.GetBody(Entity.ID).Position;
            set => _collisionProvider.GetBody(Entity.ID).Position = value;
        }

        public IBody Body => _collisionProvider.GetBody(Entity.ID);

        public IEntityProvider GetEntityProvider() => _entityProvider;
        public IEntity GetEntity(int id) => _entityProvider.GetEntity(id);
        public IEntity GetEntity(string name) => _entityProvider.GetEntity(name);

        public bool HasStimuli(int entityID, IStimulus stimulus) => _stimulusProvider.GetStimuli(entityID).Contains(stimulus);

        public IBody GetBody(int entityID) => _collisionProvider.GetBody(entityID);
        public IEnumerable<IBody> GetBodies() => _collisionProvider.GetCollisionIDs().Select(c => _collisionProvider.GetBody(c));
        public IEnumerable<IBody> GetColliderBodies() => _collisionProvider.GetCollisionIDs(Entity.ID).Select(c => _collisionProvider.GetBody(c));

        public IUIElement GetUIElement(int entityID) => _uiProvider.GetUIElement(entityID);

        public ICamera Camera { get; internal set; }
        public IInputProvider InputProvider { get; internal set; }

        public Vector3 EulerRotation { get; set; }
        //public Vector3 Translation { get; set; }

        public Dictionary<string, object> PropertiesByName { get; protected set; } = new Dictionary<string, object>();
        public Dictionary<string, object> VariablesByName { get; protected set; } = new Dictionary<string, object>();

        public bool ContainsProperty(string name) => PropertiesByName.ContainsKey(name);
        public void AddProperty(string name, object value) => PropertiesByName.Add(name, value);
        public T GetProperty<T>(string name) => (T)PropertiesByName[name];
        public void SetProperty(string name, object value) => PropertiesByName[name] = value;

        public bool ContainsVariable(string name) => VariablesByName.ContainsKey(name);
        public void AddVariable(string name, object value) => VariablesByName.Add(name, value);
        public void RemoveVariable(string name) => VariablesByName.Remove(name);
        public T GetVariable<T>(string name) => (T)VariablesByName[name];
        public T GetVariableOrDefault<T>(string name) => VariablesByName.ContainsKey(name) ? (T)VariablesByName[name] : default(T);
        public void SetVariable(string name, object value) => VariablesByName[name] = value;

        public void RemoveVariableIfExists(string name)
        {
            if (VariablesByName.ContainsKey(name))
            {
                VariablesByName.Remove(name);
            }
        }

        public int GetEntityIDFromMousePosition()
        {
            if (InputProvider != null && _selectionTracker != null && InputProvider.IsMouseInWindow && InputProvider.MouseCoordinates.HasValue)
            {
                var coordinates = new Vector2(InputProvider.MouseCoordinates.Value.X, InputProvider.WindowSize.Height - InputProvider.MouseCoordinates.Value.Y);
                return _selectionTracker.GetEntityIDFromPoint(coordinates);
            }

            return 0;
        }

        public void SetEntityProvider(IEntityProvider entityProvider) => _entityProvider = entityProvider;
        public void SetCollisionProvider(ICollisionProvider collisionProvider) => _collisionProvider = collisionProvider;
        public void SetInputProvider(IInputProvider inputProvider) => InputProvider = inputProvider;
        public void SetStimulusProvider(IStimulusProvider stimulusProvider) => _stimulusProvider = stimulusProvider;
        public void SetSelectionTracker(ISelectionTracker selectionTracker) => _selectionTracker = selectionTracker;
        public void SetUIProvider(IUIProvider uiProvider) => _uiProvider = uiProvider;
    }
}
