using SauceEditor.ViewModels.Docks;
using SauceEditor.ViewModels.Properties;
using SauceEditorCore.Models.Components;
using SauceEditorCore.Models.Entities;
using SpiceEngine.Entities;
using SpiceEngine.Entities.Builders;
using SpiceEngine.Entities.Layers;
using SpiceEngine.Game;
using SpiceEngine.Maps;
using SpiceEngine.Outputs;
using SpiceEngine.Rendering.Processing;
using SpiceEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using ViewTypes = SauceEditor.Models.ViewTypes;

namespace SauceEditor.ViewModels
{
    public class GamePanelManagerViewModel : DockViewModel, ILayerSetter
    {
        private GameLoader _gameLoader = new GameLoader();

        private readonly object _panelLock = new object();
        private bool _isGLContextLoaded = false;
        private bool _isMapLoadedInPanels = false;

        private Stopwatch _loadWatch = new Stopwatch();
        private int _loadTimeout = 300000;

        public GamePanelManagerViewModel() : base(DockTypes.Game) => IsPlayable = true;

        public IDisplayProperties PropertyDisplayer { get; set; }

        public GameManager GameManager { get; set; }
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
            panelViewModel.Panel.Load += (s, args) =>
            {
                // Because this panel has finished loading in, we can now safely notify the GameLoader that we are ready to load in some RenderBuilders
                //panelViewModel.Panel.LoadGameManager(GameManager, MapComponent.Map);
                LoadPanels();
                _gameLoader.AddRenderableLoader(panelViewModel.Panel.RenderManager);
            };
            panelViewModel.Panel.EntityDuplicated += (s, args) => DuplicateEntity(args.Duplication.OriginalID, args.Duplication.DuplicatedID);
            /*panelViewModel.Panel.PanelLoaded += (s, args) =>
            {
                //_gameLoader.AddRenderManager(panelViewModel.Panel.RenderManager);
            };*/
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

