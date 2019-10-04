using SauceEditorCore.Models.Components;
using System.Collections.Generic;

namespace SauceEditorCore.Models.Libraries
{
    public class Library<T> : ILibrary where T : IComponent
    {
        private List<T> _components = new List<T>();

        public Library(string path) => Path = path;

        public string Path { get; set; }

        public void Load()
        {
            foreach (var component in _components)
            {
                component.Load();
            }
        }

        public void AddComponent(T component) => _components.Add(component);

        public T GetComponentAt(int index) => _components[index];

        public void Clear() => _components.Clear();
    }
}
