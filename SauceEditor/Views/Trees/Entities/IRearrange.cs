using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SauceEditor.Views.Trees.Entities
{
    public interface IRearrange
    {
        void Rearrange(string name, DragEventArgs args);
    }
}
