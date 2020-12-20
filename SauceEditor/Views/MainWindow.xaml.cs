using SauceEditor.Views.Docks;
using SauceEditor.Views.Factories;
using SauceEditor.Views.Settings;
using SauceEditor.Views.Tools;
using SpiceEngineCore.Maps;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using GameWindow = SpiceEngine.Game.GameWindow;
using Map = SpiceEngine.Maps.Map;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;
using OpenTK;

namespace SauceEditor.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainView, IWindowFactory
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

        public void CreateGameWindow(IMap map)
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
