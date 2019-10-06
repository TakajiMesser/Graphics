using SauceEditorCore.Models.Components;
using SpiceEngineCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SauceEditorCore.Models.Libraries
{
    public class Library<T> : ILibrary where T : IComponent
    {
        private List<T> _components = new List<T>();

        public Library(string path)
        {
            Path = path;
            Name = GetName();
        }

        public string Name { get; }
        public string Path { get; set; }
        public IEnumerable<IComponent> Components => _components.OfType<IComponent>();

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

        private static string GetName()
        {
            return new TypeSwitch<string>()
                .Case<MapComponent>(() => "Maps")
                .Case<ModelComponent>(() => "Models")
                .Case<BehaviorComponent>(() => "Behaviors")
                .Case<TextureComponent>(() => "Textures")
                .Case<SoundComponent>(() => "Sounds")
                .Case<MaterialComponent>(() => "Materials")
                .Case<ArchetypeComponent>(() => "Archetypes")
                .Case<ScriptComponent>(() => "Scripts")
                .Default(() => throw new NotImplementedException())
                .Match<T>();
        }
    }
}
