using SauceEditor.ViewModels.Docks;

namespace SauceEditor.Views.Docks
{
    public interface IDockTracker
    {
        DockableViewModel ActiveLeftDockVM { get; set; }
        DockableViewModel ActiveCenterDockVM { get; set; }
        DockableViewModel ActiveRightDockVM { get; set; }
    }
}
