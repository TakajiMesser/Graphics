using System;

namespace SauceEditor.Views.Trees.Projects
{
    public class ItemSelectedEventArgs : EventArgs
    {
        public string FilePath { get; private set; }

        public ItemSelectedEventArgs(string filePath) => FilePath = filePath;
    }
}
