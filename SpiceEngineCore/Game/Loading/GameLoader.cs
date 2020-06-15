using SpiceEngineCore.Entities;
using SpiceEngineCore.Game.Loading;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Helpers;
using SpiceEngineCore.Maps;
using SpiceEngineCore.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityMappingEventArgs = SpiceEngineCore.Maps.EntityMappingEventArgs;

namespace SpiceEngineCore.Game
{
    public class GameLoader : IGameLoader
    {
        private IEntityProvider _entityProvider;

        private List<IEntityBuilder> _entityBuilders = new List<IEntityBuilder>();

        private List<IComponentLoader> _componentLoaders = new List<IComponentLoader>();
        private MultiRenderLoader _multiRenderLoader = new MultiRenderLoader();

        private int _loadIndex = 0;

        private readonly object _builderLock = new object();
        private readonly object _loadLock = new object();

        public bool IsInEditorMode { get; set; } = false;

        public EntityMapping EntityMapping { get; private set; } = null;

        public bool TrackEntityMapping
        {
            get => EntityMapping != null;
            set => EntityMapping = value ? new EntityMapping() : null;
        }

        public int RendererWaitCount
        {
            get => _multiRenderLoader.LoaderWaitCount;
            set => _multiRenderLoader.LoaderWaitCount = value;
        }

        public bool IsLoading { get; private set; }

        public event EventHandler<EventArgs> TimedOut;
        public event EventHandler<EntityMappingEventArgs> EntitiesMapped;

        public void SetEntityProvider(IEntityProvider entityProvider)
        {
            lock (_builderLock)
            {
                _entityProvider = entityProvider;
            }
        }

        public void AddComponentLoader(IComponentLoader componentLoader) => _componentLoaders.Add(componentLoader);
        public void AddRenderableLoader(IRenderableLoader renderableLoader) => _multiRenderLoader.AddLoader(renderableLoader);

        public void Add(IMapEntity mapEntity)
        {
            lock (_builderLock)
            {
                AddBuilders(mapEntity);
                AddToEntityMapping(mapEntity);
            }
        }

        private void AddBuilders(IMapEntity mapEntity)
        {
            foreach (var componentLoader in _componentLoaders)
            {
                componentLoader.AddBuilder(mapEntity);
            }

            // TODO - Handle this in a cleaner way
            if (!IsInEditorMode && mapEntity is IMapVolume)
            {
                _multiRenderLoader.AddBuilder(null);
            }
            else
            {
                _multiRenderLoader.AddBuilder(mapEntity);
            }

            _entityBuilders.Add(mapEntity);
        }

        private void AddToEntityMapping(IMapEntity mapEntity)
        {
            var entityMapping = EntityMapping;

            if (entityMapping != null)
            {
                if (mapEntity is IMapCamera)
                {
                    entityMapping.AddCameras(1);
                }
                else if (mapEntity is IMapBrush)
                {
                    entityMapping.AddBrushes(1);
                }
                else if (mapEntity is IMapActor)
                {
                    entityMapping.AddActors(1);
                }
                else if (mapEntity is IMapLight)
                {
                    entityMapping.AddLights(1);
                }
                else if (mapEntity is IMapVolume)
                {
                    entityMapping.AddVolumes(1);
                }
                else if (mapEntity is IMapUIItem)
                {
                    entityMapping.AddUIItems(1);
                }
            }
        }

        public void AddFromMap(IMap map)
        {
            lock (_builderLock)
            {
                for (var i = 0; i < map.CameraCount; i++)
                {
                    AddBuilders(map.GetCameraAt(i));
                }

                for (var i = 0; i < map.BrushCount; i++)
                {
                    AddBuilders(map.GetBrushAt(i));
                }

                for (var i = 0; i < map.ActorCount; i++)
                {
                    AddBuilders(map.GetActorAt(i));
                }

                for (var i = 0; i < map.LightCount; i++)
                {
                    AddBuilders(map.GetLightAt(i));
                }

                for (var i = 0; i < map.VolumeCount; i++)
                {
                    AddBuilders(map.GetVolumeAt(i));
                }

                for (var i = 0; i < map.UIItemCount; i++)
                {
                    AddBuilders(map.GetUIItemAt(i));
                }

                if (EntityMapping != null)
                {
                    EntityMapping.AddCameras(map.CameraCount);
                    EntityMapping.AddBrushes(map.BrushCount);
                    EntityMapping.AddActors(map.ActorCount);
                    EntityMapping.AddLights(map.LightCount);
                    EntityMapping.AddVolumes(map.VolumeCount);
                    EntityMapping.AddUIItems(map.UIItemCount);
                }
            }
        }

