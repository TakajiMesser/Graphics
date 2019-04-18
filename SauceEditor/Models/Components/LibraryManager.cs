using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;

namespace SauceEditor.Models.Components
{
    public class LibraryManager
    {
        public Library<Map> MapLibrary { get; set; } = new Library<Map>();
        public Library<Model> ModelLibrary { get; set; } = new Library<Model>();
        public Library<Behavior> BehaviorLibrary { get; set; } = new Library<Behavior>();
        public Library<Texture> TextureLibrary { get; set; } = new Library<Texture>();
        public Library<Sound> SoundLibrary { get; set; } = new Library<Sound>();
        public Library<Material> MaterialLibrary { get; set; } = new Library<Material>();
        public Library<Archetype> ArchetypeLibrary { get; set; } = new Library<Archetype>();
        public Library<Script> ScriptLibrary { get; set; } = new Library<Script>();

        public void Save(string path)
        {
            using (var writer = XmlWriter.Create(path))
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
                return serializer.ReadObject(reader, true) as Project;
            }
        }
    }
}
