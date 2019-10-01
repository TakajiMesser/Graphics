using System.IO;
using SauceEditorCore.Models.Components;

namespace SauceEditorCore.Models.Libraries
{
    public class LibraryManager
    {
        public const string FILE_EXTENSION = "LM";

        private LibraryNode _node;

        public string Path { get; private set; }

        public Library<MapComponent> MapLibrary { get; private set; }
        public Library<ModelComponent> ModelLibrary { get; private set; }
        public Library<BehaviorComponent> BehaviorLibrary { get; private set; }
        public Library<TextureComponent> TextureLibrary { get; private set; }
        public Library<SoundComponent> SoundLibrary { get; private set; }
        public Library<MaterialComponent> MaterialLibrary { get; private set; }
        public Library<ArchetypeComponent> ArchetypeLibrary { get; private set; }
        public Library<ScriptComponent> ScriptLibrary { get; private set; }

        public void Save()
        {
            File.WriteAllLines(Path, new []
            {
                MapLibrary.Path,
                    ModelLibrary.Path,
                    BehaviorLibrary.Path,
                    TextureLibrary.Path,
                    SoundLibrary.Path,
                    MaterialLibrary.Path,
                    ArchetypeLibrary.Path,
                    ScriptLibrary.Path,
            });
        }

        public void Load()
        {
            var lines = File.ReadAllLines(Path);

            MapLibrary = new Library<MapComponent>(lines[0]);
            ModelLibrary = new Library<ModelComponent>(lines[1]);
            BehaviorLibrary = new Library<BehaviorComponent>(lines[2]);
            TextureLibrary = new Library<TextureComponent>(lines[3]);
            SoundLibrary = new Library<SoundComponent>(lines[4]);
            MaterialLibrary = new Library<MaterialComponent>(lines[5]);
            ArchetypeLibrary = new Library<ArchetypeComponent>(lines[6]);
            ScriptLibrary = new Library<ScriptComponent>(lines[7]);
        }

        private void LoadAllLibraries()
        {
            MapLibrary.Load();
            ModelLibrary.Load();
            BehaviorLibrary.Load();
            TextureLibrary.Load();
            SoundLibrary.Load();
            MaterialLibrary.Load();
            ArchetypeLibrary.Load();
            ScriptLibrary.Load();
        }

    }
}
