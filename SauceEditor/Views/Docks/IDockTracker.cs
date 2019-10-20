using SauceEditor.ViewModels.Docks;

namespace SauceEditor.Views
{
    public interface IDockTracker
    {
        DockableViewModel ActiveLeftDockVM { get; set; }
        DockableViewModel ActiveCenterDockVM { get; set; }
        DockableViewModel ActiveRightDockVM { get; set; }
    }
}
