using System;

namespace SauceEditor.Views.Controls.ProjectTree
{
    public class ItemSelectedEventArgs : EventArgs
    {
        public string FilePath { get; private set; }

        public ItemSelectedEventArgs(string filePath)
        {
            FilePath = filePath;
        }
    }
}
