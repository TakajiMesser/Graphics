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

        private List<IEntityBuilder> _entityBuilders = new List<IEntityBuilder>();
        private List<IRenderableBuilder> _renderableBuilders = new List<IRenderableBuilder>();

        private ComponentBuilderSet<IShapeBuilder> _shapeBuilders = new ComponentBuilderSet<IShapeBuilder>();
        private ComponentBuilderSet<IBehaviorBuilder> _behaviorBuilders = new ComponentBuilderSet<IBehaviorBuilder>();
        private ComponentBuilderSet<IAnimatorBuilder> _animatorBuilders = new ComponentBuilderSet<IAnimatorBuilder>();

        private List<IComponentLoader<IRenderableBuilder>> _renderableLoaders = new List<IComponentLoader<IRenderableBuilder>>();

        private int _rendererWaitCount;
        private TaskCompletionSource<bool>[] _rendererAddedTasks;

        private int _loadIndex = 0;

        private readonly object _builderLock = new object();
        private readonly object _loaderLock = new object();

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

        public void SetPhysicsLoader(IComponentLoader<IShapeBuilder> physicsLoader) => _shapeBuilders.SetLoader(physicsLoader);
        public void SetBehaviorLoader(IComponentLoader<IBehaviorBuilder> behaviorLoader) => _behaviorBuilders.SetLoader(behaviorLoader);
        public void SetAnimatorLoader(IComponentLoader<IAnimatorBuilder> animatorLoader) => _animatorBuilders.SetLoader(animatorLoader);

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

        /*public void Add(IEntityBuilder entityBuilder, IShapeBuilder shapeBuilder, IBehaviorBuilder behaviorBuilder, IRenderableBuilder renderableBuilder)
        {
            lock (_builderLock)
            {
                _shapeBuilders.AddBuilder(shapeBuilder);
                _behaviorBuilders.Add(behaviorBuilder);

                _renderableBuilders.Add(renderableBuilder);
                _entityBuilders.Add(entityBuilder);
            }
        }*/

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
            _shapeBuilders.AddBuilder(mapEntity);
            _behaviorBuilders.AddBuilder(mapEntity);
            _animatorBuilders.AddBuilder(mapEntity);

            _renderableBuilders.Add(mapEntity is IRenderableBuilder renderableBuilder ? renderableBuilder : null);
            _entityBuilders.Add(mapEntity);
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
            IComponentLoader<IRenderableBuilder>[] renderableLoaders = null;
            var rendererWaitCount = 0;

            lock (_builderLock)
            {
                entityCount = _entityBuilders.Count;
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
            var loadRenderTasks = new Task[rendererWaitCount][];

            for (var i = 0; i < rendererWaitCount; i++)
            {
                loadRenderTasks[i] = new Task[entityCount];
            }

            _shapeBuilders.Begin(entityCount, startBuilderIndex);
            _behaviorBuilders.Begin(entityCount, startBuilderIndex);
            _animatorBuilders.Begin(entityCount, startBuilderIndex);

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
                        _shapeBuilders.ProcessTask(taskIndex, currentBuilderIndex, id);
                        _behaviorBuilders.ProcessTask(taskIndex, currentBuilderIndex, id);
                        _animatorBuilders.ProcessTask(taskIndex, currentBuilderIndex, id);
                    });

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
                _shapeBuilders.ProcessTasks(),
                _behaviorBuilders.ProcessTasks(),
                _animatorBuilders.ProcessTasks()
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

        public void Load()
        {
            IsLoading = true;

            // Only process for builders added by the time we begin loading
            var entityCount = 0;
            IComponentLoader<IShapeBuilder> physicsLoader = null;
            IComponentLoader<IBehaviorBuilder> behaviorLoader = null;
            IComponentLoader<IAnimatorBuilder> animatorLoader = null;
            IComponentLoader<IRenderableBuilder>[] renderableLoaders = null;
            var rendererWaitCount = 0;

            lock (_builderLock)
            {
                entityCount = _entityBuilders.Count;
                physicsLoader = _shapeBuilders.GetLoader();
                behaviorLoader = _behaviorBuilders.GetLoader();
                animatorLoader = _animatorBuilders.GetLoader();
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

                    _shapeBuilders.LoadBuilder(id, index, physicsLoader);
                    _behaviorBuilders.LoadBuilder(id, index, behaviorLoader);
                    _animatorBuilders.LoadBuilder(id, index, animatorLoader);

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

            physicsLoader?.Load();
            behaviorLoader?.Load();
            animatorLoader?.Load();

            lock (_builderLock)
            {
                // TODO - If we're just setting the value in the list to null, we can do this after each task
                for (var i = startBuilderIndex; i < index; i++)
                {
                    _entityBuilders[i] = null;
                }
            }

            _shapeBuilders.RemoveBuilders(startBuilderIndex, index);
            _behaviorBuilders.RemoveBuilders(startBuilderIndex, index);
            _animatorBuilders.RemoveBuilders(startBuilderIndex, index);

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
