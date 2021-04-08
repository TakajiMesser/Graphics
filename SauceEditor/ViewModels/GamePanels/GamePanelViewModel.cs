using SauceEditor.ViewModels.Docks;
using SauceEditor.ViewModels.Properties;
using SauceEditor.ViewModels.Trees.Entities;
using SauceEditor.Views.Factories;
using SauceEditorCore.Models.Components;
using SauceEditorCore.Models.Entities;
using SpiceEngine.Game;
using SpiceEngine.Maps;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Layers;
using SpiceEngineCore.Game;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Maps;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Utilities;
using SweetGraphicsCore.Selection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using ViewTypes = SauceEditor.Models.ViewTypes;

namespace SauceEditor.ViewModels
{
    public class GamePanelViewModel : DockableViewModel, ILayerSetter, IMapper
    {
        private GameLoader _gameLoader = new GameLoader(4);

        private readonly object _panelLock = new object();
        private bool _isGLContextLoaded = false;
        private bool _isMapLoadedInPanels = false;

        private Stopwatch _loadWatch = new Stopwatch();
        private int _loadTimeout = 300000;

        public GamePanelViewModel() => _gameLoader.IsInEditorMode = true;

        public IDisplayProperties PropertyDisplayer { get; set; }
        public IDisplayEntities EntityDisplayer { get; set; }
        public IEntityFactory EntityFactory { get; set; }

        public SimulationManager SimulationManager { get; set; }
        public MapComponent MapComponent { get; set; }

        public TransformModes TransformMode { get; set; }
        public ViewTypes ViewType { get; set; }

        public GamePaneViewModel PerspectiveViewModel { get; set; }
        public GamePaneViewModel XViewModel { get; set; }
        public GamePaneViewModel YViewModel { get; set; }
        public GamePaneViewModel ZViewModel { get; set; }

        //public SelectionManager SelectionManager { get; set; }
        //public List<EditorEntity> SelectedEntities { get; set; }

        public Resolution Resolution { get; set; }

        //public event EventHandler<EntitiesEventArgs> EntitySelectionChanged;
        //public event EventHandler<CommandEventArgs> CommandExecuted;

        public void OnResolutionChanged()
        {
            if (SimulationManager == null)
            {
                SimulationManager = new SimulationManager(Resolution);
                SimulationManager.Load();
            }
        }

        public void OnTransformModeChanged()
        {
            PerspectiveViewModel.Viewport.TransformMode = TransformMode;
            XViewModel.Viewport.TransformMode = TransformMode;
            YViewModel.Viewport.TransformMode = TransformMode;
            ZViewModel.Viewport.TransformMode = TransformMode;

            CommandManager.InvalidateRequerySuggested();
        }

        public void OnPerspectiveViewModelChanged() => OnPaneViewModelChange(PerspectiveViewModel);
        public void OnXViewModelChanged() => OnPaneViewModelChange(XViewModel);
        public void OnYViewModelChanged() => OnPaneViewModelChange(YViewModel);
        public void OnZViewModelChanged() => OnPaneViewModelChange(ZViewModel);

        public void AddMapCamera(IMapCamera mapCamera)
        {
            MapComponent.Map.AddCamera(mapCamera);

            _gameLoader.Add(mapCamera);
            _gameLoader.LoadSync();
        }

        public void AddMapBrush(IMapBrush mapBrush)
        {
            MapComponent.Map.AddBrush(mapBrush);

            _gameLoader.Add(mapBrush);
            _gameLoader.LoadSync();
        }

        public void AddMapActor(IMapActor mapActor)
        {
            MapComponent.Map.AddActor(mapActor);

            _gameLoader.Add(mapActor);
            _gameLoader.LoadSync();
        }

        public void AddMapLight(IMapLight mapLight)
        {
            MapComponent.Map.AddLight(mapLight);

            _gameLoader.Add(mapLight);
            _gameLoader.LoadSync();
        }

        public void AddMapVolume(IMapVolume mapVolume)
        {
            MapComponent.Map.AddVolume(mapVolume);

            _gameLoader.Add(mapVolume);
            _gameLoader.LoadSync();
        }

