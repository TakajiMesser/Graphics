using SauceEditorCore.Models.Components;
using System.Collections.Generic;

namespace SauceEditorCore.Models.Libraries
{
    public class LibraryNode
    {
        private Dictionary<string, LibraryNode> _childrenByName = new Dictionary<string, LibraryNode>();

        public string Name { get; }
        public string Path { get; }
        public List<IComponent> Components { get; } = new List<IComponent>();

        public LibraryNode Parent { get; private set; }
        public IEnumerable<LibraryNode> Children => _childrenByName.Values;

        public LibraryNode(string path)
        {
            Name = System.IO.Path.GetDirectoryName(path);
            Path = path;
        }

        public void AddChild(LibraryNode node)
        {
            _childrenByName.Add(node.Name, node);
            node.Parent = this;
        }

        public LibraryNode GetChild(string name) => _childrenByName[name];

        public void ClearChildren() => _childrenByName.Clear();

        public static BitmapImage GetPreviewIcon()
        {
            return null;
        }
    }
}
