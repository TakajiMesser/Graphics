using OpenTK;
using SauceEditor.Models;
using SauceEditor.ViewModels;
using SauceEditor.Views.Behaviors;
using SauceEditor.Views.Factories;
using SauceEditor.Views.GamePanels;
using SauceEditor.Views.Libraries;
using SauceEditor.Views.Properties;
using SauceEditor.Views.Scripts;
using SauceEditor.Views.Settings;
using SauceEditor.Views.Tools;
using SauceEditor.Views.Trees.Entities;
using SauceEditor.Views.Trees.Projects;
using SauceEditorCore.Models.Components;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using GameWindow = SpiceEngine.Game.GameWindow;
using Map = SpiceEngine.Maps.Map;

namespace SauceEditor.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainView, IWindowFactory, IGameDockFactory
    {
        private DockTracker _dockTracker;

        // Side panels
        private ProjectTreePanel _projectTree = new ProjectTreePanel();
        private LibraryPanel _libraryPanel = new LibraryPanel();
        private PropertyPanel _propertyPanel = new PropertyPanel();
        private EntityTreePanel _entityTree = new EntityTreePanel();

        private ToolsPanel _toolPanel = new ToolsPanel();
        private ModelToolPanel _modelToolPanel = new ModelToolPanel();

        // Separate windows
        private GameWindow _gameWindow;
        private SettingsWindow _settingsWindow;

        public MainWindow()
        {
            PresentationTraceSources.DataBindingSource.Listeners.Add(new ConsoleTraceListener());
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Error;
            InitializeComponent();

            _dockTracker = new DockTracker(MainDockingManager, PropertyDockingManager, ToolDockingManager)
            {
                MainWindowVM = ViewModel
            };

            //ViewModel.MainViewFactory = _dockTracker;
            ViewModel.DockTracker = _dockTracker;
            ViewModel.GameDockFactory = this;
            ViewModel.WindowFactory = this;

            Menu.ViewModel.MainView = this;
            //Menu.ViewModel.MainViewFactory = _dockTracker;
            Menu.ViewModel.WindowFactory = this;

            // TODO - Should this binding happen in the ViewModel itself?
            Menu.ViewModel.ComponentFactory = ViewModel;

            ViewModel.ProjectTreePanelViewModel = _projectTree.ViewModel;
            ViewModel.LibraryPanelViewModel = _libraryPanel.ViewModel;
            ViewModel.PropertyViewModel = _propertyPanel.ViewModel;
            ViewModel.EntityTreePanelViewModel = _entityTree.ViewModel;

            ViewModel.ToolsPanelViewModel = _toolPanel.ViewModel;

            MainDockingManager.ActiveContentChanged += (s, args) =>
            {
                ViewModel.PlayCommand.InvokeCanExecuteChanged();
                //_propertyPanel.ViewModel.Properties = new EntityPropertyViewModel();
            };
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _gameWindow?.Close();
            base.OnClosing(e);
            Application.Current.Shutdown();
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            /*_projectTree.MapSelected += (s, args) => OpenMap(args.FilePath);
            _projectTree.ModelSelected += (s, args) => OpenModel(args.FilePath);
            _projectTree.BehaviorSelected += (s, args) => OpenBehavior(args.FilePath);
            _projectTree.TextureSelected += (s, args) => OpenTexture(args.FilePath);
            _projectTree.AudioSelected += (s, args) => OpenSound(args.FilePath);*/

            _toolPanel.ToolSelected += ToolPanel_ToolSelected;

            _dockTracker.AddToPropertyDock(_projectTree);
            _dockTracker.AddToPropertyDock(_propertyPanel);
            _dockTracker.AddToPropertyDock(_libraryPanel);
            _dockTracker.AddToPropertyDock(_entityTree);

            _dockTracker.AddToToolDock(_modelToolPanel);

            _projectTree.IsActive = true;
            //SideDockManager.ActiveContent = _projectTree;
        }

        private void ToolPanel_ToolSelected(object sender, ToolSelectedEventArgs e)
        {
            //_gamePanelManager?.ViewModel.SetSelectedTool(e.NewTool);
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

        public ViewModel CreateGamePanelManager(MapComponent mapComponent, SauceEditorCore.Models.Components.Component component = null)
        {
            var gamePanelManager = new GamePanelManager()
            {
                Title = mapComponent.Name,
                CanClose = true
            };

            //gamePanelManager.ViewModel.Factory = this;
            gamePanelManager.ViewModel.UpdateFromModel(mapComponent);
            gamePanelManager.IsActiveChanged += (s, args) => ViewModel.PropertyViewModel.InitializeProperties(component ?? mapComponent);

            _dockTracker.AddToGameDock(gamePanelManager);

            if (component != null)
            {
                switch (component)
                {
                    case ModelComponent modelComponent:
                        _modelToolPanel.ViewModel.ModelComponent = modelComponent;
                        _modelToolPanel.ViewModel.LayerSetter = gamePanelManager.ViewModel;

                        // We need to wait for the set view to load
                        if (EditorSettings.Instance.DefaultView == ViewTypes.Perspective)
                        {
                            // TODO - This is mad janky, we are waiting for this specific panel to load before we trigger adding the appropriate entities
                            gamePanelManager.ViewModel.PerspectiveViewModel.Panel.PanelLoaded += (s, args) =>
                            {
                                _modelToolPanel.ViewModel.OnModelToolTypeChanged();
                            };
                        }
                        break;
                }
            }
            
            return gamePanelManager.ViewModel;
        }

        public ViewModel CreateScriptView(ScriptComponent scriptComponent)
        {
            var scriptView = new ScriptView()
            {
                Title = scriptComponent.Name,
                CanClose = true
            };
            scriptView.ViewModel.UpdateFromModel(scriptComponent);

            _dockTracker.AddToGameDock(scriptView);

            return scriptView.ViewModel;
        }

        public ViewModel CreateBehaviorView(BehaviorComponent behaviorComponent)
        {
            var behaviorView = new BehaviorView()
            {
                Title = behaviorComponent.Name,
                CanClose = true
            };

            _dockTracker.AddToGameDock(behaviorView);

            return behaviorView.ViewModel;

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
        }

        public void CreateGameWindow(Map map)
        {
            //_gamePanelManager.IsEnabled = false;
            using (var gameWindow = new GameWindow(map))
            {
                gameWindow.VSync = VSyncMode.Adaptive;
                gameWindow.LoadAndRun();
                //gameWindow.Run(60.0, 0.0);
            }

            /*var gameWindow = new GameWindow(map)
            {
                VSync = VSyncMode.Adaptive
            };
            //_gameWindow.Closed += (s, args) => _gamePanelManager.IsEnabled = true;
            gameWindow.Run(60.0, 0.0);*/
        }

        public void CreateSettingsWindow()
        {
            _settingsWindow = new SettingsWindow();
            _settingsWindow.ViewModel.MainView = this;

            ViewModel.SettingsWindowViewModel = _settingsWindow.ViewModel;
            _settingsWindow.Show();
        }

        public void SaveAll()
        {
            //_map?.Save(_mapPath);
            //_projectTree?.SaveProject();
        }
    }
}
