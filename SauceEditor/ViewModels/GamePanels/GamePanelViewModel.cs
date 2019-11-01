using SauceEditor.ViewModels.Docks;
using SauceEditor.ViewModels.Properties;
using SauceEditor.ViewModels.Trees.Entities;
using SauceEditor.Views.Factories;
using SauceEditorCore.Models.Components;
using SauceEditorCore.Models.Entities;
using SpiceEngine.Game;
using SpiceEngine.Rendering.Processing;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Layers;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Maps;
using SpiceEngineCore.Outputs;
using SpiceEngineCore.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using ViewTypes = SauceEditor.Models.ViewTypes;

namespace SauceEditor.ViewModels
{
    public class GamePanelViewModel : DockableViewModel, ILayerSetter, IMapper
    {
        private GameLoader _gameLoader = new GameLoader();

        private readonly object _panelLock = new object();
        private bool _isGLContextLoaded = false;
        private bool _isMapLoadedInPanels = false;

        private Stopwatch _loadWatch = new Stopwatch();
        private int _loadTimeout = 300000;

        public IDisplayProperties PropertyDisplayer { get; set; }
        public IDisplayEntities EntityDisplayer { get; set; }
        public IEntityFactory EntityFactory { get; set; }

        public GameManager GameManager { get; set; }
        public MapComponent MapComponent { get; set; }

        public TransformModes TransformMode { get; set; }
        public ViewTypes ViewType { get; set; }

        public GamePaneViewModel PerspectiveViewModel { get; set; }
        //public GamePaneViewModel XViewModel { get; set; }
        //public GamePaneViewModel YViewModel { get; set; }
        //public GamePaneViewModel ZViewModel { get; set; }

        //public SelectionManager SelectionManager { get; set; }
        //public List<EditorEntity> SelectedEntities { get; set; }

        public Resolution Resolution { get; set; }

        //public event EventHandler<EntitiesEventArgs> EntitySelectionChanged;
        //public event EventHandler<CommandEventArgs> CommandExecuted;

        public void OnResolutionChanged()
        {
            if (GameManager == null)
            {
                GameManager = new GameManager(Resolution);
            }
        }

        public void OnTransformModeChanged()
        {
            PerspectiveViewModel.Control.TransformMode = TransformMode;
            //XViewModel.Control.TransformMode = TransformMode;
            //YViewModel.Control.TransformMode = TransformMode;
            //ZViewModel.Control.TransformMode = TransformMode;

            CommandManager.InvalidateRequerySuggested();
        }

        public void OnPerspectiveViewModelChanged() => OnPanelViewModelChange(PerspectiveViewModel);
        /*public void OnXViewModelChanged() => OnPanelViewModelChange(XViewModel);
        public void OnYViewModelChanged() => OnPanelViewModelChange(YViewModel);
        public void OnZViewModelChanged() => OnPanelViewModelChange(ZViewModel);*/

        public void AddMapBrush(IMapBrush mapBrush)
        {
            MapComponent.Map.AddBrush(mapBrush);

            _gameLoader.AddFromMapEntity(mapBrush);
            _gameLoader.Load();
        }

        //public void AddMapActor(MapActor mapActor) { }

        public void AddMapVolume(MapVolume mapVolume)
        {
            MapComponent.Map.Volumes.Add(mapVolume);

            _gameLoader.AddFromMapEntity(mapVolume);
            _gameLoader.Load();
        }

        public void AddMapLight(MapLight mapLight)
        {
            MapComponent.Map.Lights.Add(mapLight);

            _gameLoader.AddFromMapEntity(mapLight);
            _gameLoader.Load();
        }

        private void OnPanelViewModelChange(GamePaneViewModel panelViewModel)
        {
            //SelectionManager = panelViewModel.Panel.SelectionManager;
            panelViewModel.EntityProvider = GameManager.EntityManager;
            panelViewModel.GameLoader = _gameLoader;
            panelViewModel.Mapper = this;
            panelViewModel.Control.EntitySelectionChanged += (s, args) => UpdatedSelection(args.Entities);
            panelViewModel.Control.Load += (s, args) =>
            {
                // Because this panel has finished loading in, we can now safely notify the GameLoader that we are ready to load in some RenderBuilders
                //panelViewModel.Panel.LoadGameManager(GameManager, MapComponent.Map);
                LoadPanels();
                _gameLoader.AddRenderableLoader(panelViewModel.Control.RenderManager);
            };
            panelViewModel.Control.EntityDuplicated += (s, args) => DuplicateEntity(args.Duplication.OriginalID, args.Duplication.DuplicatedID);
            /*panelViewModel.Panel.PanelLoaded += (s, args) =>
            {
                //_gameLoader.AddRenderManager(panelViewModel.Panel.RenderManager);
            };*/
        }

