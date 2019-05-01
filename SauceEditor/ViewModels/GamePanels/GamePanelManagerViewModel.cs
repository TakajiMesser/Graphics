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
using System.Windows.Input;

namespace SauceEditor.ViewModels
{
    public class GamePanelManagerViewModel : ViewModel, IMainDockViewModel
    {
        public MapManager MapManager { get; set; }
        public GameManager GameManager { get; set; }
        public EntityMapping EntityMapping { get; set; }

        public bool IsPlayable => true;

        public TransformModes TransformMode { get; set; }
        public Models.ViewTypes ViewType { get; set; }

        public GamePanelViewModel PerspectiveViewModel { get; set; }
        public GamePanelViewModel XViewModel { get; set; }
        public GamePanelViewModel YViewModel { get; set; }
        public GamePanelViewModel ZViewModel { get; set; }

        public Resolution Resolution { get; set; }

        public Map Map => MapManager.Map;

        private RelayCommand _translateCommand;
        public RelayCommand TranslateCommand
        {
            get
            {
                return _translateCommand ?? (_translateCommand = new RelayCommand(
                    p => TransformMode = TransformModes.Translate,
                    p => TransformMode != TransformModes.Translate
                ));
            }
        }

        private RelayCommand _rotateCommand;
        public RelayCommand RotateCommand
        {
            get
            {
                return _rotateCommand ?? (_rotateCommand = new RelayCommand(
                    p => TransformMode = TransformModes.Rotate,
                    p => TransformMode != TransformModes.Rotate
                ));
            }
        }

        private RelayCommand _scaleCommand;
        public RelayCommand ScaleCommand
        {
            get
            {
                return _scaleCommand ?? (_scaleCommand = new RelayCommand(
                    p => TransformMode = TransformModes.Scale,
                    p => TransformMode != TransformModes.Scale
                ));
            }
        }

        public event EventHandler<EntitiesEventArgs> EntitySelectionChanged;
        public event EventHandler<CommandEventArgs> CommandExecuted;

        public void OnTransformModeChanged()
        {
            PerspectiveViewModel.Panel.TransformMode = TransformMode;
            XViewModel.Panel.TransformMode = TransformMode;
            YViewModel.Panel.TransformMode = TransformMode;
            ZViewModel.Panel.TransformMode = TransformMode;

            CommandManager.InvalidateRequerySuggested();
        }

        public void OnViewTypeChanged()
        {

        }

        public void RequestUpdate()
        {
            PerspectiveViewModel.Panel.Invalidate();
            XViewModel.Panel.Invalidate();
            YViewModel.Panel.Invalidate();
            ZViewModel.Panel.Invalidate();
        }

        public void SetSelectedTool(SpiceEngine.Game.Tools tool)
        {
            PerspectiveViewModel.Panel.SelectedTool = tool;
            XViewModel.Panel.SelectedTool = tool;
            YViewModel.Panel.SelectedTool = tool;
            ZViewModel.Panel.SelectedTool = tool;
        }

        public void UpdateEntity(EditorEntity entity)
        {
            PerspectiveViewModel.Panel.UpdateEntities(entity.Entity.Yield());
            XViewModel.Panel.UpdateEntities(entity.Entity.Yield());
            YViewModel.Panel.UpdateEntities(entity.Entity.Yield());
            ZViewModel.Panel.UpdateEntities(entity.Entity.Yield());
        }

        public void UpdateEntities(IEnumerable<IEntity> entities)
        {
            PerspectiveViewModel.Panel.UpdateEntities(entities);
            XViewModel.Panel.UpdateEntities(entities);
            YViewModel.Panel.UpdateEntities(entities);
            ZViewModel.Panel.UpdateEntities(entities);
        }

        public void DuplicateEntity(int entityID, int duplicateEntityID)
        {
            PerspectiveViewModel.Panel.Duplicate(entityID, duplicateEntityID);
            XViewModel.Panel.Duplicate(entityID, duplicateEntityID);
            YViewModel.Panel.Duplicate(entityID, duplicateEntityID);
            ZViewModel.Panel.Duplicate(entityID, duplicateEntityID);
        }

        public void CenterView()
        {
            PerspectiveViewModel?.Panel.CenterView();
            XViewModel?.Panel.CenterView();
            YViewModel?.Panel.CenterView();
            ZViewModel?.Panel.CenterView();
        }

        private object _panelLock = new object();
        private bool _isGLContextLoaded = false;
        private bool _isMapLoadedInPanels = false;

        public void UpdateFromModel(Map map)
        {
            MapManager = new MapManager(map);
            GameManager = new GameManager(Resolution);

            EntityMapping = GameManager.LoadFromMap(MapManager.Map);
            MapManager.SetEntityMapping(EntityMapping);

            lock (_panelLock)
            {
                // Lock and check to ensure that this only happens once
                if (_isGLContextLoaded && !_isMapLoadedInPanels)
                {
                    PerspectiveViewModel.Panel.LoadGameManager(GameManager, MapManager.Map, EntityMapping);
                    XViewModel.Panel.LoadGameManager(GameManager, MapManager.Map, EntityMapping);
                    YViewModel.Panel.LoadGameManager(GameManager, MapManager.Map, EntityMapping);
                    ZViewModel.Panel.LoadGameManager(GameManager, MapManager.Map, EntityMapping);

                    _isMapLoadedInPanels = true;
                }
            }
        }

        public void LoadPanels()
        {
            // Wait for at least one panel to finish loading so that we can be sure the GLContext is properly loaded
            if (!_isGLContextLoaded)
            {
                lock (_panelLock)
                {
                    // Lock and check to ensure that this only happens once
                    _isGLContextLoaded = true;

                    if (!_isMapLoadedInPanels && MapManager != null)
                    {
                        // TODO - Determine how to handle the fact that each GamePanel is its own IMouseDelta...
                        PerspectiveViewModel.Panel.LoadGameManager(GameManager, MapManager.Map, EntityMapping);
                        XViewModel.Panel.LoadGameManager(GameManager, MapManager.Map, EntityMapping);
                        YViewModel.Panel.LoadGameManager(GameManager, MapManager.Map, EntityMapping);
                        ZViewModel.Panel.LoadGameManager(GameManager, MapManager.Map, EntityMapping);

                        _isMapLoadedInPanels = true;
                    }
                }
            }
        }
    }
}