using SauceEditor.Models;
using SauceEditor.ViewModels.Commands;
using SpiceEngine.Entities;
using SpiceEngine.Game;
using SpiceEngine.Maps;
using SpiceEngine.Outputs;
using SpiceEngine.Rendering.Processing;
using SpiceEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.AvalonDock.Layout;
using ViewTypes = SpiceEngine.Game.ViewTypes;

namespace SauceEditor.Views.GamePanels
{
    /// <summary>
    /// Interaction logic for GamePanelManager.xaml
    /// </summary>
    public partial class GamePanelManager : DockPanel
    {
        private MapManager _mapManager;
        private GameManager _gameManager;
        private EntityMapping _entityMapping;

        private GamePanelView _perspectiveView;
        private GamePanelView _xView;
        private GamePanelView _yView;
        private GamePanelView _zView;

        public Resolution Resolution { get; set; }

        public Map Map => _mapManager.Map;

        public event EventHandler<EntitiesEventArgs> EntitySelectionChanged;
        public event EventHandler<CommandEventArgs> CommandExecuted;

        public GamePanelManager(string mapPath)
        {
            InitializeComponent();
            Open(mapPath);

            ViewModel.PerspectiveViewModel = _perspectiveView.ViewModel;
            ViewModel.XViewModel = _xView.ViewModel;
            ViewModel.YViewModel = _yView.ViewModel;
            ViewModel.ZViewModel = _zView.ViewModel;
        }

        public void RequestUpdate()
        {
            _perspectiveView.Panel.Invalidate();
            _xView.Panel.Invalidate();
            _yView.Panel.Invalidate();
            _zView.Panel.Invalidate();
        }

        public void SetSelectedTool(SpiceEngine.Game.Tools tool)
        {
            _perspectiveView.Panel.SelectedTool = tool;
            _xView.Panel.SelectedTool = tool;
            _yView.Panel.SelectedTool = tool;
            _zView.Panel.SelectedTool = tool;
        }

        public void UpdateEntity(EditorEntity entity)
        {
            _perspectiveView.Panel.UpdateEntities(entity.Entity.Yield());
            _xView.Panel.UpdateEntities(entity.Entity.Yield());
            _yView.Panel.UpdateEntities(entity.Entity.Yield());
            _zView.Panel.UpdateEntities(entity.Entity.Yield());
        }

        public void UpdateEntities(IEnumerable<IEntity> entities)
        {
            _perspectiveView.Panel.UpdateEntities(entities);
            _xView.Panel.UpdateEntities(entities);
            _yView.Panel.UpdateEntities(entities);
            _zView.Panel.UpdateEntities(entities);
        }

        private void MainDock_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    ViewModel.TransformMode = (TransformModes)((int)(ViewModel.TransformMode + 1) % Enum.GetValues(typeof(TransformModes)).Length);
                    break;
                case Key.Home:
                    _perspectiveView?.Panel.CenterView();
                    _xView?.Panel.CenterView();
                    _yView?.Panel.CenterView();
                    _zView?.Panel.CenterView();
                    break;
            }