        public void ClearLayer(string layerName)
        {
            // TODO - Remove entities from EntityManager tracking as well
            // Disable the layer and delay removing the entities
            var entityIDs = GameManager.EntityManager.GetLayerEntityIDs(layerName).ToList();

            GameManager.EntityManager.SetLayerState(layerName, LayerStates.Disabled);
            GameManager.EntityManager.ClearLayer(layerName);
            //PerspectiveViewModel.Panel.DelayAction(2, () => GameManager.EntityManager.ClearLayer(layerName));

            foreach (var entityID in entityIDs)
            {
                PerspectiveViewModel.Panel.RemoveEntity(entityID);
                XViewModel.Panel.RemoveEntity(entityID);
                YViewModel.Panel.RemoveEntity(entityID);
                ZViewModel.Panel.RemoveEntity(entityID);
            }
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

        public void AddToLayer(string layerName, IEnumerable<IEntityBuilder> entityBuilders)
        {
            // TODO - Correct and reorganize this method
            // First, we need to ensure that this layer exists in the LayerManager
            // We then need to add all of these entities to it, then enable the layer
            // In subsequent calls, we just need to disable the root layer, then enable the requested layers
            // An issue is that we want to ONLY render the Root layer for diffuse/lit. The new layer should just be for selection/wireframe
            //var modelEntities = entities.ToList();

            // Assign entity ID's
            /*foreach (var entity in modelEntities)
            {
                var id = GameManager.EntityManager.AddEntity(entity);
                entity.ID = id;
            }*/
            //GameManager.EntityManager.AddEntities(modelEntities);
            GameManager.EntityManager.EntitiesAdded += (s, args) =>
            {
                GameManager.EntityManager.AddEntitiesToLayer(layerName, args.Builders.Select(b => b.Item1));

                foreach (var builder in args.Builders)
                {
                    if (builder.Item2 is IRenderableBuilder renderableBuilder)
                    {
                        var renderable = renderableBuilder.ToRenderable();

                        PerspectiveViewModel.Panel.AddEntity(builder.Item1, renderable);
                        XViewModel.Panel.AddEntity(builder.Item1, renderable);
                        YViewModel.Panel.AddEntity(builder.Item1, renderable);
                        ZViewModel.Panel.AddEntity(builder.Item1, renderable);
                    }
                }

                PerspectiveViewModel.Panel.DoLoad();
                XViewModel.Panel.DoLoad();
                YViewModel.Panel.DoLoad();
                ZViewModel.Panel.DoLoad();
            };

            // Add these entities to a new layer, enable it, and disable all other layers
            if (!GameManager.EntityManager.ContainsLayer(layerName))
            {
                GameManager.EntityManager.AddLayer(layerName);
            }
            GameManager.EntityManager.AddEntities(entityBuilders);

            //GameManager.EntityManager.AddEntitiesToLayer(layerName, modelEntities.Select(e => e.ID));

            //GameManager.EntityManager.SetLayerState(layerName, LayerStates.Enabled);
            //GameManager.EntityManager.SetRenderLayerState(layerName, LayerStates.Enabled);

            /*foreach (var entity in modelEntities)
            {
                var renderable = entity.ToRenderable();

                PerspectiveViewModel.Panel.AddEntity(entity.ID, renderable);
                XViewModel.Panel.AddEntity(entity.ID, renderable);
                YViewModel.Panel.AddEntity(entity.ID, renderable);
                ZViewModel.Panel.AddEntity(entity.ID, renderable);
            }*/

            /*PerspectiveViewModel.Panel.DoLoad();
            XViewModel.Panel.DoLoad();
            YViewModel.Panel.DoLoad();
            ZViewModel.Panel.DoLoad();*/

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

        /*public void LoadAndRun()
        {
            _loadWatch.Start();

            // Begin the background Task for loading the game world
            LoadAsync();

            // Begin a loop that blocks until the game world is loaded (to prevent the window from being disposed)
            while (true)
            {
                ProcessLoadEvents();
            }
        }*/

        public void UpdateFromModel(MapComponent mapComponent)
        {
            MapComponent = mapComponent;
            LoadAsync();
        }

        private async void LoadAsync()
        {
            GameManager = new GameManager(Resolution);
            GameManager.LoadFromMap(MapComponent.Map);

            _gameLoader.RendererWaitCount = 4;
            _gameLoader.SetEntityProvider(GameManager.EntityManager);
            _gameLoader.SetPhysicsLoader(GameManager.PhysicsManager);
            _gameLoader.SetBehaviorLoader(GameManager.BehaviorManager);
            //_gameLoader.AddRenderManager(_renderManager);

            _gameLoader.AddFromMap(MapComponent.Map);

            //_renderManager.SetEntityProvider(_gameManager.EntityManager);
            //_renderManager.SetCamera(_gameManager.Camera);

            //_gameLoader.Load();
            await _gameLoader.LoadAsync();

            MapComponent.SetEntityMapping(_gameLoader.EntityMapping);

            //_renderManager.LoadFromMap(_map);
            //GameManager.BehaviorManager.Load();
        }

        private void LoadPanels()
        {
            // Wait for at least one panel to finish loading so that we can be sure the GLContext is properly loaded
            if (!_isGLContextLoaded)
            {
                lock (_panelLock)
                {
                    // Lock and check to ensure that this only happens once
                    _isGLContextLoaded = true;

                    // TODO - I'm not entirely sure why I needed to have this only happen once when the FIRST panel gets loaded...
                    if (!_isMapLoadedInPanels && MapComponent != null)
                    {
                        // TODO - Determine how to handle the fact that each GamePanel is its own IMouseDelta...
                        PerspectiveViewModel.Panel.LoadGameManager(GameManager, MapComponent.Map);
                        XViewModel.Panel.LoadGameManager(GameManager, MapComponent.Map);
                        YViewModel.Panel.LoadGameManager(GameManager, MapComponent.Map);
                        ZViewModel.Panel.LoadGameManager(GameManager, MapComponent.Map);

                        _isMapLoadedInPanels = true;
                    }
                }
            }
        }
    }
}