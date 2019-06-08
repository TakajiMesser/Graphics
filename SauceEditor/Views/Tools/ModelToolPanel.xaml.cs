using SauceEditor.ViewModels.Docks;
using Xceed.Wpf.AvalonDock.Layout;

namespace SauceEditor.Views.Tools
{
    /// <summary>
    /// Interaction logic for ModelToolPanel.xaml
    /// </summary>
    public partial class ModelToolPanel : LayoutAnchorable, IHaveDockViewModel
    {
        public ModelToolPanel()
        {
            InitializeComponent();
        }

        public DockViewModel GetViewModel() => ViewModel;
    }
}
