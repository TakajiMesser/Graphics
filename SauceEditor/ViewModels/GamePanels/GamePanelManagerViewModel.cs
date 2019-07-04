using SauceEditor.ViewModels.Docks;
using SauceEditor.ViewModels.Properties;
using SauceEditorCore.Models.Components;
using SauceEditorCore.Models.Entities;
using SpiceEngine.Entities;
using SpiceEngine.Game;
using SpiceEngine.Maps;
using SpiceEngine.Outputs;
using SpiceEngine.Rendering;
using SpiceEngine.Rendering.Processing;
using SpiceEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ViewTypes = SauceEditor.Models.ViewTypes;

namespace SauceEditor.ViewModels
{
    public class GamePanelManagerViewModel : DockViewModel, ILayerSetter
    {
        public GamePanelManagerViewModel() : base(DockTypes.Game) => IsPlayable = true;

        public IDisplayProperties PropertyDisplayer { get; set; }

        public GameManager GameManager { get; set; }
        public EntityMapping EntityMapping { get; set; }
        public MapComponent MapComponent { get; set; }

        public TransformModes TransformMode { get; set; }
        public ViewTypes ViewType { get; set; }

        public GamePanelViewModel PerspectiveViewModel { get; set; }
        public GamePanelViewModel XViewModel { get; set; }
        public GamePanelViewModel YViewModel { get; set; }
        public GamePanelViewModel ZViewModel { get; set; }

        //public SelectionManager SelectionManager { get; set; }
        //public List<EditorEntity> SelectedEntities { get; set; }

        public Resolution Resolution { get; set; }

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

        public void OnPerspectiveViewModelChanged() => OnPanelViewModelChange(PerspectiveViewModel);
        public void OnXViewModelChanged() => OnPanelViewModelChange(XViewModel);
        public void OnYViewModelChanged() => OnPanelViewModelChange(YViewModel);
        public void OnZViewModelChanged() => OnPanelViewModelChange(ZViewModel);

        private void OnPanelViewModelChange(GamePanelViewModel panelViewModel)
        {
            //SelectionManager = panelViewModel.Panel.SelectionManager;

            panelViewModel.Panel.EntitySelectionChanged += (s, args) => UpdatedSelection(args.Entities);
            panelViewModel.Panel.Load += (s, args) => LoadPanels();
            panelViewModel.Panel.EntityDuplicated += (s, args) => DuplicateEntity(args.Duplication.OriginalID, args.Duplication.DuplicatedID);
        }

        public void EnableLayer(string layerName)
        {
            // If the layer is enabled, it means that these IDs will get included regardless if they show up in other layers
            GameManager.EntityManager.SetLayerState(layerName, LayerStates.Enabled);
        }

        public void DisableLayer(string layerName)
        {
            // If the layer is disabled, it means that these IDs will get excluded regardless if they show up in other layers
            GameManager.EntityManager.SetLayerState(layerName, LayerStates.Disabled);
        }

        public void NeutralizeLayer(string layerName)
        {
            // If the layer is neutralized, it means that these IDs will be included UNLESS they are excluded in another layer
            GameManager.EntityManager.SetLayerState(layerName, LayerStates.Neutral);
        }

        // e.g.
        // Root     - 1, 2, 3, 4, 5, 6, 7, 8, 9, 10
        // Face     - 2, 3, 4, 5
        // Triangle - 6, 7, 8
        // Vertex   - 9, 10
        // 
        // By default, Root is NEUTRAL
        // Face ->
        //      Leave Root NEUTRAL for Render
        //      Set Face ENABLED
        //      Set Triangle DISABLED
        //      Set Vertex DISABLED
        // Triangle ->
        //      

