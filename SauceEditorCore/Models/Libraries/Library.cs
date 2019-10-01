using System.Collections.Generic;
using System.IO;
using SauceEditorCore.Helpers;
using SauceEditorCore.Models.Components;

namespace SauceEditorCore.Models.Libraries
{
    public class Library<T> where T : IComponent
    {
        private LibraryNode _node = new LibraryNode();

        public Library(string path) => Path = path;

        public string Path { get; set; }
        public List<IComponent> Components { get; } = new List<IComponent>();

        public void Load() => SearchForComponents(Path, _node);

        private void SearchForComponents(string path, LibraryNode node)
        {
            foreach (var filePath in Directory.GetFiles(path))
            {
                var extension = System.IO.Path.GetExtension(filePath);

                if (ComponentFactory.IsValidExtension<T>(extension))
                {
                    var component = ComponentFactory.Create<T>(filePath);

                    Components.Add(component);
                    node.Components.Add(component);
                }
            }

            foreach (var directoryPath in Directory.GetDirectories(path))
            {
                var childNode = new LibraryNode(directoryPath);
                node.Nodes.Add(childNode);

                SearchForComponents(directoryPath, childNode);
            }
        }

        private class LibraryNode
        {
            public string Name { get; }
            public List<IComponent> Components { get; } = new List<IComponent>();
            public List<LibraryNode> Nodes { get; } = new List<LibraryNode>();

            public LibraryNode(string path) => Name = System.IO.Path.GetDirectoryName(path);
        }
    }
}
