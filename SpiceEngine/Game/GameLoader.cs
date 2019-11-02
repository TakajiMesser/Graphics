using SpiceEngine.Rendering.PostProcessing;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Game.Loading;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Helpers;
using SpiceEngineCore.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityMappingEventArgs = SpiceEngineCore.Maps.EntityMappingEventArgs;

namespace SpiceEngine.Game
{
    public class GameLoader : IGameLoader
    {
        private IEntityProvider _entityProvider;
        private IComponentLoader<IShapeBuilder> _physicsLoader;
        private IComponentLoader<IBehaviorBuilder> _behaviorLoader;
        private List<IComponentLoader<IRenderableBuilder>> _renderableLoaders = new List<IComponentLoader<IRenderableBuilder>>();

        private List<IEntityBuilder> _entityBuilders = new List<IEntityBuilder>();
        private List<IShapeBuilder> _shapeBuilders = new List<IShapeBuilder>();
        private List<IBehaviorBuilder> _behaviorBuilders = new List<IBehaviorBuilder>();
        private List<IRenderableBuilder> _renderableBuilders = new List<IRenderableBuilder>();

        private int _rendererWaitCount;
        private TaskCompletionSource<bool>[] _rendererAddedTasks;

        private int _loadIndex = 0;

        private readonly object _builderLock = new object();
        private readonly object _loadLock = new object();

        public EntityMapping EntityMapping { get; private set; } = null;

        public bool TrackEntityMapping
        {
            get => EntityMapping != null;
            set => EntityMapping = value ? new EntityMapping() : null;
        }

        public int RendererWaitCount
        {
            get => _rendererWaitCount;
            set
            {
                lock (_builderLock)
                {
                    _rendererWaitCount = value;

                    //_rendererAddedTasks = ArrayExtensions.Initialize(value, new TaskCompletionSource<bool>());
                    _rendererAddedTasks = new TaskCompletionSource<bool>[value];

                    for (var i = 0; i < value; i++)
                    {
                        _rendererAddedTasks[i] = new TaskCompletionSource<bool>();
                    }
                }
            }
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

        public void SetPhysicsLoader(IComponentLoader<IShapeBuilder> physicsLoader)
        {
            lock (_builderLock)
            {
                _physicsLoader = physicsLoader;
            }
        }

        public void SetBehaviorLoader(IComponentLoader<IBehaviorBuilder> behaviorLoader)
        {
            lock (_builderLock)
            {
                _behaviorLoader = behaviorLoader;
            }
        }

        public void AddRenderableLoader(IComponentLoader<IRenderableBuilder> renderableLoader)
        {
            lock (_builderLock)
            {
                _renderableLoaders.Add(renderableLoader);

                if (_renderableLoaders.Count <= _rendererAddedTasks.Length)
                {
                    _rendererAddedTasks[_renderableLoaders.Count - 1].TrySetResult(true);
                }
            }
        }

        public void Add(IEntityBuilder entityBuilder, IShapeBuilder shapeBuilder, IBehaviorBuilder behaviorBuilder, IRenderableBuilder renderableBuilder)
        {
            lock (_builderLock)
            {
                _entityBuilders.Add(entityBuilder);
                _shapeBuilders.Add(shapeBuilder);
                _behaviorBuilders.Add(behaviorBuilder);
                _renderableBuilders.Add(renderableBuilder);
            }
        }

        public void Add(IMapEntity3D mapEntity)
        {
            lock (_builderLock)
            {
                AddBuilders(mapEntity);
                AddToEntityMapping(mapEntity);
            }
        }

        private void AddBuilders(IMapEntity3D mapEntity)
        {
            _entityBuilders.Add(mapEntity);
            _shapeBuilders.Add(mapEntity is IShapeBuilder shapeBuilder ? shapeBuilder : null);
            _behaviorBuilders.Add(mapEntity is IBehaviorBuilder behaviorBuilder ? behaviorBuilder : null);
            _renderableBuilders.Add(mapEntity is IRenderableBuilder renderableBuilder ? renderableBuilder : null);
        }

        private void AddToEntityMapping(IMapEntity3D mapEntity)
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

                if (EntityMapping != null)
                {
                    EntityMapping.AddCameras(map.CameraCount);
                    EntityMapping.AddBrushes(map.BrushCount);
                    EntityMapping.AddActors(map.ActorCount);
                    EntityMapping.AddLights(map.LightCount);
                    EntityMapping.AddVolumes(map.VolumeCount);
                }
            }
        }

