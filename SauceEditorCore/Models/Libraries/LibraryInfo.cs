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

        public LibraryInfo(ILibrary library)
        {
            Name = library.Name;
            Path = library.Path;
            //PreviewBitmap = GetPreviewIcon<T>()
        }

        public LibraryInfo(LibraryNode node)
        {
            Name = node.Name;
            Path = node.Path;
            PreviewBitmap = LibraryNode.GetPreviewBitmap();
        }

        public IEnumerable<IPathInfo> Items => _pathInfos;

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
    }
}
