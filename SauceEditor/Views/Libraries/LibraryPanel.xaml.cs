using Xceed.Wpf.AvalonDock.Layout;

namespace SauceEditor.Views.Libraries
{
    /// <summary>
    /// Interaction logic for LibraryPanel.xaml
    /// </summary>
    public partial class LibraryPanel : LayoutAnchorable
    {
        public LibraryPanel()
        {
            InitializeComponent();
            /*ViewModel.UpdateFromModel(new SauceEditorCore.Models.Libraries.LibraryManager()
            {
                Path = FilePathHelper.RESOURCES_DIRECTORY
            });*/
        }
    }
}
