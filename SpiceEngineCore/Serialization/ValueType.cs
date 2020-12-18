using Newtonsoft.Json.Linq;
using OpenTK;
using System;

namespace SpiceEngineCore.Serialization
{
    public class ValueType
    {
        public object Value { get; set; }
        public Type Type { get; set; }

        public object Cast()
        {
            if (Value is JArray arr)
            {
                if (Type == typeof(Vector2))
                {
                    return new Vector2(arr[0].Value<float>(), arr[1].Value<float>());
                }
                else if (Type == typeof(Vector3))
                {
                    return new Vector3(arr[0].Value<float>(), arr[1].Value<float>(), arr[2].Value<float>());
                }
                else if (Type == typeof(Vector4))
                {
                    return new Vector4(arr[0].Value<float>(), arr[1].Value<float>(), arr[2].Value<float>(), arr[3].Value<float>());
                }
            }
            
            return Convert.ChangeType(Value, Type);
        }

        //private ValueType() { }

        public static ValueType Create<T>(T value)
        {
            return new ValueType()
            {
                Value = value,
                Type = value.GetType()
            };
        }
    }
}
