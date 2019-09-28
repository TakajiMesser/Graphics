using SauceEditorCore.Models.Components;
using SpiceEngineCore.Serialization.Converters;
using System.Collections.Generic;

namespace SauceEditor.Models.Components
{
    /// <summary>
    /// All component paths are relative to parent Project path
    /// </summary>
    public class Project
    {
        public const string FILE_EXTENSION = ".pro";

        public string Name { get; set; }
        public string Path { get; set; }

        public List<MapComponent> MapComponents { get; set; } = new List<MapComponent>();
        public List<ModelComponent> ModelComponents { get; set; } = new List<ModelComponent>();
        public List<BehaviorComponent> BehaviorComponents { get; set; } = new List<BehaviorComponent>();
        public List<TextureComponent> TextureComponents { get; set; } = new List<TextureComponent>();
        public List<SoundComponent> SoundComponents { get; set; } = new List<SoundComponent>();
        public List<MaterialComponent> MaterialComponents { get; set; } = new List<MaterialComponent>();
        public List<ArchetypeComponent> ArchetypeComponents { get; set; } = new List<ArchetypeComponent>();
        public List<ScriptComponent> ScriptComponents { get; set; } = new List<ScriptComponent>();

        public void Save() => Serializer.Save(Path, this);

        public static Project Load(string path)
        {
            var project = Serializer.Load<Project>(path);
            project.Path = path;

            return project;
        }
    }
}
