using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace SpiceEngineCore.Serialization.Converters
{
    public static class Serializer
    {
        public static T LoadFromData<T>(byte[] data)
        {
            var serializer = GetSerializer();

            using var memory = new MemoryStream(data);
            using var stream = new StreamReader(memory);
            using var reader = new JsonTextReader(stream);

            return serializer.Deserialize<T>(reader);
        }

        public static T LoadFromContents<T>(string contents)
        {
            var serializer = GetSerializer();

            using var memory = new MemoryStream(Encoding.UTF8.GetBytes(contents));
            using var stream = new StreamReader(memory);
            using var reader = new JsonTextReader(stream);

            return serializer.Deserialize<T>(reader);
        }

        public static T LoadFromPath<T>(string filePath)
        {
            var serializer = GetSerializer();

            using var stream = new StreamReader(filePath);
            using var reader = new JsonTextReader(stream);

            return serializer.Deserialize<T>(reader);
        }


        public static void Save<T>(string filePath, T value)
        {
            var serializer = GetSerializer();

            using var stream = new StreamWriter(filePath);
            using var writer = new JsonTextWriter(stream);

            serializer.Serialize(writer, value);
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
