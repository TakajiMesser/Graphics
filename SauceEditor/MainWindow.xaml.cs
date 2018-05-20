using Jidai.Helpers;
using Microsoft.Win32;
using OpenTK;
using SauceEditor.Commands;
using SauceEditor.Controls;
using SauceEditor.Controls.GamePanels;
using SauceEditor.Controls.ProjectTree;
using SauceEditor.Controls.Properties;
using SauceEditor.Controls.Tools;
using SauceEditor.Structure;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using TakoEngine.Game;
using TakoEngine.Maps;
using GameWindow = TakoEngine.Game.GameWindow;

namespace SauceEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CommandStack CommandStack { get; set; } = new CommandStack();

        private Map _map;
        private string _mapPath;
        
        private ProjectTreeView _projectTree = new ProjectTreeView();
        private ToolsWindow _toolPanel = new ToolsWindow();
        private PropertyWindow _propertyPanel = new PropertyWindow();
        private GamePanelManager _gamePanelManager;
        private GameWindow _gameWindow;

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

            _toolPanel.DockManager = SideDockManager;
            _toolPanel.ShowAsDocument();
            _toolPanel.ToolSelected += ToolPanel_ToolSelected;

            _propertyPanel.DockManager = SideDockManager;
            _propertyPanel.ShowAsDocument();

            SideDockManager.ActiveContent = _projectTree;
        }

        private void ToolPanel_ToolSelected(object sender, ToolSelectedEventArgs e)
        {
            _gamePanelManager.SetSelectedTool(e.NewTool);
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

        private void NewMap()
        {

        }

        private void NewModel()
        {

        }

        private void NewBehavior()
        {

        }

        private void NewTexture()
        {

        }

        private void NewAudio()
        {

        }

        private void OpenMap(string filePath)
        {
            _map = Map.Load(filePath);
            _mapPath = filePath;

            PlayButton.Visibility = Visibility.Visible;
            _gamePanelManager = new GamePanelManager(MainDockManager, _mapPath);
            _gamePanelManager.Closed += (s, args) =>
            {
                PlayButton.Visibility = Visibility.Hidden;
                _map = null;
            };
            _gamePanelManager.EntitySelectionChanged += (s, args) =>
            {
                _propertyPanel.Entity = args.Entities.LastOrDefault();
                SideDockManager.ActiveContent = _propertyPanel;
            };
            _gamePanelManager.ShowAsDocument();

            _propertyPanel.EntityUpdated += (s, args) => _gamePanelManager.UpdateEntity(args.Entity);
        }

        private void OpenModel(string filePath)
        {
            PlayButton.Visibility = Visibility.Visible;

            /*var modelView = new DockableGamePanel(MainDockManager);
            modelView.Panel.LoadFromModel(filePath);
            modelView.EntitySelectionChanged += (s, args) =>
            {
                _propertyPanel.Entity = args.Entity;
                SideDockManager.ActiveContent = _propertyPanel;
            };
            modelView.Closed += (s, args) => PlayButton.Visibility = Visibility.Hidden;
            modelView.ShowAsDocument();*/

            //_propertyPanel.TransformChanged += (s, args) => modelView.Panel.Invalidate();
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

        private void NewProjectCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void NewMapCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void NewModelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void NewBehaviorCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void OpenProjectCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void OpenMapCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void OpenModelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void OpenBehaviorCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void SaveAsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void SaveAllCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
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
        }

        private void NewProjectCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new FileNameDialog();
            if (dialog.ShowDialog() == true)
            {

            }
        }

        private void NewMapCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void NewModelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void NewBehaviorCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void OpenProjectCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "Project Files|*.pro",
                DefaultExt = GameProject.FILE_EXTENSION,
                InitialDirectory = string.IsNullOrEmpty(_mapPath)
                    ? @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\TakoEngine\Jidai\Maps"
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
                Filter = "Map Files|*.map",
                InitialDirectory = string.IsNullOrEmpty(_mapPath)
                    ? @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\TakoEngine\Jidai\Maps"
                    : System.IO.Path.GetDirectoryName(_mapPath)
            };

            if (dialog.ShowDialog() == true)
            {
                _projectTree.OpenMap(dialog.FileName);
                SideDockManager.ActiveContent = _projectTree;
                Title = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName) + " - " + "SauceEditor";
            }
        }

        private void OpenModelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "obj",
                InitialDirectory = string.IsNullOrEmpty(_mapPath)
                    ? @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\TakoEngine\Jidai\Maps"
                    : System.IO.Path.GetDirectoryName(_mapPath)
            };

            if (dialog.ShowDialog() == true)
            {
                Title = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName) + " - " + "SauceEditor";
                OpenModel(dialog.FileName);
            }
        }

        private void OpenBehaviorCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "bhv",
                InitialDirectory = string.IsNullOrEmpty(_mapPath)
                    ? @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\TakoEngine\Jidai\Maps"
                    : System.IO.Path.GetDirectoryName(_mapPath)
            };

            if (dialog.ShowDialog() == true)
            {
                Title = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName) + " - " + "SauceEditor";
                OpenBehavior(dialog.FileName);
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

        private void SaveAllCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _map?.Save(_mapPath);
            _projectTree?.SaveProject();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            _gamePanelManager.IsEnabled = false;
            /*_perspectiveView.Panel.Enabled = false;
            _xView.Panel.Enabled = false;
            _yView.Panel.Enabled = false;
            _zView.Panel.Enabled = false;*/

            _gameWindow = new GameWindow(_mapPath)
            {
                VSync = VSyncMode.Adaptive
            };
            _gameWindow.Closed += (s, args) =>
            {
                _gamePanelManager.IsEnabled = true;
                /*_perspectiveView.Panel.Enabled = true;
                _xView.Panel.Enabled = true;
                _yView.Panel.Enabled = true;
                _zView.Panel.Enabled = true;*/
            };
            _gameWindow.Run(60.0, 0.0);
        }
    }
}
