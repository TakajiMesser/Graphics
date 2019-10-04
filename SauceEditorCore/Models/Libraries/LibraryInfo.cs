using SauceEditorCore.Models.Components;
using SpiceEngineCore.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SauceEditorCore.Models.Libraries
{
    public class LibraryInfo : IPathInfo 
    {
        private List<IPathInfo> _pathInfos = new List<IPathInfo>();

        private LibraryInfo() { }

        public string Name { get; private set; }
        public string Path { get; private set; }

        public bool Exists { get; private set; }
        public long FileSize { get; private set; }

        public DateTime? CreationTime { get; private set; }
        public DateTime? LastWriteTime { get; private set; }
        public DateTime? LastAccessTime { get; private set; }

        public byte[] PreviewBitmap { get; private set; }

        public void AddPathInfo(IPathInfo pathInfo) => _pathInfos.Add(pathInfo);

        public int Count => _pathInfos.Count;

        public IPathInfo GetInfoAt(int index) => _pathInfos[index];

        public void Refresh()
        {
            var directoryInfo = new DirectoryInfo(Path);

            if (directoryInfo.Exists)
            {
                Exists = true;
                FileSize = _pathInfos.Sum(c => c.FileSize);
                CreationTime = directoryInfo.CreationTime;
                LastWriteTime = directoryInfo.LastWriteTime;
                LastAccessTime = directoryInfo.LastAccessTime;
            }
            else
            {
                Exists = false;
                FileSize = 0;
                CreationTime = null;
                LastWriteTime = null;
                LastAccessTime = null;
            }
        }

        public void Clear() => _pathInfos.Clear();

        public static LibraryInfo Create<T>(Library<T> library) where T : IComponent
        {
            return new LibraryInfo()
            {
                Name = GetName<T>(),
                Path = library.Path,
                PreviewBitmap = GetPreviewIcon<T>()
            };
        }

        public static LibraryInfo Create(LibraryNode node)
        {
            return new LibraryInfo()
            {
                Name = node.Name,
                Path = node.Path,
                PreviewBitmap = LibraryNode.GetPreviewBitmap()
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

        private static byte[] GetPreviewIcon<T>() where T : IComponent
        {
            return new TypeSwitch<byte[]>()
                /*.Case<MapComponent>(() => "Maps")
                .Case<ModelComponent>(() => "Models")
                .Case<BehaviorComponent>(() => "Behaviors")
                .Case<TextureComponent>(() => "Textures")
                .Case<SoundComponent>(() => "Sounds")
                .Case<MaterialComponent>(() => "Materials")
                .Case<ArchetypeComponent>(() => "Archetypes")
                .Case<ScriptComponent>(() => "Scripts")*/
                .Default(() => new byte[0])//throw new NotImplementedException())
                .Match<T>();
        }
    }
}
