using Newtonsoft.Json;
using System.IO;

namespace SpiceEngineCore.Serialization.Converters
{
    public static class Serializer
    {
        public static T Load<T>(string filePath)
        {
            var serializer = GetSerializer();

            using (StreamReader stream = new StreamReader(filePath))
            using (JsonTextReader reader = new JsonTextReader(stream))
            {
                return serializer.Deserialize<T>(reader);
            }
        }

        public static void Save<T>(string filePath, T value)
        {
            var serializer = GetSerializer();

            using (StreamWriter stream = new StreamWriter(filePath))
            using (JsonWriter writer = new JsonTextWriter(stream))
            {
                serializer.Serialize(writer, value);
            }
        }

        private static JsonSerializer GetSerializer()
        {
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new VectorConverter());
            serializer.Converters.Add(new QuaternionConverter());

            return serializer;
        }
    }
}
