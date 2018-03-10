using TakoEngine.Maps;
using Microsoft.Win32;
using OpenTK;
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
using System.ComponentModel;
using GameWindow = TakoEngine.GameObjects.GameWindow;
using System.Diagnostics;
using SauceEditor.Controls;

namespace SauceEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Map _map;
        private string _mapPath;
        private GameWindow _gameWindow;
        private ProjectTreeView _projectTree = new ProjectTreeView();
        private PropertyPanel _propertyPanel = new PropertyPanel();
        private DockableGamePanel _perspectiveView;
        //private DocWindowCollection _docWindows;

        public MainWindow()
        {
            PresentationTraceSources.DataBindingSource.Listeners.Add(new ConsoleTraceListener());
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Error;

            InitializeComponent();

            /*var item = new TreeViewItem()
            {
                Header = "Project"
            };
            ProjectTree.Add(item);*/
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            DockManager.ParentWindow = this;
            DockManager2.ParentWindow = this;

            _projectTree.DockManager = DockManager2;
            //_projectTree.Show();
            _projectTree.ShowAsDocument();

            _propertyPanel.DockManager = DockManager2;
            //_propertyPanel.Show();
            _propertyPanel.ShowAsDocument();

            NewCommand_Executed(this, null);

            /*_docWindows = new DocWindowCollection()
            {
                DockManager = new DockingLibrary.DockManager()//DockManager
            };*/
        }

        private void OnClosing(object sender, EventArgs e)
        {
            //Properties.Settings.Default.DockingLayoutState = DockManager.GetLayoutAsXml();
            //Properties.Settings.Default.Save();
        }

        private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private bool ContainsDocument(string title)
        {
            foreach (DockingLibrary.DocumentContent doc in DockManager.Documents)
            {
                if (string.Compare(doc.Title, title, true) == 0)
                {
                    return true;
                }
            }
                
            return false;
        }

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //< local:DocWindowCollection DockPanel.Dock = "Top" x: Name = "DocWindows" />
            //_docWindows.AddDocument();
            var titleBuilder = new StringBuilder("Document");

            int i = 1;
            while (ContainsDocument(titleBuilder.ToString() + i))
            {
                i++;
            }
            titleBuilder.Append(i);

            var doc = new DocWindow()
            {
                DockManager = DockManager,
                Title = titleBuilder.ToString()
            };
            doc.ShowAsDocument();

            //files.Add(new RecentFile(doc.Title, "PATH" + doc.Title, doc.Title.Length * i));

            /*var dialog = new OpenFileDialog()
            {
                CheckPathExists = true,
                DefaultExt = "map"
            };

            if (dialog.ShowDialog() == true)
            {
                _map = new Map();
                _map.Save(dialog.FileName);

                _mapPath = dialog.FileName;
                //RunButton.IsEnabled = true;
                Title = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName) + " - " + "SauceEditor";
            }*/
        }

        private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "map",
                InitialDirectory = string.IsNullOrEmpty(_mapPath)
                    ? @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\TakoEngine\GraphicsTest\Maps"
                    : System.IO.Path.GetDirectoryName(_mapPath)
            };

            if (dialog.ShowDialog() == true)
            {
                _map = Map.Load(dialog.FileName);

                _mapPath = dialog.FileName;
                //RunButton.IsEnabled = true;
                Title = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName) + " - " + "SauceEditor";

                _perspectiveView = new DockableGamePanel(_mapPath, DockManager);
                _perspectiveView.ShowAsDocument();
            }
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _map.Save(_mapPath);
        }

        private void SaveAsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "map",
                InitialDirectory = System.IO.Path.GetDirectoryName(_mapPath)
            };

            if (dialog.ShowDialog() == true)
            {
                _map.Save(dialog.FileName);

                _mapPath = dialog.FileName;
                //RunButton.IsEnabled = true;
                Title = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName) + " - " + "SauceEditor";
            }
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            _gameWindow = new GameWindow(_mapPath)
            {
                VSync = VSyncMode.Adaptive
            };
            _gameWindow.Run(60.0, 0.0);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _gameWindow?.Close();
            base.OnClosing(e);
            Application.Current.Shutdown();
        }
    }
}