        public void EnableLayer(string layerName)
        {
            // If the layer is enabled, it means that these IDs will get included regardless if they show up in other layers
            GameManager.EntityManager.LayerProvider.SetLayerState(layerName, LayerStates.Enabled);
        }

        public void DisableLayer(string layerName)
        {
            // If the layer is disabled, it means that these IDs will get excluded regardless if they show up in other layers
            GameManager.EntityManager.LayerProvider.SetLayerState(layerName, LayerStates.Disabled);
        }

        public void NeutralizeLayer(string layerName)
        {
            // If the layer is neutralized, it means that these IDs will be included UNLESS they are excluded in another layer
            GameManager.EntityManager.LayerProvider.SetLayerState(layerName, LayerStates.Neutral);
        }

        public void ClearLayer(string layerName)
        {
            // TODO - Remove entities from EntityManager tracking as well
            // Disable the layer and delay removing the entities
            var entityIDs = GameManager.EntityManager.LayerProvider.GetLayerEntityIDs(layerName).ToList();

            GameManager.EntityManager.LayerProvider.SetLayerState(layerName, LayerStates.Disabled);
            GameManager.EntityManager.ClearLayer(layerName);
            //PerspectiveViewModel.Panel.DelayAction(2, () => GameManager.EntityManager.ClearLayer(layerName));

            foreach (var entityID in entityIDs)
            {
                PerspectiveViewModel.Control.RemoveEntity(entityID);
                /*XViewModel.Control.RemoveEntity(entityID);
                YViewModel.Control.RemoveEntity(entityID);
                ZViewModel.Control.RemoveEntity(entityID);*/
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
                foreach (var builder in args.Builders)
                {
                    GameManager.EntityManager.LayerProvider.AddToLayer(layerName, builder.Item1);

                    if (builder.Item2 is IRenderableBuilder renderableBuilder)
                    {
                        var renderable = renderableBuilder.ToRenderable();

                        PerspectiveViewModel.Control.AddEntity(builder.Item1, renderable);
                        /*XViewModel.Control.AddEntity(builder.Item1, renderable);
                        YViewModel.Control.AddEntity(builder.Item1, renderable);
                        ZViewModel.Control.AddEntity(builder.Item1, renderable);*/
                    }
                }

                PerspectiveViewModel.Control.DoLoad();
                /*XViewModel.Control.DoLoad();
                YViewModel.Control.DoLoad();
                ZViewModel.Control.DoLoad();*/
            };

            // Add these entities to a new layer, enable it, and disable all other layers
            if (!GameManager.EntityManager.LayerProvider.ContainsLayer(layerName))
            {
                GameManager.EntityManager.LayerProvider.AddLayer(layerName);
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

        public void SelectEntities(IEnumerable<int> ids)
        {
            if (ViewType != ViewTypes.Perspective) PerspectiveViewModel.Control.SelectEntities(ids);
            /*if (ViewType != ViewTypes.X) XViewModel.Control.SelectEntities(ids);
            if (ViewType != ViewTypes.Y) YViewModel.Control.SelectEntities(ids);
            if (ViewType != ViewTypes.Z) ZViewModel.Control.SelectEntities(ids);*/

            CenterView();
        }

        private void UpdatedSelection(IEnumerable<IEntity> entities)
        {
            //SelectedEntities = MapManager.GetEditorEntities(entities).ToList();
            //PropertyDisplayer.UpdateFromEntity(MapComponent.GetEditorEntities(entities).FirstOrDefault());

            if (ViewType != ViewTypes.Perspective) PerspectiveViewModel.Control.SelectEntities(entities.Select(e => e.ID));
            /*if (ViewType != ViewTypes.X) XViewModel.Control.SelectEntities(entities.Select(e => e.ID));
            if (ViewType != ViewTypes.Y) YViewModel.Control.SelectEntities(entities.Select(e => e.ID));
            if (ViewType != ViewTypes.Z) ZViewModel.Control.SelectEntities(entities.Select(e => e.ID));*/

            if (entities.Any())
            {
                if (ViewType != ViewTypes.Perspective) PerspectiveViewModel.Control.UpdateEntities(entities);
                /*if (ViewType != ViewTypes.X) XViewModel.Control.UpdateEntities(entities);
                if (ViewType != ViewTypes.Y) YViewModel.Control.UpdateEntities(entities);
                if (ViewType != ViewTypes.Z) ZViewModel.Control.UpdateEntities(entities);*/

                MapComponent.UpdateEntities(entities);
            }

            PropertyDisplayer.UpdateFromEntity(MapComponent.GetEditorEntities(entities).FirstOrDefault());

            //var editorEntities = MapManager.GetEditorEntities(entities);
            //EntitySelectionChanged?.Invoke(this, new EntitiesEventArgs(editorEntities));
            //EntitySelectionChanged?.Invoke(this, new EntitiesEventArgs(_mapManager.GetMapEntities(args.Entities)));
        }

        public void RequestUpdate()
        {
            PerspectiveViewModel.Control.Invalidate();
            /*XViewModel.Control.Invalidate();
            YViewModel.Control.Invalidate();
            ZViewModel.Control.Invalidate();*/
        }

        public void SetSelectedTool(SpiceEngine.Game.Tools tool)
        {
            PerspectiveViewModel.Control.SelectedTool = tool;
            /*XViewModel.Control.SelectedTool = tool;
            YViewModel.Control.SelectedTool = tool;
            ZViewModel.Control.SelectedTool = tool;*/
        }

        public void UpdateEntity(EditorEntity entity)
        {
            PerspectiveViewModel.Control.UpdateEntities(entity.Entity.Yield());
            /*XViewModel.Control.UpdateEntities(entity.Entity.Yield());
            YViewModel.Control.UpdateEntities(entity.Entity.Yield());
            ZViewModel.Control.UpdateEntities(entity.Entity.Yield());*/
        }

        public void UpdateEntities(IEnumerable<IEntity> entities)
        {
            PerspectiveViewModel.Control.UpdateEntities(entities);
            /*XViewModel.Control.UpdateEntities(entities);
            YViewModel.Control.UpdateEntities(entities);
            ZViewModel.Control.UpdateEntities(entities);*/
        }

        public void DuplicateEntity(int entityID, int duplicateEntityID)
        {
            PerspectiveViewModel.Control.Duplicate(entityID, duplicateEntityID);
            /*XViewModel.Control.Duplicate(entityID, duplicateEntityID);
            YViewModel.Control.Duplicate(entityID, duplicateEntityID);
            ZViewModel.Control.Duplicate(entityID, duplicateEntityID);*/
        }

        public void CenterView()
        {
            PerspectiveViewModel?.Control.CenterView();
            /*XViewModel?.Control.CenterView();
            YViewModel?.Control.CenterView();
            ZViewModel?.Control.CenterView();*/
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
            //GameManager = new GameManager(Resolution);
            GameManager.LoadFromMap(MapComponent.Map);

            // TODO - Make these less janky...
            _gameLoader.RendererWaitCount = 4;
            _gameLoader.TrackEntityMapping = true;
            _gameLoader.SetEntityProvider(GameManager.EntityManager);
            _gameLoader.SetPhysicsLoader(GameManager.PhysicsManager);
            _gameLoader.SetBehaviorLoader(GameManager.BehaviorManager);
            //_gameLoader.AddRenderManager(_renderManager);

            _gameLoader.AddFromMap(MapComponent.Map);
            _gameLoader.EntitiesMapped += (s, args) =>
            {
                MapComponent.ClearEntityMapping();
                MapComponent.SetEntityMap(_gameLoader.EntityMapping);
                EntityDisplayer.UpdateFromModel(MapComponent, EntityFactory);
            };

            //_renderManager.SetEntityProvider(_gameManager.EntityManager);
            //_renderManager.SetCamera(_gameManager.Camera);

            //_gameLoader.Load();
            await _gameLoader.LoadAsync();

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
                        PerspectiveViewModel.Control.LoadGameManager(GameManager, MapComponent.Map);
                        /*XViewModel.Control.LoadGameManager(GameManager, MapComponent.Map);
                        YViewModel.Control.LoadGameManager(GameManager, MapComponent.Map);
                        ZViewModel.Control.LoadGameManager(GameManager, MapComponent.Map);*/

                        _isMapLoadedInPanels = true;
                    }
                }
            }
        }
    }
}