            e.Handled = true;
        }

        private object _panelLock = new object();
        private bool _isGLContextLoaded = false;

        public void Open(string mapPath)
        {
            Resolution = new Resolution((int)Width, (int)Height);

            _mapManager = new MapManager(mapPath);
            _gameManager = new GameManager(Resolution);

            CreateAndShowPanels();
        }

        private void CreateAndShowPanels()
        {
            _perspectiveView = CreatePanel(ViewTypes.Perspective, AnchorableShowStrategy.Most);
            _xView = CreatePanel(ViewTypes.X, AnchorableShowStrategy.Right);
            _yView = CreatePanel(ViewTypes.Y, AnchorableShowStrategy.Bottom);
            _zView = CreatePanel(ViewTypes.Z, AnchorableShowStrategy.Right | AnchorableShowStrategy.Bottom);

            DockHelper.AddPanesToDockAsGrid(MainDockingManager, 2, _perspectiveView, _xView, _yView, _zView);
        }

        private GamePanelView CreatePanel(ViewTypes viewType, AnchorableShowStrategy showStrategy)
        {
            var gamePanel = new GamePanelView();
            gamePanel.ViewModel.ViewType = viewType;
            gamePanel.Panel.Load += (s, args) => LoadPanels();
            gamePanel.Panel.EntityDuplicated += (s, args) => DuplicateEntity(args.ID, args.NewID);
            gamePanel.EntitySelectionChanged += (s, args) => OnEntitySelectionChanged(viewType, args);
            gamePanel.Anchorable.Show();

            return gamePanel;
        }

        private void LoadPanels()
        {
            // Wait for at least one panel to finish loading so that we can be sure the GLContext is properly loaded
            lock (_panelLock)
            {
                // Lock and check to ensure that this only happens once
                if (!_isGLContextLoaded)
                {
                    _entityMapping = _gameManager.LoadFromMap(_mapManager.Map);
                    _mapManager.SetEntityMapping(_entityMapping);

                    // TODO - Determine how to handle the fact that each GamePanel is its own IMouseDelta...
                    _perspectiveView.Panel.LoadGameManager(_gameManager, _mapManager.Map, _entityMapping);
                    _xView.Panel.LoadGameManager(_gameManager, _mapManager.Map, _entityMapping);
                    _yView.Panel.LoadGameManager(_gameManager, _mapManager.Map, _entityMapping);
                    _zView.Panel.LoadGameManager(_gameManager, _mapManager.Map, _entityMapping);
                }

                _isGLContextLoaded = true;
            }
        }

        private void DuplicateEntity(int entityID, int duplicateEntityID)
        {
            _perspectiveView.Panel.Duplicate(entityID, duplicateEntityID);
            _xView.Panel.Duplicate(entityID, duplicateEntityID);
            _yView.Panel.Duplicate(entityID, duplicateEntityID);
            _zView.Panel.Duplicate(entityID, duplicateEntityID);

            //_physicsManager.DuplicateBody(entityID, duplicateEntityID);
            //_scriptManager;
        }

        private void OnEntitySelectionChanged(ViewTypes viewType, SpiceEngine.Game.EntitiesEventArgs args)
        {
            if (viewType != ViewTypes.Perspective) _perspectiveView.Panel.SelectEntities(args.Entities);
            if (viewType != ViewTypes.X) _xView.Panel.SelectEntities(args.Entities);
            if (viewType != ViewTypes.Y) _yView.Panel.SelectEntities(args.Entities);
            if (viewType != ViewTypes.Z) _zView.Panel.SelectEntities(args.Entities);

            if (args.Entities.Count > 0)
            {
                if (viewType != ViewTypes.Perspective) _perspectiveView.Panel.UpdateEntities(args.Entities);
                if (viewType != ViewTypes.X) _xView.Panel.UpdateEntities(args.Entities);
                if (viewType != ViewTypes.Y) _yView.Panel.UpdateEntities(args.Entities);
                if (viewType != ViewTypes.Z) _zView.Panel.UpdateEntities(args.Entities);

                _mapManager.UpdateEntities(args.Entities);
            }

            var editorEntities = _mapManager.GetEditorEntities(args.Entities);
            EntitySelectionChanged?.Invoke(this, new EntitiesEventArgs(editorEntities));
            //EntitySelectionChanged?.Invoke(this, new EntitiesEventArgs(_mapManager.GetMapEntities(args.Entities)));
        }

        public void SetView(Models.ViewTypes view)
        {
            switch (view)
            {
                case Models.ViewTypes.All:
                    View_All.IsSelected = true;
                    break;
                case Models.ViewTypes.Perspective:
                    View_Perspective.IsSelected = true;
                    break;
                case Models.ViewTypes.X:
                    View_X.IsSelected = true;
                    break;
                case Models.ViewTypes.Y:
                    View_Y.IsSelected = true;
                    break;
                case Models.ViewTypes.Z:
                    View_Z.IsSelected = true;
                    break;
            }
        }

        private void ViewComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ViewComboBox.SelectedItem as ComboBoxItem;
            
            switch (selectedItem.Content)
            {
                case "All":
                    DockHelper.ShowAllPanesInDockAsGrid(MainDockingManager);
                    break;
                case "Perspective":
                    DockHelper.ShowSinglePaneInDockGrid(MainDockingManager, _perspectiveView);
                    break;
                case "X":
                    DockHelper.ShowSinglePaneInDockGrid(MainDockingManager, _xView);
                    break;
                case "Y":
                    DockHelper.ShowSinglePaneInDockGrid(MainDockingManager, _yView);
                    break;
                case "Z":
                    DockHelper.ShowSinglePaneInDockGrid(MainDockingManager, _zView);
                    break;
            }
        }
    }
}
