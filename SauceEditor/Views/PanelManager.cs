using SauceEditor.Models;
using SauceEditor.ViewModels;
using SauceEditor.Views.Behaviors;
using SauceEditor.Views.Factories;
using SauceEditor.Views.GamePanels;
using SauceEditor.Views.Libraries;
using SauceEditor.Views.Properties;
using SauceEditor.Views.Scripts;
using SauceEditor.Views.Tools;
using SauceEditor.Views.Trees.Entities;
using SauceEditor.Views.Trees.Projects;
using SauceEditorCore.Models.Components;
using System.Collections.Generic;
using Component = SauceEditorCore.Models.Components.Component;

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
        private List<ScriptPanel> _scriptPanels = new List<ScriptPanel>();
        private List<BehaviorPanel> _behaviorPanels = new List<BehaviorPanel>();

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
            _mainWindowViewModel.PanelFactory = this;
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

        public void CreateGamePanel(MapComponent mapComponent, Component component = null)
        {
            var gamePanel = new GamePanel()
            {
                Title = mapComponent.Name,
                CanClose = true
            };

            gamePanel.ViewModel.EntityFactory = _mainWindowViewModel;

            if (_mainWindowViewModel.EntityTreePanelViewModel != null)
            {
                _mainWindowViewModel.EntityTreePanelViewModel.LayerProvider = gamePanel.ViewModel.GameManager.EntityManager.LayerProvider;
                gamePanel.ViewModel.EntityDisplayer = _mainWindowViewModel.EntityTreePanelViewModel;
            }

            gamePanel.ViewModel.UpdateFromModel(mapComponent);
            gamePanel.IsActiveChanged += (s, args) => _mainWindowViewModel.PropertyViewModel.InitializeProperties(component ?? mapComponent);

            _gamePanels.Add(gamePanel);
            _dockTracker.AddToCenterDock(gamePanel, gamePanel.ViewModel);

            if (component != null)
            {
                switch (component)
                {
                    case ModelComponent modelComponent:
                        _mainWindowViewModel.ModelToolPanelViewModel.ModelComponent = modelComponent;
                        _mainWindowViewModel.ModelToolPanelViewModel.LayerSetter = gamePanel.ViewModel;

                        // We need to wait for the set view to load
                        if (EditorSettings.Instance.DefaultView == ViewTypes.Perspective)
                        {
                            // TODO - This is mad janky, we are waiting for this specific panel to load before we trigger adding the appropriate entities
                            gamePanel.ViewModel.PerspectiveViewModel.Control.PanelLoaded += (s, args) =>
                            {
                                _mainWindowViewModel.ModelToolPanelViewModel.OnModelToolTypeChanged();
                            };
                        }
                        break;
                }
            }
            else
            {
                // Because this is a plain ol' MapComponent, update the main view model reference
                _mainWindowViewModel.GamePanelViewModel = gamePanel.ViewModel;
            }

            gamePanel.ViewModel.IsActive = true;
        }

        public void CreateScriptPanel(ScriptComponent scriptComponent)
        {
            var scriptPanel = new ScriptPanel()
            {
                Title = scriptComponent.Name,
                CanClose = true
            };
            scriptPanel.ViewModel.UpdateFromModel(scriptComponent);

            _scriptPanels.Add(scriptPanel);
            _dockTracker.AddToCenterDock(scriptPanel, scriptPanel.ViewModel);

            _mainWindowViewModel.ScriptPanelViewModel = scriptPanel.ViewModel;

            scriptPanel.ViewModel.IsActive = true;
        }

        public void CreateBehaviorPanel(BehaviorComponent behaviorComponent)
        {
            var behaviorPanel = new BehaviorPanel()
            {
                Title = behaviorComponent.Name,
                CanClose = true
            };

            _behaviorPanels.Add(behaviorPanel);
            _dockTracker.AddToCenterDock(behaviorPanel, behaviorPanel.ViewModel);

            _mainWindowViewModel.BehaviorPanelViewModel = behaviorPanel.ViewModel;

            behaviorPanel.ViewModel.IsActive = true;

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
