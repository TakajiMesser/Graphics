using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;

namespace SauceEditor.Models.Components
{
    /// <summary>
    /// All component paths are relative to parent Project path
    /// </summary>
    public class Project : Component
    {
        public const string FILE_EXTENSION = ".pro";

        public List<Map> Maps { get; set; } = new List<Map>();
        public List<Model> Models { get; set; } = new List<Model>();
        public List<Behavior> Behaviors { get; set; } = new List<Behavior>();
        public List<Texture> Textures { get; set; } = new List<Texture>();
        public List<Sound> Sounds { get; set; } = new List<Sound>();
        public List<Material> Materials { get; set; } = new List<Material>();
        public List<Archetype> Archetypes { get; set; } = new List<Archetype>();
        public List<Script> Scripts { get; set; } = new List<Script>();

        public void Save()
        {
            using (var writer = XmlWriter.Create(Path))
            {
                var serializer = new NetDataContractSerializer();
                serializer.WriteObject(writer, this);
            }
        }

        public static Project Load(string path)
        {
            using (var reader = XmlReader.Create(path))
            {
                var serializer = new NetDataContractSerializer();
                
                var project = serializer.ReadObject(reader, true) as Project;
                project.Path = path;

                return project;
            }
        }
    }
}
