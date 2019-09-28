using SauceEditor.ViewModels;
using SauceEditor.ViewModels.Docks;
using System.Collections.Generic;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;

namespace SauceEditor.Views
{
    public interface ITrackDocks
    {
        MainWindowViewModel MainWindowVM { get; set; }
        DockViewModel ActiveGameDockVM { get; set; }
        DockViewModel ActivePropertyDockVM { get; set; }
        DockViewModel ActiveToolDockVM { get; set; }
    }
}