        public async Task LoadAsync()
        {
            //LogWatch logWatch = LogWatch.CreateWithTimeout("GameLoader", 300000, 1000);
            //logWatch.TimedOut += (s, args) => TimedOut?.Invoke(this, args);
            LogWatch logWatch = LogWatch.CreateAndStart("GameLoader");

            // Only process for builders added by the time we begin loading
            var entityCount = 0;
            IComponentLoader<IShapeBuilder> physicsLoader = null;
            IComponentLoader<IBehaviorBuilder> behaviorLoader = null;
            IComponentLoader<IRenderableBuilder>[] renderableLoaders = null;
            var rendererWaitCount = 0;

            lock (_builderLock)
            {
                entityCount = _entityBuilders.Count;
                physicsLoader = _physicsLoader;
                behaviorLoader = _behaviorLoader;
                renderableLoaders = _renderableLoaders.ToArray();
                rendererWaitCount = _rendererWaitCount;
            }

            var startBuilderIndex = 0;

            lock (_loadLock)
            {
                startBuilderIndex = _loadIndex;
                _loadIndex = entityCount;
            }

            entityCount -= startBuilderIndex;

            var loadEntityTasks = new Task[entityCount];
            var loadShapeTasks = new Task[entityCount];
            var loadBehaviorTasks = new Task[entityCount];
            var loadRenderTasks = new Task[rendererWaitCount][];

            for (var i = 0; i < rendererWaitCount; i++)
            {
                loadRenderTasks[i] = new Task[entityCount];
            }

            var index = startBuilderIndex;
            var ids = _entityProvider.AssignEntityIDs(_entityBuilders.Skip(startBuilderIndex).Take(entityCount));

            //logWatch.Log("Loop Start");

            using (var idIterator = ids.GetEnumerator())
            {
                while (idIterator.MoveNext())
                {
                    var id = idIterator.Current;
                    var currentBuilderIndex = index;
                    var taskIndex = currentBuilderIndex - startBuilderIndex;

                    loadEntityTasks[taskIndex] = Task.Run(() =>
                    {
                        // We need to ensure that the entity builder has been loaded before loading ANYTHING else
                        _entityProvider.LoadEntity(id);

                        // Load the renderable data (which we do NOT need to wait for completion on, but which we do need to track)
                        // TODO - Pretty gross to perform the same null check repeatedly in this loop...
                        for (var i = 0; i < rendererWaitCount; i++)
                        {
                            loadRenderTasks[i][taskIndex] = LoadRenderableBuilder(id, currentBuilderIndex, i, renderableLoaders);
                        }

                        // Load the game data (which we NEED to wait for completion on)
                        loadShapeTasks[taskIndex] = Task.Run(() => LoadShapeBuilder(id, currentBuilderIndex, physicsLoader));
                        loadBehaviorTasks[taskIndex] = Task.Run(() => LoadBehaviorBuilder(id, currentBuilderIndex, behaviorLoader));
                    });

                    EntityMapping?.AddID(id);
                    index++;
                }
            }

            EntitiesMapped?.Invoke(this, new EntityMappingEventArgs(EntityMapping));
            //logWatch.Log("Loop End");

            await Task.WhenAll(loadEntityTasks);

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
                Task.Run(async () =>
                {
                    await Task.WhenAll(loadShapeTasks);
                    await physicsLoader.Load();
                    lock (_builderLock)
                    {
                        for (var i = startBuilderIndex; i < index; i++)
                        {
                            _shapeBuilders[i] = null;
                        }
                    }
                }),
                Task.Run(async () =>
                {
                    await Task.WhenAll(loadBehaviorTasks);
                    await behaviorLoader.Load();
                    lock (_builderLock)
                    {
                        for (var i = startBuilderIndex; i < index; i++)
                        {
                            _behaviorBuilders[i] = null;
                        }
                    }
                }),
            };

            // Hook up renderable loaders to fire events when all renderable builders have been added for each available render loader
            for (var i = 0; i < rendererWaitCount; i++)
            {
                var rendererIndex = i;

                loadTasks.Add(Task.Run(async () =>
                {
                    await Task.WhenAll(loadRenderTasks[rendererIndex]);

                    //logWatch.Log("LoadRenderTasks Done");

                    IComponentLoader<IRenderableBuilder> renderableLoader;

                    lock (_builderLock)
                    {
                        renderableLoader = _renderableLoaders[rendererIndex];
                    }

                    //logWatch.Log("Begin Renderer Load");
                    await renderableLoader.Load();
                    //logWatch.Log("End Renderer Load");
                }));
            }
        
            await Task.WhenAll(loadTasks);

            lock (_builderLock)
            {
                for (var i = startBuilderIndex; i < index; i++)
                {
                    _renderableBuilders[i] = null;
                }
            }

            //logWatch.Log("Load Tasks End");

            // TODO - For now, just clear out builders when we're done with them. 
            // Later, we might want to allow some additional loading even while this task is running...
            /*lock (_lock)
            {
                _entityBuilders.Clear();
                _shapeBuilders.Clear();
                _behaviorBuilders.Clear();
                _renderableBuilders.Clear();
            }*/

