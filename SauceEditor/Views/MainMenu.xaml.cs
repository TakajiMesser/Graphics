using System;
using System.Diagnostics;
using System.Windows.Controls;

namespace SauceEditor.Views
{
    public class FileEventArgs : EventArgs
    {
        public string FileName { get; private set; }

        public FileEventArgs(string fileName) => FileName = fileName;
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainMenu : Menu
    {
        public MainMenu()
        {
            PresentationTraceSources.DataBindingSource.Listeners.Add(new ConsoleTraceListener());
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Error;

            InitializeComponent();
        }
    }
}