        public async Task LoadAsync()
        {
            try
            {
                //LogWatch logWatch = LogWatch.CreateWithTimeout("GameLoader", 300000, 1000);
                //logWatch.TimedOut += (s, args) => TimedOut?.Invoke(this, args);
                LogWatch logWatch = LogWatch.CreateAndStart("GameLoader");

                // Only process for builders added by the time we begin loading
                var entityCount = 0;

                lock (_builderLock)
                {
                    entityCount = _entityBuilders.Count;
                }

                var startBuilderIndex = 0;

                lock (_loadLock)
                {
                    startBuilderIndex = _loadIndex;
                    _loadIndex = entityCount;
                }

                entityCount -= startBuilderIndex;

                var loadEntityTasks = new Task[entityCount];

                foreach (var componentLoader in _componentLoaders)
                {
                    componentLoader.InitializeLoad(entityCount, startBuilderIndex);
                }

                _multiRenderLoader.InitializeLoad(entityCount, startBuilderIndex);

                var index = startBuilderIndex;
                var ids = _entityProvider.AssignEntityIDs(_entityBuilders.Skip(startBuilderIndex).Take(entityCount));

                //logWatch.Log("Loop Start");

                using (var idIterator = ids.GetEnumerator())
                {
                    while (idIterator.MoveNext())
                    {
                        var id = idIterator.Current;
                        var taskIndex = index - startBuilderIndex;

                        loadEntityTasks[taskIndex] = Task.Run(() => _entityProvider.LoadEntity(id));

                        foreach (var componentLoader in _componentLoaders)
                        {
                            componentLoader.AddLoadTask(id);
                        }

                        _multiRenderLoader.AddLoadTask(id);

                        EntityMapping?.AddID(id);
                        index++;
                    }
                }

                EntitiesMapped?.Invoke(this, new EntityMappingEventArgs(EntityMapping));
                //logWatch.Log("Loop End");

                await Task.WhenAll(loadEntityTasks);

                _entityProvider.Load();

                lock (_builderLock)
                {
                    // TODO - If we're just setting the value in the list to null, we can do this after each task
                    for (var i = startBuilderIndex; i < index; i++)
                    {
                        _entityBuilders[i] = null;
                    }
                }

                var loadTasks = new List<Task>
                {
                    _multiRenderLoader.LoadAsync()
                };

                foreach (var componentLoader in _componentLoaders)
                {
                    loadTasks.Add(componentLoader.LoadAsync());
                }

                await Task.WhenAll(loadTasks);

                //logWatch.Log("Load Tasks End");
                logWatch.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void LoadSync()
        {
            IsLoading = true;

            // Only process for builders added by the time we begin loading
            var entityCount = 0;

            lock (_builderLock)
            {
                entityCount = _entityBuilders.Count;
            }

            var startBuilderIndex = 0;

            lock (_loadLock)
            {
                startBuilderIndex = _loadIndex;
                _loadIndex = entityCount;
            }

            entityCount -= startBuilderIndex;

            _multiRenderLoader.InitializeLoad(entityCount, startBuilderIndex);

            foreach (var componentLoader in _componentLoaders)
            {
                componentLoader.InitializeLoad(entityCount, startBuilderIndex);
            }

            var index = startBuilderIndex;
            var ids = _entityProvider.AssignEntityIDs(_entityBuilders.Skip(startBuilderIndex).Take(entityCount));

            using (var idIterator = ids.GetEnumerator())
            {
                while (idIterator.MoveNext())
                {
                    var id = idIterator.Current;
                    var currentBuilderIndex = index;
                    var taskIndex = currentBuilderIndex - startBuilderIndex;

                    // We need to ensure that the entity builder has been loaded before loading ANYTHING else
                    _entityProvider.LoadEntity(id);

                    // Load the game data (which we NEED to wait for completion on)
                    _multiRenderLoader?.AddLoadTask(id);

                    foreach (var componentLoader in _componentLoaders)
                    {
                        componentLoader.AddLoadTask(id);
                    }

                    EntityMapping?.AddID(id);
                    index++;
                }
            }

            EntitiesMapped?.Invoke(this, new EntityMappingEventArgs(EntityMapping));

            _multiRenderLoader?.LoadSync();

            foreach (var componentLoader in _componentLoaders)
            {
                componentLoader.LoadSync();
            }

            lock (_builderLock)
            {
                // TODO - If we're just setting the value in the list to null, we can do this after each task
                for (var i = startBuilderIndex; i < index; i++)
                {
                    _entityBuilders[i] = null;
                }
            }

            IsLoading = false;
        }
    }
}
