using SauceEditor.Models;
using SauceEditor.Models.Components;
using SauceEditor.Views.Factories;
using SpiceEngine.Entities;
using SpiceEngine.Game;
using SpiceEngine.Maps;
using SpiceEngine.Outputs;
using SpiceEngine.Rendering.Processing;
using SpiceEngine.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ViewTypes = SauceEditor.Models.ViewTypes;

namespace SauceEditor.ViewModels
{
    public class GamePanelManagerViewModel : MainDockViewModel
    {
        public GamePanelManagerViewModel()
        {
            IsPlayable = true;
        }

        public GameManager GameManager { get; set; }
        public EntityMapping EntityMapping { get; set; }

        public TransformModes TransformMode { get; set; }
        public ViewTypes ViewType { get; set; }

        public GamePanelViewModel PerspectiveViewModel { get; set; }
        public GamePanelViewModel XViewModel { get; set; }
        public GamePanelViewModel YViewModel { get; set; }
        public GamePanelViewModel ZViewModel { get; set; }

        public List<EditorEntity> SelectedEntities { get; set; }

        public Resolution Resolution { get; set; }

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

        //public event EventHandler<EntitiesEventArgs> EntitySelectionChanged;
        //public event EventHandler<CommandEventArgs> CommandExecuted;

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

        public void OnPerspectiveViewModelChanged() => OnPanelViewModelChange(PerspectiveViewModel);
        public void OnXViewModelChanged() => OnPanelViewModelChange(XViewModel);
        public void OnYViewModelChanged() => OnPanelViewModelChange(YViewModel);
        public void OnZViewModelChanged() => OnPanelViewModelChange(ZViewModel);

        private void OnPanelViewModelChange(GamePanelViewModel panelViewModel)
        {
            panelViewModel.Panel.EntitySelectionChanged += (s, args) => SelectedEntities = MapManager.GetEditorEntities(args.Entities).ToList();
            panelViewModel.Panel.Load += (s, args) => LoadPanels();
            panelViewModel.Panel.EntityDuplicated += (s, args) => DuplicateEntity(args.ID, args.NewID);
        }

        public void OnSelectedEntitiesChanged()
        {
            if (ViewType != ViewTypes.Perspective) PerspectiveViewModel.Panel.SelectEntities(SelectedEntities.Select(e => e.Entity));
            if (ViewType != ViewTypes.X) XViewModel.Panel.SelectEntities(SelectedEntities.Select(e => e.Entity));
            if (ViewType != ViewTypes.Y) YViewModel.Panel.SelectEntities(SelectedEntities.Select(e => e.Entity));
            if (ViewType != ViewTypes.Z) ZViewModel.Panel.SelectEntities(SelectedEntities.Select(e => e.Entity));

            if (SelectedEntities.Any())
            {
                if (ViewType != ViewTypes.Perspective) PerspectiveViewModel.Panel.UpdateEntities(SelectedEntities.Select(e => e.Entity));
                if (ViewType != ViewTypes.X) XViewModel.Panel.UpdateEntities(SelectedEntities.Select(e => e.Entity));
                if (ViewType != ViewTypes.Y) YViewModel.Panel.UpdateEntities(SelectedEntities.Select(e => e.Entity));
                if (ViewType != ViewTypes.Z) ZViewModel.Panel.UpdateEntities(SelectedEntities.Select(e => e.Entity));

                MapManager.UpdateEntities(SelectedEntities.Select(e => e.Entity));
            }

            var editorEntities = MapManager.GetEditorEntities(SelectedEntities.Select(e => e.Entity));
            //EntitySelectionChanged?.Invoke(this, new EntitiesEventArgs(editorEntities));
            //EntitySelectionChanged?.Invoke(this, new EntitiesEventArgs(_mapManager.GetMapEntities(args.Entities)));
        }

        public void RequestUpdate()
        {
            PerspectiveViewModel.Panel.Invalidate();
            XViewModel.Panel.Invalidate();
            YViewModel.Panel.Invalidate();
            ZViewModel.Panel.Invalidate();
        }

        public void SetSelectedTool(Tools tool)
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

        private readonly object _panelLock = new object();
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