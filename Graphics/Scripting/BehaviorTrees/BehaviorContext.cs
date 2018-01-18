using Graphics.GameObjects;
using Graphics.Helpers;
using Graphics.Inputs;
using Graphics.Physics.Collision;
using Graphics.Physics.Raycasting;
using Graphics.Scripting.BehaviorTrees.Composites;
using Graphics.Scripting.BehaviorTrees.Decorators;
using Graphics.Scripting.BehaviorTrees.Leaves;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Graphics.Scripting.BehaviorTrees
{
    public class BehaviorContext
    {
        // Values set by the GameObject
        public string GameObjectName { get; internal set; }
        public Bounds Bounds { get; internal set; }
        public IEnumerable<Bounds> Colliders { get; internal set; }
        public InputState InputState { get; internal set; }
        public InputMapping InputMapping { get; internal set; }
        public Camera Camera { get; internal set; }

        // Values set by the GameObject, or altered by Behavior Nodes
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Quaternion QRotation { get; set; } = Quaternion.Identity;
        public Vector3 Scale { get; set; }

        // Values set by Behavior Nodes, and RESET by the GameObject
        public Vector3 Translation { get; set; }

        public Dictionary<string, object> PropertiesByName { get; protected set; } = new Dictionary<string, object>();
        public Dictionary<string, object> VariablesByName { get; protected set; } = new Dictionary<string, object>();

        public bool ContainsVariable(string name) => VariablesByName.ContainsKey(name);
        public bool ContainsProperty(string name) => PropertiesByName.ContainsKey(name);

        public void AddProperty(string name, object value) => PropertiesByName.Add(name, value);

        public void AddVariable(string name, object value) => VariablesByName.Add(name, value);
        public void RemoveVariable(string name) => VariablesByName.Remove(name);

        public void RemoveVariableIfExists(string name)
        {
            if (VariablesByName.ContainsKey(name))
            {
                VariablesByName.Remove(name);
            }
        }

        public T GetProperty<T>(string name) => (T)PropertiesByName[name];
        public T GetVariable<T>(string name) => (T)VariablesByName[name];

        public void SetProperty(string name, object value) => PropertiesByName[name] = value;
        public void SetVariable(string name, object value) => VariablesByName[name] = value;

        public Vector3 GetTranslation(float speed)
        {
            Vector3 translation = new Vector3();

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
