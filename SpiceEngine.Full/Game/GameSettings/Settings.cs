using System.Runtime.Serialization;
using System.Xml;

namespace SpiceEngine.Game.GameSettings
{
    public class Settings
    {
        public const string FILE_EXTENSION = ".user";

        public GameplaySettings GameplaySettings { get; set; }
        public ControlSettings ControlSettings { get; set; }
        public VideoSettings VideoSettings { get; set; }
        public AudioSettings AudioSettings { get; set; }

        public void Save(string path)
        {
            /*using (var writer = XmlWriter.Create(path))
            {
                var serializer = new NetDataContractSerializer();
                serializer.WriteObject(writer, this);
            }*/
        }

        public static Settings Load(string path)
        {
            /*using (var reader = XmlReader.Create(path))
            {
                var serializer = new NetDataContractSerializer();
                return serializer.ReadObject(reader, true) as Settings;
            }*/
            return null;
        }
    }
}
