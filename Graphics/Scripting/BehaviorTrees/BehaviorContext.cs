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

        public Dictionary<string, object> VariablesByName { get; protected set; } = new Dictionary<string, object>();

        public bool ContainsKey(string key) => VariablesByName.ContainsKey(key);

        public void Add(string key, object value) => VariablesByName.Add(key, value);
        public void Remove(string key) => VariablesByName.Remove(key);

        public void RemoveIfExists(string key)
        {
            if (VariablesByName.ContainsKey(key))
            {
                VariablesByName.Remove(key);
            }
        }

        public object this[string key]
        {
            get => VariablesByName[key];
            set => VariablesByName[key] = value;
        }

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
