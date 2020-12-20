using Newtonsoft.Json.Linq;
using System;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

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
