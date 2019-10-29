using SpiceEngineCore.Rendering.Models;
using System.Windows;

namespace SauceEditor.Views.GamePanels
{
    public interface IPosition
    {
        void Position(ModelMesh modelMesh, DragEventArgs args);
    }
}
