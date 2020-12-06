using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpiceEngineCore.Geometry.Quaternions;
using System;
using System.Linq;

namespace SpiceEngineCore.Serialization.Converters
{
    public class QuaternionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(Quaternion);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = JToken.Load(reader);
            if (obj.Type == JTokenType.Array)
            {
                var arr = (JArray)obj;
                if (arr.All(token => token.Type == JTokenType.Float) && arr.Count == 4)
                {
                    return new Quaternion(arr[0].Value<float>(), arr[1].Value<float>(), arr[2].Value<float>(), arr[3].Value<float>());
                }
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartArray();

            var quaternion = (Quaternion)value;
            writer.WriteValue(quaternion.X);
            writer.WriteValue(quaternion.Y);
            writer.WriteValue(quaternion.Z);
            writer.WriteValue(quaternion.W);

            writer.WriteEndArray();
        }
    }
}
