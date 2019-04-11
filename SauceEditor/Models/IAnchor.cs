using SpiceEngine.Entities;
using SpiceEngine.Maps;
using Xceed.Wpf.AvalonDock.Layout;

namespace SauceEditor.Models
{
    public interface IAnchor
    {
        LayoutAnchorable Anchor { get; }
    }
}
