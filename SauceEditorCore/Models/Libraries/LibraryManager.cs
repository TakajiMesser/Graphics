using SauceEditorCore.Helpers;
using SauceEditorCore.Models.Components;
using SpiceEngineCore.Helpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace SauceEditorCore.Models.Libraries
{
    public enum PathSortStyles
    {
        Added,
        Alphabetical,
        Size,
        CreationTimes,
        WriteTimes,
        AccessTimes,
        Type
    }

    public class LibraryManager
    {
        public const string FILE_EXTENSION = "LM";

        private Dictionary<string, ComponentInfo> _componentInfoByPath = new Dictionary<string, ComponentInfo>();
        private Dictionary<string, LibraryInfo> _nodeInfoByPath = new Dictionary<string, LibraryInfo>();

        private LibraryInfo _mapLibraryInfo;
        private LibraryInfo _modelLibraryInfo;
        private LibraryInfo _behaviorLibraryInfo;
        private LibraryInfo _textureLibraryInfo;
        private LibraryInfo _soundLibraryInfo;
        private LibraryInfo _materialLibraryInfo;
        private LibraryInfo _archetypeLibraryInfo;
        private LibraryInfo _scriptLibraryInfo;

        public LibraryNode RootNode { get; private set; }

        public string Path { get; set; }

        public Library<MapComponent> MapLibrary { get; private set; }
        public Library<ModelComponent> ModelLibrary { get; private set; }
        public Library<BehaviorComponent> BehaviorLibrary { get; private set; }
        public Library<TextureComponent> TextureLibrary { get; private set; }
        public Library<SoundComponent> SoundLibrary { get; private set; }
        public Library<MaterialComponent> MaterialLibrary { get; private set; }
        public Library<ArchetypeComponent> ArchetypeLibrary { get; private set; }
        public Library<ScriptComponent> ScriptLibrary { get; private set; }

        public IEnumerable<IPathInfo> GetNodePathInfos(string path)
        {
            var nodeInfo = _nodeInfoByPath[path];

            for (var i = 0; i < nodeInfo.Count; i++)
            {
                yield return nodeInfo.GetInfoAt(i);
            }
        }

        public IEnumerable<IPathInfo> GetLibraryPaths()
        {
            yield return _mapLibraryInfo;
            yield return _modelLibraryInfo;
            yield return _behaviorLibraryInfo;
            yield return _textureLibraryInfo;
            yield return _soundLibraryInfo;
            yield return _materialLibraryInfo;
            yield return _archetypeLibraryInfo;
            yield return _scriptLibraryInfo;
        }

        public IEnumerable<IPathInfo> GetMapLibraryPaths() => GetLibraryPaths(_mapLibraryInfo);
        public IEnumerable<IPathInfo> GetModelLibraryPaths() => GetLibraryPaths(_modelLibraryInfo);
        public IEnumerable<IPathInfo> GetBehaviorLibraryPaths() => GetLibraryPaths(_behaviorLibraryInfo);
        public IEnumerable<IPathInfo> GetTextureLibraryPaths() => GetLibraryPaths(_textureLibraryInfo);
        public IEnumerable<IPathInfo> GetSoundLibraryPaths() => GetLibraryPaths(_soundLibraryInfo);
        public IEnumerable<IPathInfo> GetMaterialLibraryPaths() => GetLibraryPaths(_materialLibraryInfo);
        public IEnumerable<IPathInfo> GetArchetypeLibraryPaths() => GetLibraryPaths(_archetypeLibraryInfo);
        public IEnumerable<IPathInfo> GetScriptLibraryPaths() => GetLibraryPaths(_scriptLibraryInfo);

        private IEnumerable<IPathInfo> GetLibraryPaths(LibraryInfo libraryInfo)
        {
            for (var i = 0; i < libraryInfo.Count; i++)
            {
                yield return libraryInfo.GetInfoAt(i);
            }
        }

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
            //var lines = File.ReadAllLines(Path);

            // TODO - Should we allow for all libraries to be located in different places?
            /*MapLibrary = new Library<MapComponent>(lines[0]);
            ModelLibrary = new Library<ModelComponent>(lines[1]);
            BehaviorLibrary = new Library<BehaviorComponent>(lines[2]);
            TextureLibrary = new Library<TextureComponent>(lines[3]);
            SoundLibrary = new Library<SoundComponent>(lines[4]);
            MaterialLibrary = new Library<MaterialComponent>(lines[5]);
            ArchetypeLibrary = new Library<ArchetypeComponent>(lines[6]);
            ScriptLibrary = new Library<ScriptComponent>(lines[7]);*/

            MapLibrary = new Library<MapComponent>(Path);
            ModelLibrary = new Library<ModelComponent>(Path);
            BehaviorLibrary = new Library<BehaviorComponent>(Path);
            TextureLibrary = new Library<TextureComponent>(Path);
            SoundLibrary = new Library<SoundComponent>(Path);
            MaterialLibrary = new Library<MaterialComponent>(Path);
            ArchetypeLibrary = new Library<ArchetypeComponent>(Path);
            ScriptLibrary = new Library<ScriptComponent>(Path);

            _mapLibraryInfo = LibraryInfo.Create(MapLibrary);
            _modelLibraryInfo = LibraryInfo.Create(ModelLibrary);
            _behaviorLibraryInfo = LibraryInfo.Create(BehaviorLibrary);
            _textureLibraryInfo = LibraryInfo.Create(TextureLibrary);
            _soundLibraryInfo = LibraryInfo.Create(SoundLibrary);
            _materialLibraryInfo = LibraryInfo.Create(MaterialLibrary);
            _archetypeLibraryInfo = LibraryInfo.Create(ArchetypeLibrary);
            _scriptLibraryInfo = LibraryInfo.Create(ScriptLibrary);

            RootNode = new LibraryNode(Path); 

            var rootNodeInfo = LibraryInfo.Create(RootNode);
            _nodeInfoByPath.Add(rootNodeInfo.Path, rootNodeInfo);

            SearchForComponents(Path, RootNode, rootNodeInfo);
        }

        private void SearchForComponents(string path, LibraryNode node, LibraryInfo nodeInfo)
        {
            foreach (var filePath in Directory.GetFiles(path))
            {
                var extension = System.IO.Path.GetExtension(filePath);

                AddComponent<MapComponent>(node, nodeInfo, filePath, extension);
                AddComponent<ModelComponent>(node, nodeInfo, filePath, extension);
                AddComponent<BehaviorComponent>(node, nodeInfo, filePath, extension);
                AddComponent<TextureComponent>(node, nodeInfo, filePath, extension);
                AddComponent<SoundComponent>(node, nodeInfo, filePath, extension);
                AddComponent<MaterialComponent>(node, nodeInfo, filePath, extension);
                AddComponent<ArchetypeComponent>(node, nodeInfo, filePath, extension);
                AddComponent<ScriptComponent>(node, nodeInfo, filePath, extension);
            }

            foreach (var directoryPath in Directory.GetDirectories(path))
            {
                var childNode = new LibraryNode(directoryPath);
                node.AddChild(childNode);

                var childNodeInfo = LibraryInfo.Create(childNode);
                nodeInfo.AddPathInfo(childNodeInfo);
                _nodeInfoByPath.Add(childNodeInfo.Path, childNodeInfo);

                SearchForComponents(directoryPath, childNode, childNodeInfo);
            }
        }

        private void AddComponent<T>(LibraryNode node, LibraryInfo nodeInfo, string filePath, string extension) where T : IComponent
        {
            if (ComponentFactory.IsValidExtension<T>(extension))
            {
                var component = ComponentFactory.Create<T>(filePath);
                var library = GetLibrary<T>();
                var libraryInfo = GetLibraryInfo<T>();

                var componentInfo = ComponentInfo.Create(component);
                nodeInfo.AddPathInfo(componentInfo);
                _componentInfoByPath.Add(filePath, componentInfo);

                library.AddComponent(component);
                libraryInfo.AddPathInfo(componentInfo);
                node.Components.Add(component);
            }
        }

        private Library<T> GetLibrary<T>() where T : IComponent
        {
            return (Library<T>)new TypeSwitch<ILibrary>()
                .Case<MapComponent>(() => MapLibrary)
                .Case<ModelComponent>(() => ModelLibrary)
                .Case<BehaviorComponent>(() => BehaviorLibrary)
                .Case<TextureComponent>(() => TextureLibrary)
                .Case<SoundComponent>(() => SoundLibrary)
                .Case<MaterialComponent>(() => MaterialLibrary)
                .Case<ArchetypeComponent>(() => ArchetypeLibrary)
                .Case<ScriptComponent>(() => ScriptLibrary)
                .Default(() => throw new NotImplementedException())
                .Match<T>();
        }

        private LibraryInfo GetLibraryInfo<T>() where T : IComponent
        {
            return new TypeSwitch<LibraryInfo>()
                .Case<MapComponent>(() => _mapLibraryInfo)
                .Case<ModelComponent>(() => _modelLibraryInfo)
                .Case<BehaviorComponent>(() => _behaviorLibraryInfo)
                .Case<TextureComponent>(() => _textureLibraryInfo)
                .Case<SoundComponent>(() => _soundLibraryInfo)
                .Case<MaterialComponent>(() => _materialLibraryInfo)
                .Case<ArchetypeComponent>(() => _archetypeLibraryInfo)
                .Case<ScriptComponent>(() => _scriptLibraryInfo)
                .Default(() => throw new NotImplementedException())
                .Match<T>();
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
