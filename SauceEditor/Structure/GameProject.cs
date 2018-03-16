using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SauceEditor.Structure
{
    public class GameProject
    {
        public const string FILE_EXTENSION = ".pro";

        public string Name { get; set; }

        /// <summary>
        /// All paths should be relative to the project location
        /// </summary>
        public List<string> MapPaths { get; set; }
        public List<string> ModelPaths { get; set; }
        public List<string> BehaviorPaths { get; set; }
        public List<string> TexturePaths { get; set; }
        public List<string> AudioPaths { get; set; }

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