            logWatch.Stop();
        }

        private async Task LoadRenderableBuilder(int id, int builderIndex, int rendererIndex, IComponentLoader<IRenderableBuilder>[] renderableLoaders)
        {
            IRenderableBuilder renderableBuilder = _renderableBuilders[builderIndex];

            if (renderableBuilder != null)
            {
                if (rendererIndex < renderableLoaders.Length)
                {
                    renderableLoaders[rendererIndex].AddComponent(id, renderableBuilder);
                }
                else
                {
                    var result = await _rendererAddedTasks[rendererIndex].Task;

                    if (result)
                    {
                        IComponentLoader<IRenderableBuilder> renderableLoader;

                        lock (_builderLock)
                        {
                            renderableLoader = _renderableLoaders[rendererIndex];
                        }

                        renderableLoader.AddComponent(id, renderableBuilder);
                    }
                }
            }
        }

        private void LoadShapeBuilder(int id, int builderIndex, IComponentLoader<IShapeBuilder> physicsLoader)
        {
            if (physicsLoader != null)
            {
                var shapeBuilder = _shapeBuilders[builderIndex];

                if (shapeBuilder != null)
                {
                    physicsLoader.AddComponent(id, shapeBuilder);
                }
            }
        }

        private void LoadBehaviorBuilder(int id, int builderIndex, IComponentLoader<IBehaviorBuilder> behaviorLoader)
        {
            if (behaviorLoader != null)
            {
                var behaviorBuilder = _behaviorBuilders[builderIndex];

                if (behaviorBuilder != null)
                {
                    behaviorLoader.AddComponent(id, behaviorBuilder);
                }
            }
        }

        public void Load()
        {
            IsLoading = true;

            // Only process for builders added by the time we begin loading
            var entityCount = 0;
            IComponentLoader<IShapeBuilder> physicsLoader = null;
            IComponentLoader<IBehaviorBuilder> behaviorLoader = null;
            IComponentLoader<IRenderableBuilder>[] renderableLoaders = null;
            var rendererWaitCount = 0;

            lock (_builderLock)
            {
                entityCount = _entityBuilders.Count;
                physicsLoader = _physicsLoader;
                behaviorLoader = _behaviorLoader;
                renderableLoaders = _renderableLoaders.ToArray();
                rendererWaitCount = _rendererWaitCount;
            }

            var startBuilderIndex = 0;

            lock (_loadLock)
            {
                startBuilderIndex = _loadIndex;
                _loadIndex = entityCount;
            }

            entityCount -= startBuilderIndex;

            var loadRenderTasks = new Task[rendererWaitCount][];

            for (var i = 0; i < rendererWaitCount; i++)
            {
                loadRenderTasks[i] = new Task[entityCount];
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

                    LoadShapeBuilder(id, index, physicsLoader);
                    LoadBehaviorBuilder(id, index, behaviorLoader);

                    // TODO - We need to wait for AT LEAST ONE renderer to load this entity, but not for all. We do, however, need to track completion for the others
                    for (var i = 0; i < rendererWaitCount; i++)
                    {
                        loadRenderTasks[i][taskIndex] = LoadRenderableBuilder(id, currentBuilderIndex, i, renderableLoaders);
                    }

                    EntityMapping?.AddID(id);
                    index++;
                }
            }

            EntitiesMapped?.Invoke(this, new EntityMappingEventArgs(EntityMapping));

            physicsLoader.Load();
            behaviorLoader.Load();

            lock (_builderLock)
            {
                // TODO - If we're just setting the value in the list to null, we can do this after each task
                for (var i = startBuilderIndex; i < index; i++)
                {
                    _entityBuilders[i] = null;
                    _shapeBuilders[i] = null;
                    _behaviorBuilders[i] = null;
                }
            }

            var fullyLoadRenderableTasks = new Task[rendererWaitCount];

            for (var i = 0; i < rendererWaitCount; i++)
            {
                var rendererIndex = i;

                fullyLoadRenderableTasks[i] = Task.Run(async () =>
                {
                    // TODO - An exception seems to be happening in this task...
                    try
                    {
                        await Task.WhenAll(loadRenderTasks[rendererIndex]);

                        IComponentLoader<IRenderableBuilder> renderableLoader;

                        lock (_builderLock)
                        {
                            renderableLoader = _renderableLoaders[rendererIndex];
                        }

                        await renderableLoader.Load();
                    }
                    catch (Exception ex)
                    {
                        LogManager.LogToScreen("EXCEPTION: " + ex);
                    }
                });
            }

            // We only need to wait for a single renderer to load all of its builders in before returning from this method
            Task.WaitAny(fullyLoadRenderableTasks);

            // However, we should ensure that ALL renderers complete before we clear out the builders
            Task.WhenAll(fullyLoadRenderableTasks).ContinueWith(t =>
            {
                lock (_builderLock)
                {
                    for (var i = startBuilderIndex; i < index; i++)
                    {
                        _renderableBuilders[i] = null;
                    }
                }
            });

            IsLoading = false;
        }
    }
}
