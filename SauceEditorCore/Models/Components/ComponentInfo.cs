using System;
using System.IO;

namespace SauceEditorCore.Models.Components
{
    public class ComponentInfo
    {
        public string Name { get; }
        public string Path { get; }

        public bool Exists { get; private set; }
        public long FileSize { get; private set; }
        
        public DateTime? CreationTime { get; private set; }
        public DateTime? LastWriteTime { get; private set; }
        public DateTime? LastAccessTime { get; private set; }

        public ComponentInfo(IComponent component)
        {
            Name = component.Name;
            Path = component.Path;
        }

        public void Refresh()
        {
            var fileInfo = new FileInfo(Path);

            if (fileInfo.Exists)
            {
                Exists = true;
                FileSize = fileInfo.Length;
                CreationTime = fileInfo.CreationTime;
                LastWriteTime = fileInfo.LastWriteTime;
                LastAccessTime = fileInfo.LastAccessTime;
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
    }
}
