using OpenTK;
using System.Collections.Generic;
using TakoEngine.Entities.Cameras;
using TakoEngine.Inputs;
using TakoEngine.Physics.Collision;

namespace TakoEngine.Scripting.BehaviorTrees
{
    public class BehaviorContext
    {
        // Values set by the Actor
        public string ActorName { get; internal set; }
        public Bounds Bounds { get; internal set; }
        public IEnumerable<Bounds> Colliders { get; internal set; }
        public InputState InputState { get; internal set; }
        public InputMapping InputMapping { get; internal set; }
        public Camera Camera { get; internal set; }

        // Values set by the Actor, or altered by Behavior Nodes
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Quaternion QRotation { get; set; } = Quaternion.Identity;
        public Vector3 Scale { get; set; }

        // Values set by Behavior Nodes, and RESET by the Actor
        public Vector3 Translation { get; set; }

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

        public Vector3 GetTranslation(float speed)
        {
            Vector3 translation = new Vector3();

            // Project the "Up" vector of the camera's view onto the XY plane, since that is what we restrict our movement translation to
            var flattenedUp = Camera._viewMatrix.Up.Xy;
            var up = new Vector3(flattenedUp.X, flattenedUp.Y, 0.0f);
            var right = new Vector3(flattenedUp.Y, -flattenedUp.X, 0.0f);

            if (InputState.IsHeld(InputMapping.Forward))
            {
                 translation += up.Normalized() * speed;
            }

            if (InputState.IsHeld(InputMapping.Left))
            {
                translation -= right.Normalized() * speed;
            }

            if (InputState.IsHeld(InputMapping.Backward))
            {
                translation -= up.Normalized() * speed;
            }

            if (InputState.IsHeld(InputMapping.Right))
            {
                translation += right.Normalized() * speed;
            }

            return translation;
        }
    }
}
