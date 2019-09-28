using SauceEditorCore.Models.Components;
using System.Collections.Generic;
using System.IO;

namespace SauceEditorCore.Models.Libraries
{
    public class Library : Component
    {
        public List<IComponent> Components { get; } = new List<IComponent>();

        public Library(string path) : base(path) { }

        public override void Load()
        {
            foreach (var filePath in Directory.GetFiles(Path))
            {
                var extension = System.IO.Path.GetExtension(filePath);

                switch (extension)
                {
                    case "map":
                        Components.Add(new MapComponent(filePath));
                        break;
                }
            }

            foreach (var directoryPath in Directory.GetDirectories(Path))
            {
                Components.Add(new Library(directoryPath));
            }
        }

        public override void Save()
        {
            
        }
    }
}