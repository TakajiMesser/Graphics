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
        public string GameObjectName { get; set; }
        public IEnumerable<Collider> Colliders { get; set; }
        public InputState InputState { get; set; }
        public InputMapping InputMapping { get; set; }
        public Camera Camera { get; set; }

        // Values set by the GameObject, or altered by Behavior Nodes
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }

        // Values set by Behavior Nodes, and RESET by the GameObject
        public Vector3 Translation { get; set; }

        public Dictionary<string, object> VariablesByName { get; protected set; } = new Dictionary<string, object>();

        public bool ContainsKey(string key) => VariablesByName.ContainsKey(key);

        public void Add(string key, object value) => VariablesByName.Add(key, value);
        public void Remove(string key) => VariablesByName.Remove(key);

        public object this[string key]
        {
            get => VariablesByName[key];
            set => VariablesByName[key] = value;
        }
    }
}
