﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SpiceEngineCore.Serialization.Converters
{
    public class VectorConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(Vector2)
            || objectType == typeof(Vector3)
            || objectType == typeof(Vector4);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = JToken.Load(reader);
            if (obj.Type == JTokenType.Array)
            {
                var arr = (JArray)obj;
                if (arr.All(token => token.Type == JTokenType.Float))
                {
                    switch (arr.Count)
                    {
                        case 2:
                            return new Vector2(arr[0].Value<float>(), arr[1].Value<float>());
                        case 3:
                            return new Vector3(arr[0].Value<float>(), arr[1].Value<float>(), arr[2].Value<float>());
                        case 4:
                            return new Vector4(arr[0].Value<float>(), arr[1].Value<float>(), arr[2].Value<float>(), arr[3].Value<float>());
                    }
                }
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartArray();

            switch (value)
            {
                case Vector2 vector2:
                    writer.WriteValue(vector2.X);
                    writer.WriteValue(vector2.Y);
                    break;
                case Vector3 vector3:
                    writer.WriteValue(vector3.X);
                    writer.WriteValue(vector3.Y);
                    writer.WriteValue(vector3.Z);
                    break;
                case Vector4 vector4:
                    writer.WriteValue(vector4.X);
                    writer.WriteValue(vector4.Y);
                    writer.WriteValue(vector4.Z);
                    writer.WriteValue(vector4.W);
                    break;
            }

            writer.WriteEndArray();
        }
    }
}
