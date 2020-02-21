using SpiceEngineCore.Scripting;
using System;
using System.Runtime.Serialization;

namespace UmamiScriptingCore.Behaviors.Properties
{
    [DataContract]
    public class Property : IProperty
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

        public Property(string name, Type type, object value, bool isConstant = false)
        {
            Name = name;
            Type = type;
            Value = value;
            IsConstant = isConstant;
        }
    }
}
