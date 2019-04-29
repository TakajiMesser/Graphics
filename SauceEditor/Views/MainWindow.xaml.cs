﻿using OpenTK;
using SauceEditor.Helpers;
using SauceEditor.Models;
using SauceEditor.Models.Components;
using SauceEditor.ViewModels;
using SauceEditor.Views.Behaviors;
using SauceEditor.Views.Factories;
using SauceEditor.Views.GamePanels;
using SauceEditor.Views.ProjectTree;
using SauceEditor.Views.Properties;
using SauceEditor.Views.Scripts;
using SauceEditor.Views.Settings;
using SauceEditor.Views.Tools;
using System;
using System.ComponentModel;
using System.Diagnostics;
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
    public partial class MainWindow : Window, IMainViewFactory, IWindowFactory
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

            ViewModel.MainViewFactory = this;
            ViewModel.WindowFactory = this;

            Menu.ViewModel.MainViewFactory = this;
            Menu.ViewModel.WindowFactory = this;

            // TODO - Should this binding happen in the ViewModel itself?
            Menu.ViewModel.ComponentFactory = ViewModel;

            ViewModel.ProjectTreePanelViewModel = _projectTree.ViewModel;
            ViewModel.ToolsPanelViewModel = _toolPanel.ViewModel;
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
            /*_projectTree.MapSelected += (s, args) => OpenMap(args.FilePath);
            _projectTree.ModelSelected += (s, args) => OpenModel(args.FilePath);
            _projectTree.BehaviorSelected += (s, args) => OpenBehavior(args.FilePath);
            _projectTree.TextureSelected += (s, args) => OpenTexture(args.FilePath);
            _projectTree.AudioSelected += (s, args) => OpenSound(args.FilePath);*/

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
            _settingsWindow = new SettingsWindow();
            _settingsWindow.ViewModel.MainViewFactory = this;

            ViewModel.SettingsWindowViewModel = _settingsWindow.ViewModel;
            _settingsWindow.Show();
        }

        public ViewModel CreateGamePanelManager(Map map)
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

        public ViewModel CreateScriptView(Script script)
        {
            var scriptView = new ScriptView();
            scriptView.ViewModel.UpdateFromModel(script);

            var title = "";
            DockHelper.AddToDockAsAnchorableDocument(MainDockingManager, scriptView, title);

            return scriptView.ViewModel;
        }

        public ViewModel CreateBehaviorView(Behavior behavior)
        {
            var behaviorView = new BehaviorView();

            var title = "";// Path.GetFileNameWithoutExtension(filePath);
            DockHelper.AddToDockAsAnchorableDocument(MainDockingManager, behaviorView, title, () =>
            {
                ViewModel.PlayVisibility = Visibility.Hidden;
                //_map = null;
            });

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

        public void SaveAll()
        {
            //_map?.Save(_mapPath);
            //_projectTree?.SaveProject();
        }

        public void LoadSettings()
        {
            ViewModel.Settings = EditorSettings.Load(SauceEditor.Helpers.FilePathHelper.SETTINGS_PATH);
        }
    }
}
