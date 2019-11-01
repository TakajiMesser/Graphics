using SpiceEngineCore.Rendering.Models;
using System.Windows;

namespace SauceEditor.Views.GamePanels
{
    public interface IDragPosition
    {
        System.Drawing.Point Position(DragEventArgs args);
    }
}
