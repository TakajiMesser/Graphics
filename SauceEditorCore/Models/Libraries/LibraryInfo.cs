using SauceEditorCore.Models.Components;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using SpiceEngineCore.Helpers;

namespace SauceEditorCore.Models.Libraries
{
    public class LibraryInfo : IPathInfo 
    {
        private List<ComponentInfo> _componentInfos = new List<ComponentInfo>();

        private LibraryInfo() { }

        public string Name { get; private set; }
        public string Path { get; private set; }
        public bool Exists { get; private set; }
        public long FileSize { get; private set; }
        public BitmapImage PreviewIcon { get; private set; }

        public void AddComponentInfo(ComponentInfo componentInfo) => _componentInfos.Add(componentInfo);

        public int Count => _componentInfos.Count;

        public ComponentInfo GetInfoAt(int index) => _componentInfos[index];

        public void Refresh()
        {
            FileSize = 0;

            if (Directory.Exists(Path))
            {
                Exists = true;
                FileSize = _componentInfos.Sum(c => c.FileSize);
            }
            else
            {
                Exists = false;
            }
        }

        public void Clear() => _componentInfos.Clear();

        public static LibraryInfo Create<T>(Library<T> library) where T : IComponent
        {
            return new LibraryInfo()
            {
                Name = GetName<T>(),
                Path = library.Path,
                PreviewIcon = GetPreviewIcon<T>()
            };
        }

        public static LibraryInfo Create(LibraryNode node)
        {
            return new LibraryInfo()
            {
                Name = node.Name,
                Path = node.Path,
                PreviewIcon = LibraryNode.GetPreviewIcon()
            };
        }

        private static string GetName<T>() where T : IComponent
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

        private static BitmapSource GetPreviewIcon<T>() where T : IComponent
        {
            return new TypeSwitch<BitmapSource>()
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
