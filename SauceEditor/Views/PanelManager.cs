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
    public class PanelManager : IPanelFactory
    {
        private MainWindowViewModel _mainWindowViewModel;
        private DockTracker _dockTracker;

        // Left Panels
        //private ToolsPanel _toolsPanel;
        private ModelToolPanel _modelToolPanel;
        private BrushToolPanel _brushToolPanel;

        // Center Panels
        private List<GamePanel> _gamePanels = new List<GamePanel>();
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
            _modelToolPanel = new ModelToolPanel();
            _brushToolPanel = new BrushToolPanel();

            _projectTreePanel = new ProjectTreePanel();
            _libraryPanel = new LibraryPanel();
            _propertyPanel = new PropertyPanel();
            _entityTreePanel = new EntityTreePanel();

            //_mainWindowViewModel.ToolsPanelViewModel = _toolPanel.ViewModel;
            _mainWindowViewModel.ModelToolPanelViewModel = _modelToolPanel.ViewModel;
            _mainWindowViewModel.BrushToolPanelViewModel = _brushToolPanel.ViewModel;

            _mainWindowViewModel.ProjectTreePanelViewModel = _projectTreePanel.ViewModel;
            _mainWindowViewModel.LibraryPanelViewModel = _libraryPanel.ViewModel;
            _mainWindowViewModel.PropertyViewModel = _propertyPanel.ViewModel;
            _mainWindowViewModel.EntityTreePanelViewModel = _entityTreePanel.ViewModel;
        }

        public void AddDefaultPanels()
        {
            AddDefaultLeftPanels();
            AddDefaultCenterPanels();
            AddDefaultRightPanels();
        }

        public void AddGamePanel(GamePanel gamePanel)
        {
            _gamePanels.Add(gamePanel);
            _dockTracker.AddToCenterDock(gamePanel, gamePanel.ViewModel);
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

        public void OpenModelToolPanel()
        {
            if (!_dockTracker.ContainsLeftDock(_modelToolPanel?.ViewModel))
            {
                _modelToolPanel = new ModelToolPanel();
                _mainWindowViewModel.ModelToolPanelViewModel = _modelToolPanel.ViewModel;
                _dockTracker.AddToLeftDock(_modelToolPanel, _modelToolPanel.ViewModel);
            }

            _modelToolPanel.ViewModel.IsActive = true;
        }

        public void OpenBrushToolPanel()
        {
            if (!_dockTracker.ContainsLeftDock(_brushToolPanel?.ViewModel))
            {
                _brushToolPanel = new BrushToolPanel();
                _mainWindowViewModel.BrushToolPanelViewModel = _brushToolPanel.ViewModel;
                _dockTracker.AddToLeftDock(_brushToolPanel, _brushToolPanel.ViewModel);
            }

            _brushToolPanel.ViewModel.IsActive = true;
        }

        public void OpenProjectTreePanel()
        {
            if (!_dockTracker.ContainsLeftDock(_projectTreePanel?.ViewModel))
            {
                _projectTreePanel = new ProjectTreePanel();
                _mainWindowViewModel.ProjectTreePanelViewModel = _projectTreePanel.ViewModel;
                _dockTracker.AddToRightDock(_projectTreePanel, _projectTreePanel.ViewModel);
            }

            _modelToolPanel.ViewModel.IsActive = true;
        }

        public void OpenLibraryPanel()
        {
            if (!_dockTracker.ContainsLeftDock(_libraryPanel?.ViewModel))
            {
                _libraryPanel = new LibraryPanel();
                _mainWindowViewModel.LibraryPanelViewModel = _libraryPanel.ViewModel;
                _dockTracker.AddToRightDock(_libraryPanel, _libraryPanel.ViewModel);
            }

            _libraryPanel.ViewModel.IsActive = true;
        }

        public void OpenPropertyPanel()
        {
            if (!_dockTracker.ContainsLeftDock(_propertyPanel?.ViewModel))
            {
                _propertyPanel = new PropertyPanel();
                _mainWindowViewModel.PropertyViewModel = _propertyPanel.ViewModel;
                _dockTracker.AddToRightDock(_propertyPanel, _propertyPanel.ViewModel);
            }

            _propertyPanel.ViewModel.IsActive = true;
        }

        public void OpenEntityTreePanel()
        {
            if (!_dockTracker.ContainsLeftDock(_entityTreePanel?.ViewModel))
            {
                _entityTreePanel = new EntityTreePanel();
                _mainWindowViewModel.EntityTreePanelViewModel = _entityTreePanel.ViewModel;
                _dockTracker.AddToRightDock(_entityTreePanel, _entityTreePanel.ViewModel);
            }

            _entityTreePanel.ViewModel.IsActive = true;
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
