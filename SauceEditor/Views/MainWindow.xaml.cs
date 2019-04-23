using OpenTK;
using SauceEditor.Helpers;
using SauceEditor.Helpers.Builders;
using SauceEditor.Models;
using SauceEditor.ViewModels;
using SauceEditor.Views.Behaviors;
using SauceEditor.Views.Factories;
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
using GameWindow = SpiceEngine.Game.GameWindow;
using Map = SauceEditor.Models.Components.Map;

namespace SauceEditor.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainViewFactory, IWindowFactory, IComponentFactory 
    {
        // Main views
        private GamePanelManager _gamePanelManager;
        private BehaviorView _behaviorView;
        private ScriptView _scriptView;

        // Side panels
        private ProjectTreePanel _projectTree = new ProjectTreePanel();
        private ToolsPanel _toolPanel = new ToolsPanel();
        private PropertyPanel _propertyPanel = new PropertyPanel();

        // Separate windows
        private GameWindow _gameWindow;
        private SettingsWindow _settingsWindow;

        public MainWindow()
        {
            PresentationTraceSources.DataBindingSource.Listeners.Add(new ConsoleTraceListener());
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Error;
            InitializeComponent();

            Menu.ViewModel.MainViewFactory = this;
            Menu.ViewModel.WindowFactory = this;
            Menu.ViewModel.ComponentFactory = this;

            ViewModel.PropertiesViewModel = _propertyPanel.ViewModel;
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
            _projectTree.AudioSelected += (s, args) => OpenSound(args.FilePath);

            _toolPanel.ToolSelected += ToolPanel_ToolSelected;

            //ViewModel.SideDockViewModels.Add(_projectTree.ViewModel);
            //ViewModel.SideDockViewModels.Add(_toolPanel.ViewModel);
            ViewModel.SideDockViewModels.Add(_propertyPanel.ViewModel);

            DockHelper.AddToDockAsDocument(SideDockingManager, _projectTree);
            DockHelper.AddToDockAsDocument(SideDockingManager, _toolPanel);
            DockHelper.AddToDockAsDocument(SideDockingManager, _propertyPanel);

            _projectTree.IsActive = true;
            //SideDockManager.ActiveContent = _projectTree;
        }

        private void ToolPanel_ToolSelected(object sender, ToolSelectedEventArgs e)
        {
            _gamePanelManager?.ViewModel.SetSelectedTool(e.NewTool);
        }

        private void OnClosing(object sender, EventArgs e)
        {
            /*Properties.Settings.Default.DockingLayoutState = DockManager.GetLayoutAsXml();
            Properties.Settings.Default.Save();*/
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //_map.Save(_mapPath);
        }

        private void SaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            /*var dialog = new OpenFileDialog()
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
            }*/
        }

        public void CreateGameWindow(Map map)
        {
            //_gamePanelManager.IsEnabled = false;

            var gameWindow = new GameWindow(map.GameMap)
            {
                VSync = VSyncMode.Adaptive
            };
            //_gameWindow.Closed += (s, args) => _gamePanelManager.IsEnabled = true;
            gameWindow.Run(60.0, 0.0);
        }

        public void CreateSettingsWindow()
        {
            _settingsWindow = new SettingsWindow(ViewModel.Settings);
            _settingsWindow.SettingsChanged += (s, args) =>
            {
                ViewModel.Settings = args.Settings;
                ViewModel.Settings.Save(SauceEditor.Helpers.FilePathHelper.SETTINGS_PATH);
            };
            _settingsWindow.Show();
        }

        public ViewModel CreateGamePanelView(Map map)
        {
            var gamePanelManager = new GamePanelManager();
            gamePanelManager.EntitySelectionChanged += (s, args) =>
            {
                // For now, just go with the last entity that was selected
                //_propertyPanel.Entity = args.Entities.LastOrDefault();
                _propertyPanel.ViewModel.UpdateFromModel(args.Entities.LastOrDefault());
                _propertyPanel.IsActive = true;
                //SideDockManager.ActiveContent = _propertyPanel;
            };
            //gamePanelManager.CommandExecuted += (s, args) => CommandExecuted(args.Command);
            gamePanelManager.Open(map.GameMap);

            var title = "";// Path.GetFileNameWithoutExtension(filePath);
            DockHelper.AddToDockAsAnchorableDocument(MainDockingManager, gamePanelManager, title, () =>
            {
                ViewModel.PlayVisibility = Visibility.Hidden;
            });

            gamePanelManager.SetView(ViewModel.Settings.DefaultView);
            return gamePanelManager.ViewModel;
        }

        public void SaveAll()
        {
            //_map?.Save(_mapPath);
            _projectTree?.SaveProject();
        }

        public void CreateProject()
        {

        }

        public void CreateMap()
        {

        }

        public void CreateModel()
        {

        }

        public void CreateBehavior()
        {

        }

        public void CreateTexture()
        {

        }

        public void CreateSound()
        {

        }

        public void CreateMaterial()
        {

        }

        public void CreateArchetype()
        {

        }

        public void CreateScript()
        {

        }

        public void OpenProject(string filePath)
        {
            _projectTree.OpenProject(filePath);
            _projectTree.IsActive = true;
            //SideDockManager.ActiveContent = _projectTree;
            Title = Path.GetFileNameWithoutExtension(filePath) + " - " + "SauceEditor";
        }

        public void OpenMap(string filePath)
        {
            var map = new SauceEditor.Models.Components.Map()
            {
                Name = "Test Map",
                Path = filePath,
                GameMap = SpiceEngine.Maps.Map.Load(filePath)
            };

            ViewModel.GamePanelManagerViewModel = (GamePanelManagerViewModel)CreateGamePanelView(map);

            _propertyPanel.EntityUpdated += (s, args) => _gamePanelManager.ViewModel.UpdateEntity(args.Entity);
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

        public void OpenModel(string filePath)
        {
            Title = Path.GetFileNameWithoutExtension(filePath) + " - " + "SauceEditor";

            var map = MapBuilder.GenerateModelMap(filePath);

            //_map = Map.Load(filePath);
            //_mapPath = filePath;

            //PlayButton.Visibility = Visibility.Visible;
            _gamePanelManager = new GamePanelManager();
            _gamePanelManager.EntitySelectionChanged += (s, args) =>
            {
                // For now, just go with the last entity that was selected
                //_propertyPanel.Entity = args.Entities.LastOrDefault();
                _propertyPanel.ViewModel.UpdateFromModel(args.Entities.LastOrDefault());
                _propertyPanel.IsActive = true;
                //SideDockManager.ActiveContent = _propertyPanel;
            };
            //_gamePanelManager.CommandExecuted += (s, args) => CommandExecuted(args.Command);
            _gamePanelManager.Open(map);

            DockHelper.AddToDockAsAnchorableDocument(MainDockingManager, _gamePanelManager, Path.GetFileNameWithoutExtension(filePath), () => /*_map = null*/{ });

            _gamePanelManager.SetView(ViewTypes.Perspective);

            /*_propertyPanel.EntityUpdated += (s, args) => _gamePanelManager.ViewModel.UpdateEntity(args.Entity);
            _propertyPanel.ScriptOpened += (s, args) =>
            {
                if (_propertyPanel.ViewModel.EditorEntity != null && _propertyPanel.ViewModel.EditorEntity.Entity is Actor actor && _propertyPanel.ViewModel.EditorEntity.MapEntity is MapActor mapActor)
                {
                    /*_scriptView = new ScriptView(filePath, actor, mapActor);
                    _scriptView.Saved += (sender, e) =>
                    {
                        //mapActor.Behavior.e.Script;
                    };*

                    OpenBehavior(args.FileName);
                }
                else
                {
                    // TODO - Throw some error to the user here?
                }
            };*/
        }

        public void OpenBehavior(string filePath)
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

            Title = System.IO.Path.GetFileNameWithoutExtension(filePath) + " - " + "SauceEditor";

            _behaviorView = _behaviorView ?? new BehaviorView();

            DockHelper.AddToDockAsAnchorableDocument(MainDockingManager, _behaviorView, Path.GetFileNameWithoutExtension(filePath), () =>
            {
                PlayButton.Visibility = Visibility.Hidden;
                //_map = null;
            });
        }

        public void OpenTexture(string filePath)
        {

        }

        public void OpenSound(string filePath)
        {

        }

        public void OpenMaterial(string filePath)
        {

        }

        public void OpenArchetype(string filePath)
        {

        }

        public void OpenScript(string filePath)
        {

        }
    }
}
