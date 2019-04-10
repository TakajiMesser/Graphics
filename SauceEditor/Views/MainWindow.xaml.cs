using Microsoft.Win32;
using OpenTK;
using SampleGameProject.Helpers;
using SauceEditor.Models;
using SauceEditor.ViewModels.Commands;
using SauceEditor.Views.Behaviors;
using SauceEditor.Views.GamePanels;
using SauceEditor.Views.ProjectTree;
using SauceEditor.Views.Properties;
using SauceEditor.Views.Scripts;
using SauceEditor.Views.Settings;
using SauceEditor.Views.Tools;
using SpiceEngine.Entities.Actors;
using SpiceEngine.Maps;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Xceed.Wpf.AvalonDock.Layout;
using GameWindow = SpiceEngine.Game.GameWindow;
using ICommand = SauceEditor.ViewModels.Commands.ICommand;

namespace SauceEditor.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CommandStack CommandStack { get; private set; } = new CommandStack();

        private Map _map;
        private string _mapPath;
        
        // Side panels
        private ProjectTreePanel _projectTree = new ProjectTreePanel();
        private ToolsPanel _toolPanel = new ToolsPanel();
        private PropertyPanel _propertyPanel = new PropertyPanel();

        // Main views
        private GamePanelManager _gamePanelManager;
        private BehaviorView _behaviorView;
        private ScriptView _scriptView;

        // Separate windows
        private GameWindow _gameWindow;
        private SettingsWindow _settingsWindow;
        private EditorSettings _settings;

        public MainWindow()
        {
            CreateTestProject();
            LoadOrCreateSettings();

            PresentationTraceSources.DataBindingSource.Listeners.Add(new ConsoleTraceListener());
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Error;

            InitializeComponent();

            ViewModel.PropertiesViewModel = _propertyPanel.ViewModel;
            ViewModel.EntityUpdated += (s, args) => _gamePanelManager.RequestUpdate();
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

        private void LoadOrCreateSettings()
        {
            if (File.Exists(SauceEditor.Helpers.FilePathHelper.SETTINGS_PATH))
            {
                _settings = EditorSettings.Load(SauceEditor.Helpers.FilePathHelper.SETTINGS_PATH);
            }
            else
            {
                _settings = new EditorSettings();
                _settings.Save(SauceEditor.Helpers.FilePathHelper.SETTINGS_PATH);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _gameWindow?.Close();
            base.OnClosing(e);
            Application.Current.Shutdown();
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            _projectTree.MapSelected += (s, args) => OpenMap(args.FilePath);
            _projectTree.ModelSelected += (s, args) => OpenModel(args.FilePath);
            _projectTree.BehaviorSelected += (s, args) => OpenBehavior(args.FilePath);
            _projectTree.TextureSelected += (s, args) => OpenTexture(args.FilePath);
            _projectTree.AudioSelected += (s, args) => OpenAudio(args.FilePath);

            _toolPanel.ToolSelected += ToolPanel_ToolSelected;

            DockHelper.AddToDockAsDocument(SideDockingManager, _projectTree);
            DockHelper.AddToDockAsDocument(SideDockingManager, _toolPanel);
            DockHelper.AddToDockAsDocument(SideDockingManager, _propertyPanel);

            _projectTree.IsActive = true;
            //SideDockManager.ActiveContent = _projectTree;
        }

        private void ToolPanel_ToolSelected(object sender, ToolSelectedEventArgs e)
        {
            _gamePanelManager?.SetSelectedTool(e.NewTool);
        }

        private void OnClosing(object sender, EventArgs e)
        {
            //Properties.Settings.Default.DockingLayoutState = DockManager.GetLayoutAsXml();
            //Properties.Settings.Default.Save();
        }

        /*private bool ContainsDocument(string title)
        {
            foreach (DockingLibrary.DocumentContent doc in MainDockManager.Documents)
            {
                if (string.Compare(doc.Title, title, true) == 0)
                {
                    return true;
                }
            }
                
            return false;
        }*/

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            /*var titleBuilder = new StringBuilder("Document");

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

            //files.Add(new RecentFile(doc.Title, "PATH" + doc.Title, doc.Title.Length * i));*/
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

            _gameWindow = new GameWindow(_gamePanelManager.Map)
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
            //_gameWindow.LoadMap(_gamePanelManager.Map);
        }

        private void Settings_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void Settings_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _settingsWindow = new SettingsWindow(_settings);
            _settingsWindow.SettingsChanged += (s, args) =>
            {
                _settings = args.Settings;
                _settings.Save(SauceEditor.Helpers.FilePathHelper.SETTINGS_PATH);
            };
            _settingsWindow.Show();
        }

        private void Menu_MapOpened(object sender, FileEventArgs e) => OpenMap(e.FileName);

        private void Menu_ProjectOpened(object sender, FileEventArgs e)
        {
            _projectTree.OpenProject(e.FileName);
            _projectTree.IsActive = true;
            //SideDockManager.ActiveContent = _projectTree;
            Title = System.IO.Path.GetFileNameWithoutExtension(e.FileName) + " - " + "SauceEditor";
        }

        private void Menu_ModelOpened(object sender, FileEventArgs e)
        {
            
        }

        private void Menu_BehaviorOpened(object sender, FileEventArgs e)
        {
            Title = System.IO.Path.GetFileNameWithoutExtension(e.FileName) + " - " + "SauceEditor";
        }

        private void Menu_UndoCalled(object sender, EventArgs e) => CommandStack.Undo();

        private void Menu_RedoCalled(object sender, EventArgs e) => CommandStack.Redo();

        private void UndoButton_Click(object sender, RoutedEventArgs e) => CommandStack.Undo();

        private void RedoButton_Click(object sender, RoutedEventArgs e) => CommandStack.Redo();

        private void CommandExecuted(ICommand command)
        {
            CommandStack.Push(command);

            // TODO - Also need to enable/disable menu items for undo/redo
            if (CommandStack.CanUndo)
            {
                UndoButton.IsEnabled = true;
            }
            else
            {
                UndoButton.IsEnabled = false;
            }

            if (CommandStack.CanRedo)
            {
                RedoButton.IsEnabled = true;
            }
            else
            {
                RedoButton.IsEnabled = false;
            }
        }

        private void OpenMap(string filePath)
        {
            Title = System.IO.Path.GetFileNameWithoutExtension(filePath) + " - " + "SauceEditor";

            _map = Map.Load(filePath);
            _mapPath = filePath;

            PlayButton.Visibility = Visibility.Visible;
            _gamePanelManager = new GamePanelManager(_mapPath);
            _gamePanelManager.EntitySelectionChanged += (s, args) =>
            {
                // For now, just go with the last entity that was selected
                //_propertyPanel.Entity = args.Entities.LastOrDefault();
                _propertyPanel.ViewModel.UpdateFromModel(args.Entities.LastOrDefault());
                _propertyPanel.IsActive = true;
                //SideDockManager.ActiveContent = _propertyPanel;
            };
            _gamePanelManager.CommandExecuted += (s, args) => CommandExecuted(args.Command);

            DockHelper.AddToDockAsAnchorableDocument(MainDockingManager, _gamePanelManager, Path.GetFileNameWithoutExtension(_filePath), () =>
            {
                PlayButton.Visibility = Visibility.Hidden;
                _map = null;
            });

            _gamePanelManager.SetView(_settings.DefaultView);

            _propertyPanel.EntityUpdated += (s, args) => _gamePanelManager.UpdateEntity(args.Entity);

            _propertyPanel.ScriptOpened += (s, args) =>
            {
                if (_propertyPanel.ViewModel.EditorEntity != null && _propertyPanel.ViewModel.EditorEntity.Entity is Actor actor && _propertyPanel.ViewModel.EditorEntity.MapEntity is MapActor mapActor)
                {
                    /*_scriptView = new ScriptView(filePath, actor, mapActor);
                    _scriptView.Saved += (sender, e) =>
                    {
                        //mapActor.Behavior.e.Script;
                    };*/

                    OpenBehavior(args.FileName);
                }
                else
                {
                    // TODO - Throw some error to the user here?
                }
            };
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
            // Temporary measures...
            /*if (_scriptView == null)
            {
                _scriptView = new ScriptView(filePath);
            }

            var anchorable = new LayoutAnchorable
            {
                Title = Path.GetFileNameWithoutExtension(filePath),
                Content = _scriptView,
                CanClose = true
            };
            anchorable.Closed += (s, args) =>
            {
                PlayButton.Visibility = Visibility.Hidden;
                _map = null;
            };
            anchorable.AddToLayout(MainDockingManager, AnchorableShowStrategy.Most);
            anchorable.DockAsDocument();*/

            /* if (_behaviorView == null)
            {
                _behaviorView = new BehaviorView();
            }*/

            _behaviorView = _behaviorView ?? new BehaviorView();

            DockHelper.AddToDockAsAnchorableDocument(MainDockingManager, _behaviorView, Path.GetFileNameWithoutExtension(filePath), () =>
            {
                PlayButton.Visibility = Visibility.Hidden;
                _map = null;
            });
        }

        private void OpenTexture(string filePath)
        {

        }

        private void OpenAudio(string filePath)
        {

        }
    }
}
