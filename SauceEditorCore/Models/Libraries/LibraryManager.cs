using System.IO;

namespace SauceEditorCore.Models.Libraries
{
    public class LibraryManager
    {
        public static string FILE_EXTENSION = "LM";

        public Library MapLibrary { get; private set; }
        public Library ModelLibrary { get; private set; }
        public Library BehaviorLibrary { get; private set; }
        public Library TextureLibrary { get; private set; }
        public Library SoundLibrary { get; private set; }
        public Library MaterialLibrary { get; private set; }
        public Library ArchetypeLibrary { get; private set; }
        public Library ScriptLibrary { get; private set; }

        public void Save(string path)
        {
            File.WriteAllLines(path, new[]
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

        public static LibraryManager Load(string path)
        {
            var lines = File.ReadAllLines(path);

            return new LibraryManager()
            {
                MapLibrary = new Library(lines[0]),
                ModelLibrary = new Library(lines[1]),
                BehaviorLibrary = new Library(lines[2]),
                TextureLibrary = new Library(lines[3]),
                SoundLibrary = new Library(lines[4]),
                MaterialLibrary = new Library(lines[5]),
                ArchetypeLibrary = new Library(lines[6]),
                ScriptLibrary = new Library(lines[7])
            };
        }
    }
}