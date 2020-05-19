using System;
using System.Collections.Generic;

namespace SauceEditor.ViewModels.Libraries
{
    public class PathInfoEventArgs : EventArgs
    {
        public IEnumerable<PathInfoViewModel> Items { get; }

        public PathInfoEventArgs(IEnumerable<PathInfoViewModel> items) => Items = items;
    }
}