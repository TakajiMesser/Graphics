using SauceEditorCore.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SauceEditorCore.Models.Libraries
{
    public interface IPathInfo
    {
        string Name { get; }
        string Path { get; }
        bool Exists { get; }
        long FileSize { get; }
        BitmapImage PreviewIcon { get; }

        void Refresh();
    }
}
