using System.Collections.Generic;
using System.IO;
using SauceEditorCore.Helpers;
using SauceEditorCore.Models.Components;

namespace SauceEditorCore.Models.Libraries
{
    public class LibraryNode
    {
        public string Name { get; }
        public List<IComponent> Components { get; } = new List<IComponent>();
        public List<LibraryNode> Nodes { get; } = new List<LibraryNode>();

        public LibraryNode(string path) => Name = System.IO.Path.GetDirectoryName(path);
    }
}