        private void OnPaneViewModelChange(GamePaneViewModel paneViewModel)
        {
            //SelectionManager = panelViewModel.Panel.SelectionManager;
            paneViewModel.EntityProvider = SimulationManager.EntityProvider;
            paneViewModel.GameLoader = _gameLoader;
            paneViewModel.Mapper = this;
            paneViewModel.Viewport.EntitySelectionChanged += (s, args) => UpdatedSelection(args.Entities);
            paneViewModel.Viewport.PanelLoaded += (s, args) =>
            {
                // Because this panel has finished loading in, we can now safely notify the GameLoader that we are ready to load in some RenderBuilders
                //panelViewModel.Panel.LoadGameManager(GameManager, MapComponent.Map);

                // We need to "register" the PanelCamera with the game loader to avoid ID offset problems
                //_gameLoader.Add(mapActor);
                //_gameLoader.LoadSync();
                //paneViewModel.Viewport.Initialize(new SpiceEngineCore.Game.Settings.Configuration(), SimulationManager, MapComponent.Map);
                //paneViewModel.Viewport.LoadSimulation(SimulationManager, MapComponent.Map);
                //LoadPanels();
                _gameLoader.AddRenderableLoader(paneViewModel.Viewport.RenderManager);
            };
            paneViewModel.Viewport.EntityDuplicated += (s, args) => DuplicateEntity(args.Duplication.OriginalID, args.Duplication.DuplicatedID);
            //paneViewModel.Viewport.Initialize(new SpiceEngineCore.Game.Settings.Configuration(), SimulationManager, MapComponent.Map);
            /*panelViewModel.Panel.PanelLoaded += (s, args) =>
            {
                //_gameLoader.AddRenderManager(panelViewModel.Panel.RenderManager);
            };*/
        }

        public void EnableLayer(string layerName)
        {
            // If the layer is enabled, it means that these IDs will get included regardless if they show up in other layers
            SimulationManager.EntityProvider.LayerProvider.SetLayerState(layerName, LayerStates.Enabled);
        }

        public void DisableLayer(string layerName)
        {
            // If the layer is disabled, it means that these IDs will get excluded regardless if they show up in other layers
            SimulationManager.EntityProvider.LayerProvider.SetLayerState(layerName, LayerStates.Disabled);
        }

        public void NeutralizeLayer(string layerName)
        {
            // If the layer is neutralized, it means that these IDs will be included UNLESS they are excluded in another layer
            SimulationManager.EntityProvider.LayerProvider.SetLayerState(layerName, LayerStates.Neutral);
        }

