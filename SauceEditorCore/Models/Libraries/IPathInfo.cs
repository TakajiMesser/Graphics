using System;

namespace SauceEditorCore.Models.Libraries
{
    public interface IPathInfo
    {
        string Name { get; }
        string Path { get; }

        bool Exists { get; }
        long FileSize { get; }

        DateTime? CreationTime { get; }
        DateTime? LastWriteTime { get; }
        DateTime? LastAccessTime { get; }

        byte[] PreviewBitmap { get; }

        void Refresh();
    }
}
