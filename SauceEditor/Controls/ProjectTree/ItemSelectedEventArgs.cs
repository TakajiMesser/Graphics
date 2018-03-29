using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceEditor.Controls.ProjectTree
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