        public void ClearLayer(string layerName)
        {
            // TODO - Remove entities from EntityManager tracking as well
            // Disable the layer and delay removing the entities
            var entityIDs = SimulationManager.EntityProvider.LayerProvider.GetLayerEntityIDs(layerName).ToList();

            SimulationManager.EntityProvider.LayerProvider.SetLayerState(layerName, LayerStates.Disabled);
            SimulationManager.EntityProvider.LayerProvider.ClearLayer(layerName);
            //PerspectiveViewModel.Panel.DelayAction(2, () => GameManager.EntityManager.ClearLayer(layerName));

            foreach (var entityID in entityIDs)
            {
                PerspectiveViewModel.Viewport.RemoveEntity(entityID);
                XViewModel.Viewport.RemoveEntity(entityID);
                YViewModel.Viewport.RemoveEntity(entityID);
                ZViewModel.Viewport.RemoveEntity(entityID);
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
            SimulationManager.EntityProvider.EntitiesAdded += (s, args) =>
            {
                foreach (var builder in args.Builders)
                {
                    SimulationManager.EntityProvider.LayerProvider.AddToLayer(layerName, builder.Item1);

                    if (builder.Item2 is IRenderableBuilder renderableBuilder)
                    {
                        var renderable = renderableBuilder.ToRenderable();

                        PerspectiveViewModel.Viewport.AddEntity(builder.Item1, renderable);
                        XViewModel.Viewport.AddEntity(builder.Item1, renderable);
                        YViewModel.Viewport.AddEntity(builder.Item1, renderable);
                        ZViewModel.Viewport.AddEntity(builder.Item1, renderable);
                    }
                }

                PerspectiveViewModel.Viewport.DoLoad();
                XViewModel.Viewport.DoLoad();
                YViewModel.Viewport.DoLoad();
                ZViewModel.Viewport.DoLoad();
            };

            // Add these entities to a new layer, enable it, and disable all other layers
            if (!SimulationManager.EntityProvider.LayerProvider.ContainsLayer(layerName))
            {
                SimulationManager.EntityProvider.LayerProvider.AddLayer(layerName);
            }

            foreach (var builder in entityBuilders)
            {
                SimulationManager.EntityProvider.AddEntity(builder);
            }

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
            if (ViewType != ViewTypes.Perspective) PerspectiveViewModel.Viewport.SelectEntities(ids);
            if (ViewType != ViewTypes.X) XViewModel.Viewport.SelectEntities(ids);
            if (ViewType != ViewTypes.Y) YViewModel.Viewport.SelectEntities(ids);
            if (ViewType != ViewTypes.Z) ZViewModel.Viewport.SelectEntities(ids);

            CenterView();
        }

        private void UpdatedSelection(IEnumerable<IEntity> entities)
        {
            //SelectedEntities = MapManager.GetEditorEntities(entities).ToList();
            //PropertyDisplayer.UpdateFromEntity(MapComponent.GetEditorEntities(entities).FirstOrDefault());

            if (ViewType != ViewTypes.Perspective) PerspectiveViewModel.Viewport.SelectEntities(entities.Select(e => e.ID));
            if (ViewType != ViewTypes.X) XViewModel.Viewport.SelectEntities(entities.Select(e => e.ID));
            if (ViewType != ViewTypes.Y) YViewModel.Viewport.SelectEntities(entities.Select(e => e.ID));
            if (ViewType != ViewTypes.Z) ZViewModel.Viewport.SelectEntities(entities.Select(e => e.ID));

            if (entities.Any())
            {
                if (ViewType != ViewTypes.Perspective) PerspectiveViewModel.Viewport.UpdateEntities(entities);
                if (ViewType != ViewTypes.X) XViewModel.Viewport.UpdateEntities(entities);
                if (ViewType != ViewTypes.Y) YViewModel.Viewport.UpdateEntities(entities);
                if (ViewType != ViewTypes.Z) ZViewModel.Viewport.UpdateEntities(entities);

                MapComponent.UpdateEntities(entities);
            }

            PropertyDisplayer.UpdateFromEntity(MapComponent.GetEditorEntities(entities).FirstOrDefault());

            //var editorEntities = MapManager.GetEditorEntities(entities);
            //EntitySelectionChanged?.Invoke(this, new EntitiesEventArgs(editorEntities));
            //EntitySelectionChanged?.Invoke(this, new EntitiesEventArgs(_mapManager.GetMapEntities(args.Entities)));
        }

        public void RequestUpdate()
        {
            PerspectiveViewModel.Viewport.Invalidate();
            XViewModel.Viewport.Invalidate();
            YViewModel.Viewport.Invalidate();
            ZViewModel.Viewport.Invalidate();
        }

        public void SetSelectedTool(SpiceEngine.Game.Tools tool)
        {
            /*PerspectiveViewModel.Viewport.SelectedTool = tool;
            XViewModel.Viewport.SelectedTool = tool;
            YViewModel.Viewport.SelectedTool = tool;
            ZViewModel.Viewport.SelectedTool = tool;*/
        }

        public void UpdateEntity(EditorEntity entity)
        {
            PerspectiveViewModel.Viewport.UpdateEntities(entity.Entity.Yield());
            XViewModel.Viewport.UpdateEntities(entity.Entity.Yield());
            YViewModel.Viewport.UpdateEntities(entity.Entity.Yield());
            ZViewModel.Viewport.UpdateEntities(entity.Entity.Yield());
        }

        public void UpdateEntities(IEnumerable<IEntity> entities)
        {
            PerspectiveViewModel.Viewport.UpdateEntities(entities);
            XViewModel.Viewport.UpdateEntities(entities);
            YViewModel.Viewport.UpdateEntities(entities);
            ZViewModel.Viewport.UpdateEntities(entities);
        }

        public void DuplicateEntity(int entityID, int duplicateEntityID)
        {
            /*PerspectiveViewModel.Viewport.Duplicate(entityID, duplicateEntityID);
            XViewModel.Viewport.Duplicate(entityID, duplicateEntityID);
            YViewModel.Viewport.Duplicate(entityID, duplicateEntityID);
            ZViewModel.Viewport.Duplicate(entityID, duplicateEntityID);*/
        }

        public void CenterView()
        {
            PerspectiveViewModel?.Viewport.CenterView();
            XViewModel?.Viewport.CenterView();
            YViewModel?.Viewport.CenterView();
            ZViewModel?.Viewport.CenterView();
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

        /*private IWindowContext _windowContext;
        private IRenderContext _renderContext;

        public WindowWrapper(Configuration configuration)
        {
            _windowContext = WindowContextFactory.SharedInstance.CreateWindowContext(configuration);
            _renderContext = RenderContextFactory.SharedInstance.CreateRenderContext(configuration, _windowContext);
        }

        public void Start(IMap map)
        {
            // TODO - Casting is a poor way to do this, can we instead put the Start call in the IWindowContext interface and handle map loading elsewhere?
            if (_windowContext is GameWindow gameWindow)
            {
                gameWindow.RenderContext = _renderContext;
                gameWindow.Map = map as Map;
                gameWindow.Start();
            }
        }*/

        private async void LoadAsync()
        {
            var configuration = new SpiceEngineCore.Game.Settings.Configuration()
            {
                HandleInputEvents = false,
                Visible = false
            };

            XViewModel.Viewport.Initialize(configuration, SimulationManager, MapComponent.Map);
            YViewModel.Viewport.Initialize(configuration, SimulationManager, MapComponent.Map);
            ZViewModel.Viewport.Initialize(configuration, SimulationManager, MapComponent.Map);
            PerspectiveViewModel.Viewport.Initialize(configuration, SimulationManager, MapComponent.Map);

            //GameManager = new GameManager(Resolution);
            //SimulationManager.LoadFromMap(MapComponent.Map);
            var map = MapComponent.Map as Map3D;
            SimulationManager.PhysicsSystem.SetBoundaries(map.Boundaries);

            // TODO - Make these less janky...
            _gameLoader.TrackEntityMapping = true;
            _gameLoader.SetEntityProvider(SimulationManager.EntityProvider);
            _gameLoader.AddComponentLoader(SimulationManager.PhysicsSystem);
            _gameLoader.AddComponentLoader(SimulationManager.BehaviorSystem);
            _gameLoader.AddComponentLoader(SimulationManager.AnimationSystem);
            _gameLoader.AddComponentLoader(SimulationManager.UISystem);
            //_gameLoader.AddRenderManager(_renderManager);

            _gameLoader.AddFromMap(MapComponent.Map);
            _gameLoader.EntitiesMapped += (s, args) =>
            {
                MapComponent.ClearEntityMapping();
                MapComponent.SetEntityMapping(_gameLoader.EntityMapping);
                EntityDisplayer.UpdateFromModel(MapComponent, EntityFactory);
            };

            //_renderManager.SetEntityProvider(_gameManager.EntityManager);
            //_renderManager.SetCamera(_gameManager.Camera);

            //_gameLoader.Load();
            await _gameLoader.LoadAsync();

            //_renderManager.LoadFromMap(_map);
            //GameManager.BehaviorManager.Load();
        }

        /*private void LoadPanels()
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
                        var configuration = new SpiceEngineCore.Game.Settings.Configuration();

                        PerspectiveViewModel.Viewport.Initialize(configuration, SimulationManager, MapComponent.Map);
                        XViewModel.Viewport.Initialize(configuration, SimulationManager, MapComponent.Map);
                        YViewModel.Viewport.Initialize(configuration, SimulationManager, MapComponent.Map);
                        ZViewModel.Viewport.Initialize(configuration, SimulationManager, MapComponent.Map);

                        _isMapLoadedInPanels = true;
                    }
                }
            }
        }*/
    }
}