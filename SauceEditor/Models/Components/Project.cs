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

        public List<MapComponent> MapComponents { get; set; } = new List<MapComponent>();
        public List<ModelComponent> ModelComponents { get; set; } = new List<ModelComponent>();
        public List<BehaviorComponent> BehaviorComponents { get; set; } = new List<BehaviorComponent>();
        public List<TextureComponent> TextureComponents { get; set; } = new List<TextureComponent>();
        public List<SoundComponent> SoundComponents { get; set; } = new List<SoundComponent>();
        public List<MaterialComponent> MaterialComponents { get; set; } = new List<MaterialComponent>();
        public List<ArchetypeComponent> ArchetypeComponents { get; set; } = new List<ArchetypeComponent>();
        public List<ScriptComponent> ScriptComponents { get; set; } = new List<ScriptComponent>();

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