        public void AddToLayer(string layerName, IEnumerable<IModelEntity> entities)
        {
            // TODO - Correct and reorganize this method
            // First, we need to ensure that this layer exists in the LayerManager
            // We then need to add all of these entities to it, then enable the layer
            // In subsequent calls, we just need to disable the root layer, then enable the requested layers
            // An issue is that we want to ONLY render the Root layer for diffuse/lit. The new layer should just be for selection/wireframe
            var modelEntities = entities.ToList();

            // Add these entities to a new layer, enable it, and disable all other layers
            if (!GameManager.EntityManager.ContainsLayer(layerName))
            {
                foreach (var entity in modelEntities)
                {
                    var id = GameManager.EntityManager.AddEntity(entity);
                    entity.ID = id;
                }

                GameManager.EntityManager.AddLayer(layerName);
                GameManager.EntityManager.AddEntitiesToLayer(layerName, modelEntities.Select(e => e.ID));
            }

            //GameManager.EntityManager.SetLayerState(layerName, LayerStates.Enabled);
            //GameManager.EntityManager.SetRenderLayerState(layerName, LayerStates.Enabled);

            foreach (var entity in modelEntities)
            {
                var renderable = entity.ToRenderable();

                PerspectiveViewModel.Panel.AddEntity(entity.ID, renderable);
                XViewModel.Panel.AddEntity(entity.ID, renderable);
                YViewModel.Panel.AddEntity(entity.ID, renderable);
                ZViewModel.Panel.AddEntity(entity.ID, renderable);
            }

            //SelectionManager.SetSelectableEntities(entities);
        }

        public void SelectEntities(IEnumerable<IEntity> entities) => throw new NotImplementedException();// SelectionManager.SelectEntities(entities);

        private void UpdatedSelection(IEnumerable<IEntity> entities)
        {
            //SelectedEntities = MapManager.GetEditorEntities(entities).ToList();
            PropertyDisplayer.UpdateFromEntity(MapComponent.GetEditorEntities(entities).FirstOrDefault());

            if (ViewType != ViewTypes.Perspective) PerspectiveViewModel.Panel.SelectEntities(entities);
            if (ViewType != ViewTypes.X) XViewModel.Panel.SelectEntities(entities);
            if (ViewType != ViewTypes.Y) YViewModel.Panel.SelectEntities(entities);
            if (ViewType != ViewTypes.Z) ZViewModel.Panel.SelectEntities(entities);

            if (entities.Any())
            {
                if (ViewType != ViewTypes.Perspective) PerspectiveViewModel.Panel.UpdateEntities(entities);
                if (ViewType != ViewTypes.X) XViewModel.Panel.UpdateEntities(entities);
                if (ViewType != ViewTypes.Y) YViewModel.Panel.UpdateEntities(entities);
                if (ViewType != ViewTypes.Z) ZViewModel.Panel.UpdateEntities(entities);

                MapComponent.UpdateEntities(entities);
            }

            //var editorEntities = MapManager.GetEditorEntities(entities);
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

        private readonly object _panelLock = new object();
        private bool _isGLContextLoaded = false;
        private bool _isMapLoadedInPanels = false;

        public void UpdateFromModel(MapComponent mapComponent)
        {
            MapComponent = mapComponent;
            GameManager = new GameManager(Resolution);

            EntityMapping = GameManager.LoadFromMap(MapComponent.Map);
            MapComponent.SetEntityMapping(EntityMapping);

            lock (_panelLock)
            {
                // Lock and check to ensure that this only happens once
                if (_isGLContextLoaded && !_isMapLoadedInPanels)
                {
                    PerspectiveViewModel.Panel.LoadGameManager(GameManager, MapComponent.Map, EntityMapping);
                    XViewModel.Panel.LoadGameManager(GameManager, MapComponent.Map, EntityMapping);
                    YViewModel.Panel.LoadGameManager(GameManager, MapComponent.Map, EntityMapping);
                    ZViewModel.Panel.LoadGameManager(GameManager, MapComponent.Map, EntityMapping);

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

                    if (!_isMapLoadedInPanels && MapComponent != null)
                    {
                        // TODO - Determine how to handle the fact that each GamePanel is its own IMouseDelta...
                        PerspectiveViewModel.Panel.LoadGameManager(GameManager, MapComponent.Map, EntityMapping);
                        XViewModel.Panel.LoadGameManager(GameManager, MapComponent.Map, EntityMapping);
                        YViewModel.Panel.LoadGameManager(GameManager, MapComponent.Map, EntityMapping);
                        ZViewModel.Panel.LoadGameManager(GameManager, MapComponent.Map, EntityMapping);

                        _isMapLoadedInPanels = true;
                    }
                }
            }
        }
    }
}