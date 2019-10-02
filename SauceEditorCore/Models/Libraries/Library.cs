using SauceEditorCore.Models.Components;
using System.Collections.Generic;

namespace SauceEditorCore.Models.Libraries
{
    public class Library<T> : ILibrary where T : IComponent
    {
        private List<T> _components = new List<T>();

        public Library(string path) => Path = path;

        public string Path { get; set; }
        public LibraryInfo Info { get; private set; } = new LibraryInfo();

        public void AddComponent(T component)
        {
            _components.Add(component);
            Info.AddComponent(component);
        }

        public T GetComponentAt(int index) => _components[index];
        public ComponentInfo GetComponentInfoAt(int index) => Info.GetInfoAt(index);

        public void Load() => Info.RefreshAll();

        public void Clear()
        {
            _components.Clear();
            Info.Clear();
        }
    }
}
