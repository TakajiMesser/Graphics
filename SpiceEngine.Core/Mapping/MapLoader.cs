using SpiceEngineCore.Serialization.Converters;

namespace SpiceEngineCore.Maps
{
    public abstract class MapLoader<T> where T : MapLoader<T>
    {
        public void Save(string filePath) => Serializer.Save(filePath, this as T);

        public static T LoadFromContents(string contents) => Serializer.LoadFromContents<T>(contents);
        public static T LoadFromData(byte[] data) => Serializer.LoadFromData<T>(data);
        public static T LoadFromPath(string filePath) => Serializer.LoadFromPath<T>(filePath);
    }
}
