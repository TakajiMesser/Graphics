using System.Windows;

namespace SauceEditor.Views.GamePanels
{
    public interface IDragPosition
    {
        Point Position(DragEventArgs args);
    }
}
