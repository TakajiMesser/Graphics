using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;

namespace SauceEditor.Models
{
    public class GameProject
    {
        public const string FILE_EXTENSION = ".pro";

        public string Name { get; set; }

        /// <summary>
        /// All paths should be relative to the project location
        /// </summary>
        public List<string> MapPaths { get; set; } = new List<string>();
        public List<string> ModelPaths { get; set; } = new List<string>();
        public List<string> BehaviorPaths { get; set; } = new List<string>();
        public List<string> TexturePaths { get; set; } = new List<string>();
        public List<string> AudioPaths { get; set; } = new List<string>();

        public void Save(string path)
        {
            using (var writer = XmlWriter.Create(path))
            {
                var serializer = new NetDataContractSerializer();
                serializer.WriteObject(writer, this);
            }
        }

        public static GameProject Load(string path)
        {
            using (var reader = XmlReader.Create(path))
            {
                var serializer = new NetDataContractSerializer();
                return serializer.ReadObject(reader, true) as GameProject;
            }
        }
    }
}
