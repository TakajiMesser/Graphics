using Graphics.Inputs;
using Graphics.Meshes;
using Graphics.Physics.Collision;
using Graphics.Rendering.Shaders;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.GameObjects
{
    [DataContract]
    public class GameProperty
    {
        [DataMember]
        internal object _value;

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public Type Type { get; private set; }

        [DataMember]
        public bool IsConstant { get; private set; }
        public object Value
        {
            get => _value;
            set
            {
                if (IsConstant)
                {
                    throw new InvalidOperationException("Cannot change the value of a constant parameter");
                }

                if (value.GetType() != Type)
                {
                    throw new ArgumentException("Value must be of type " + nameof(Type));
                }

                _value = value;
            }
        }

        public GameProperty(string name, Type type, object value, bool isConstant = false)
        {
            Name = name;
            Type = type;
            Value = value;
            IsConstant = isConstant;
        }
    }
}
