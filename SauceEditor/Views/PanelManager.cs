using SauceEditor.ViewModels;
using SauceEditor.Views.Behaviors;
using SauceEditor.Views.GamePanels;
using SauceEditor.Views.Libraries;
using SauceEditor.Views.Properties;
using SauceEditor.Views.Scripts;
using SauceEditor.Views.Tools;
using SauceEditor.Views.Trees.Entities;
using SauceEditor.Views.Trees.Projects;
using System.Collections.Generic;

namespace SauceEditor.Views
{
    public class PanelManager
    {
        private MainWindowViewModel _mainWindowViewModel;
        private DockTracker _dockTracker;

        // Left Panels
        //private ToolsPanel _toolsPanel;
        private ModelToolPanel _modelToolPanel;
        private BrushToolPanel _brushToolPanel;

        // Center Panels
        private List<GamePanelManager> _gamePanelManagers = new List<GamePanelManager>();
        private List<ScriptView> _scriptViews = new List<ScriptView>();
        private List<BehaviorView> _behaviorViews = new List<BehaviorView>();

        // Right Panels
        private ProjectTreePanel _projectTreePanel;
        private LibraryPanel _libraryPanel;
        private PropertyPanel _propertyPanel;
        private EntityTreePanel _entityTreePanel;

        public PanelManager(MainWindowViewModel mainWindowViewModel, DockTracker dockTracker)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _dockTracker = dockTracker;

            _mainWindowViewModel.DockTracker = _dockTracker;
        }

        public void InitializePanels()
        {
            _projectTreePanel = new ProjectTreePanel();
            _libraryPanel = new LibraryPanel();
            _propertyPanel = new PropertyPanel();
            _entityTreePanel = new EntityTreePanel();

            _modelToolPanel = new ModelToolPanel();
            _brushToolPanel = new BrushToolPanel();

            _mainWindowViewModel.ProjectTreePanelViewModel = _projectTreePanel.ViewModel;
            _mainWindowViewModel.LibraryPanelViewModel = _libraryPanel.ViewModel;
            _mainWindowViewModel.PropertyViewModel = _propertyPanel.ViewModel;
            _mainWindowViewModel.EntityTreePanelViewModel = _entityTreePanel.ViewModel;

            //_mainWindowViewModel.ToolsPanelViewModel = _toolPanel.ViewModel;
            _mainWindowViewModel.ModelToolPanelViewModel = _modelToolPanel.ViewModel;
            _mainWindowViewModel.BrushToolPanelViewModel = _brushToolPanel.ViewModel;
        }

        public void AddDefaultPanels()
        {
            AddDefaultLeftPanels();
            AddDefaultCenterPanels();
            AddDefaultRightPanels();
        }

        public void AddGamePanel(GamePanelManager gamePanelManager)
        {
            _gamePanelManagers.Add(gamePanelManager);
            _dockTracker.AddToCenterDock(gamePanelManager, gamePanelManager.ViewModel);
        }

        public void AddScriptView(ScriptView scriptView)
        {
            _scriptViews.Add(scriptView);
            _dockTracker.AddToCenterDock(scriptView, scriptView.ViewModel);
        }

        public void AddBehaviorView(BehaviorView behaviorView)
        {
            _behaviorViews.Add(behaviorView);
            _dockTracker.AddToCenterDock(behaviorView, behaviorView.ViewModel);
        }

        private void AddDefaultLeftPanels()
        {
            //_dockTracker.AddToLeftDock(_toolsPanel, _toolsPanel.ViewModel);
            _dockTracker.AddToLeftDock(_modelToolPanel, _modelToolPanel.ViewModel);
            _dockTracker.AddToLeftDock(_brushToolPanel, _brushToolPanel.ViewModel);
        }

        private void AddDefaultCenterPanels()
        {
            // For now, NO center panels will be added by default
        }

        private void AddDefaultRightPanels()
        {
            _dockTracker.AddToRightDock(_projectTreePanel, _projectTreePanel.ViewModel);
            _dockTracker.AddToRightDock(_libraryPanel, _libraryPanel.ViewModel);
            _dockTracker.AddToRightDock(_propertyPanel, _propertyPanel.ViewModel);
            _dockTracker.AddToRightDock(_entityTreePanel, _entityTreePanel.ViewModel);

            _projectTreePanel.IsActive = true;
        }
    }
}
