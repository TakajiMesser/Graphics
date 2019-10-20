using OpenTK;
using SauceEditor.Models;
using SauceEditor.ViewModels;
using SauceEditor.Views.Behaviors;
using SauceEditor.Views.Factories;
using SauceEditor.Views.GamePanels;
using SauceEditor.Views.Scripts;
using SauceEditor.Views.Settings;
using SauceEditor.Views.Tools;
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
        private PanelManager _panelManager;

        // Separate windows
        private GameWindow _gameWindow;
        private SettingsWindow _settingsWindow;

        public MainWindow()
        {
            PresentationTraceSources.DataBindingSource.Listeners.Add(new ConsoleTraceListener());
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Error;
            InitializeComponent();

            _panelManager = new PanelManager(ViewModel, new DockTracker(LeftDockingManager, CenterDockingManager, RightDockingManager));
            _panelManager.InitializePanels();

            ViewModel.GameDockFactory = this;
            ViewModel.WindowFactory = this;

            Menu.ViewModel.MainView = this;
            //Menu.ViewModel.MainViewFactory = _dockTracker;
            Menu.ViewModel.WindowFactory = this;
            Menu.ViewModel.PanelFactory = _panelManager;

            // TODO - Should this binding happen in the ViewModel itself?
            Menu.ViewModel.ComponentFactory = ViewModel;

            CenterDockingManager.ActiveContentChanged += (s, args) =>
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
            _projectTree.AudioSelected += (s, args) => OpenSound(args.FilePath);

            _toolPanel.ToolSelected += ToolPanel_ToolSelected;*/

            _panelManager.AddDefaultPanels();
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

        public ViewModel CreateGamePanel(MapComponent mapComponent, SauceEditorCore.Models.Components.Component component = null)
        {
            var gamePanel = new GamePanel()
            {
                Title = mapComponent.Name,
                CanClose = true
            };

            //gamePanelManager.ViewModel.Factory = this;
            gamePanel.ViewModel.EntityFactory = ViewModel;

            if (ViewModel.EntityTreePanelViewModel != null)
            {
                ViewModel.EntityTreePanelViewModel.LayerProvider = gamePanel.ViewModel.GameManager.EntityManager.LayerProvider;
                gamePanel.ViewModel.EntityDisplayer = ViewModel.EntityTreePanelViewModel;
            }

            gamePanel.ViewModel.UpdateFromModel(mapComponent);
            gamePanel.IsActiveChanged += (s, args) => ViewModel.PropertyViewModel.InitializeProperties(component ?? mapComponent);

            _panelManager.AddGamePanel(gamePanel);

            if (component != null)
            {
                switch (component)
                {
                    case ModelComponent modelComponent:
                        ViewModel.ModelToolPanelViewModel.ModelComponent = modelComponent;
                        ViewModel.ModelToolPanelViewModel.LayerSetter = gamePanel.ViewModel;

                        // We need to wait for the set view to load
                        if (EditorSettings.Instance.DefaultView == ViewTypes.Perspective)
                        {
                            // TODO - This is mad janky, we are waiting for this specific panel to load before we trigger adding the appropriate entities
                            gamePanel.ViewModel.PerspectiveViewModel.Control.PanelLoaded += (s, args) =>
                            {
                                ViewModel.ModelToolPanelViewModel.OnModelToolTypeChanged();
                            };
                        }
                        break;
                }
            }
            
            return gamePanel.ViewModel;
        }

        public ViewModel CreateScriptView(ScriptComponent scriptComponent)
        {
            var scriptView = new ScriptView()
            {
                Title = scriptComponent.Name,
                CanClose = true
            };
            scriptView.ViewModel.UpdateFromModel(scriptComponent);

            _panelManager.AddScriptView(scriptView);

            return scriptView.ViewModel;
        }

        public ViewModel CreateBehaviorView(BehaviorComponent behaviorComponent)
        {
            var behaviorView = new BehaviorView()
            {
                Title = behaviorComponent.Name,
                CanClose = true
            };

            _panelManager.AddBehaviorView(behaviorView);

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
