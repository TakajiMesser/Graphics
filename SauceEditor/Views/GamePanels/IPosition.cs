using SpiceEngine.Rendering.Meshes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SauceEditor.Views.GamePanels
{
    public interface IPosition
    {
        void Position(ModelMesh modelMesh, DragEventArgs args);
    }
}
