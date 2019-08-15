using SauceEditorCore.Models.Libraries;
using System;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace SauceEditor.Views.Tools
{
    /// <summary>
    /// Interaction logic for LibraryPanel.xaml
    /// </summary>
    public partial class LibraryPanel : LayoutAnchorable
    {
        public LibraryPanel()
        {
            InitializeComponent();
            ViewModel.LibraryManager = new LibraryManager("");
        }
    }
}
