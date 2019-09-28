using SauceEditorCore.Models.Components;
using System.Runtime.Serialization;
using System.Xml;

namespace SauceEditor.Models.Components
{
    public class LibraryManager
    {
        public Library<MapComponent> MapLibrary { get; set; } = new Library<MapComponent>();
        public Library<ModelComponent> ModelLibrary { get; set; } = new Library<ModelComponent>();
        public Library<BehaviorComponent> BehaviorLibrary { get; set; } = new Library<BehaviorComponent>();
        public Library<TextureComponent> TextureLibrary { get; set; } = new Library<TextureComponent>();
        public Library<SoundComponent> SoundLibrary { get; set; } = new Library<SoundComponent>();
        public Library<MaterialComponent> MaterialLibrary { get; set; } = new Library<MaterialComponent>();
        public Library<ArchetypeComponent> ArchetypeLibrary { get; set; } = new Library<ArchetypeComponent>();
        public Library<ScriptComponent> ScriptLibrary { get; set; } = new Library<ScriptComponent>();

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
