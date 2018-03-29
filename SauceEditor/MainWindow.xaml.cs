using GraphicsTest.Helpers;
using Microsoft.Win32;
using OpenTK;
using SauceEditor.Controls;
using SauceEditor.Controls.ProjectTree;
using SauceEditor.Structure;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using TakoEngine.Maps;
using GameWindow = TakoEngine.Game.GameWindow;

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
        private PropertyWindow _propertyPanel = new PropertyWindow();
        private DockableGamePanel _perspectiveView;
        //private DocWindowCollection _docWindows;

        public MainWindow()
        {
            CreateTestProject();

            PresentationTraceSources.DataBindingSource.Listeners.Add(new ConsoleTraceListener());
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Error;

            InitializeComponent();
        }

        private void CreateTestProject()
        {
            var gameProject = new GameProject()
            {
                Name = "MyFirstProject"
            };
            gameProject.MapPaths.Add(FilePathHelper.MAP_PATH);
            gameProject.ModelPaths.Add(FilePathHelper.BOB_LAMP_MESH_PATH);
            gameProject.Save(Path.GetDirectoryName(FilePathHelper.MAP_PATH) + "\\" + gameProject.Name + GameProject.FILE_EXTENSION);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _gameWindow?.Close();
            base.OnClosing(e);
            Application.Current.Shutdown();
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            MainDockManager.ParentWindow = this;
            SideDockManager.ParentWindow = this;

            _projectTree.DockManager = SideDockManager;
            _projectTree.ShowAsDocument();
            _projectTree.MapSelected += (s, args) => OpenMap(args.FilePath);
            _projectTree.ModelSelected += (s, args) => OpenModel(args.FilePath);
            _projectTree.BehaviorSelected += (s, args) => OpenBehavior(args.FilePath);
            _projectTree.TextureSelected += (s, args) => OpenTexture(args.FilePath);
            _projectTree.AudioSelected += (s, args) => OpenAudio(args.FilePath);

            _propertyPanel.DockManager = SideDockManager;
            //_propertyPanel.Show();
            _propertyPanel.ShowAsDocument();

            SideDockManager.ActiveContent = _projectTree;
        }

        private void OnClosing(object sender, EventArgs e)
        {
            //Properties.Settings.Default.DockingLayoutState = DockManager.GetLayoutAsXml();
            //Properties.Settings.Default.Save();
        }

        private bool ContainsDocument(string title)
        {
            foreach (DockingLibrary.DocumentContent doc in MainDockManager.Documents)
            {
                if (string.Compare(doc.Title, title, true) == 0)
                {
                    return true;
                }
            }
                
            return false;
        }

        private void OpenMap(string filePath)
        {
            _map = Map.Load(filePath);

            _mapPath = filePath;
            //RunButton.IsEnabled = true;
            //Title = System.IO.Path.GetFileNameWithoutExtension(filePath) + " - " + "SauceEditor";

            PlayButton.Visibility = Visibility.Visible;

            _perspectiveView = new DockableGamePanel(MainDockManager);

            _perspectiveView.GamePanel.LoadFromMap(_mapPath);
            //_perspectiveView.GamePanel.LoadFromModel(dialog.FileName);

            _perspectiveView.ShowAsDocument();
            _perspectiveView.EntitySelectionChanged += (s, args) =>
            {
                _propertyPanel.Entity = args.Entity;
                SideDockManager.ActiveContent = _propertyPanel;
            };
            _perspectiveView.Closed += (s, args) => PlayButton.Visibility = Visibility.Hidden;

            _propertyPanel.TransformChanged += (s, args) => _perspectiveView.GamePanel.Invalidate();
        }

        private void OpenModel(string filePath)
        {
            PlayButton.Visibility = Visibility.Visible;

            var modelView = new DockableGamePanel(MainDockManager);
            modelView.GamePanel.LoadFromModel(filePath);
            //_perspectiveView.GamePanel.LoadFromModel(dialog.FileName);
            modelView.ShowAsDocument();
            modelView.EntitySelectionChanged += (s, args) =>
            {
                _propertyPanel.Entity = args.Entity;
                SideDockManager.ActiveContent = _propertyPanel;
            };
            modelView.Closed += (s, args) => PlayButton.Visibility = Visibility.Hidden;

            _propertyPanel.TransformChanged += (s, args) => modelView.GamePanel.Invalidate();
        }

        private void OpenBehavior(string filePath)
        {

        }

        private void OpenTexture(string filePath)
        {

        }

        private void OpenAudio(string filePath)
        {

        }

        private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void OpenProjectCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void OpenMapCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void OpenModelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void SaveAsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

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
                DockManager = MainDockManager,
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

        private void OpenProjectCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = GameProject.FILE_EXTENSION,
                InitialDirectory = string.IsNullOrEmpty(_mapPath)
                    ? @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\TakoEngine\GraphicsTest\Maps"
                    : System.IO.Path.GetDirectoryName(_mapPath)
            };

            if (dialog.ShowDialog() == true)
            {
                _projectTree.OpenProject(dialog.FileName);
                SideDockManager.ActiveContent = _projectTree;
                Title = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName) + " - " + "SauceEditor";
            }
        }

        private void OpenMapCommand_Executed(object sender, ExecutedRoutedEventArgs e)
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
                Title = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName) + " - " + "SauceEditor";
                OpenMap(dialog.FileName);
            }
        }

        private void OpenModelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
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
                Title = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName) + " - " + "SauceEditor";
                OpenModel(dialog.FileName);
            }
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _map.Save(_mapPath);
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

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            _gameWindow = new GameWindow(_mapPath)
            {
                VSync = VSyncMode.Adaptive
            };
            _gameWindow.Run(60.0, 0.0);
        }
    }
}
