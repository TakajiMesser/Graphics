using SauceEditorCore.Models.Libraries;
using System;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace SauceEditor.Views.Tools
{
    /// <summary>
    /// Interaction logic for LibraryPage.xaml
    /// </summary>
    public partial class LibraryPage : LayoutAnchorable
    {
        public LibraryPage(LibraryManager libraryManager)
        {
            InitializeComponent();
        }

        public LibraryPage(Library library)
        {
            InitializeComponent();
        }
    }
}
