using SauceEditor.Helpers;
using SauceEditor.ViewModels.Docks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.AvalonDock.Layout;

namespace SauceEditor.Views.Libraries
{
    /// <summary>
    /// Interaction logic for LibraryPanel.xaml
    /// </summary>
    public partial class LibraryPanel : LayoutAnchorable, IHaveDockViewModel
    {
        public LibraryPanel()
        {
            InitializeComponent();
            ViewModel.UpdateFromModel(new SauceEditorCore.Models.Libraries.LibraryManager()
            {
                Path = FilePathHelper.RESOURCES_DIRECTORY
            });
        }

        public DockViewModel GetViewModel() => ViewModel;
    }
}
