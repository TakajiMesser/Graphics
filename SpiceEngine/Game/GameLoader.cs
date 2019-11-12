using SpiceEngineCore.Components.Animations;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Game.Loading;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Helpers;
using SpiceEngineCore.Maps;
using SpiceEngineCore.Physics.Shapes;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Scripting;
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

        private IComponentLoader<IShape, IShapeBuilder> _physicsLoader;
        private IComponentLoader<IBehavior, IBehaviorBuilder> _behaviorLoader;
        private IComponentLoader<IAnimator, IAnimatorBuilder> _animatorLoader;

        private IMultiComponentLoader<IRenderable, IRenderableBuilder> _renderableLoader = new MultiComponentLoader<IRenderable, IRenderableBuilder>();

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
            get => _renderableLoader.LoaderWaitCount;
            set => _renderableLoader.LoaderWaitCount = value;
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

        public void SetPhysicsLoader(IComponentLoader<IShape, IShapeBuilder> physicsLoader) => _physicsLoader = physicsLoader;
        public void SetBehaviorLoader(IComponentLoader<IBehavior, IBehaviorBuilder> behaviorLoader) => _behaviorLoader = behaviorLoader;
        public void SetAnimatorLoader(IComponentLoader<IAnimator, IAnimatorBuilder> animatorLoader) => _animatorLoader = animatorLoader;

        public void AddRenderableLoader(IComponentLoader<IRenderable, IRenderableBuilder> renderableLoader) => _renderableLoader.AddLoader(renderableLoader);

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
            _physicsLoader.AddBuilder(mapEntity);
            _behaviorLoader.AddBuilder(mapEntity);
            _animatorLoader.AddBuilder(mapEntity);
            _renderableLoader.AddBuilder(mapEntity);

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
            var a = 3;
            try {
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

            _physicsLoader.InitializeLoad(entityCount, startBuilderIndex);
            _behaviorLoader.InitializeLoad(entityCount, startBuilderIndex);
            _animatorLoader.InitializeLoad(entityCount, startBuilderIndex);
            _renderableLoader.InitializeLoad(entityCount, startBuilderIndex);

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

                    _physicsLoader.AddLoadTask(id);
                    _behaviorLoader.AddLoadTask(id);
                    _animatorLoader.AddLoadTask(id);
                    _renderableLoader.AddLoadTask(id);

                    /*var id = idIterator.Current;
                    var currentBuilderIndex = index;
                    var taskIndex = index - startBuilderIndex;
                    
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
                        _physicsLoader.AddLoadTask(id);
                        _behaviorLoader.AddLoadTask(id);
                        _animatorLoader.AddLoadTask(id);
                    });*/

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
                _physicsLoader.LoadAsync(),
                _behaviorLoader.LoadAsync(),
                _animatorLoader.LoadAsync(),
                _renderableLoader.LoadAsync()
            };
        
            await Task.WhenAll(loadTasks);

            //logWatch.Log("Load Tasks End");
            logWatch.Stop();
            } catch (Exception ex)
            {
                a = 4;
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

            _physicsLoader.InitializeLoad(entityCount, startBuilderIndex);
            _behaviorLoader.InitializeLoad(entityCount, startBuilderIndex);
            _animatorLoader.InitializeLoad(entityCount, startBuilderIndex);
            _renderableLoader.InitializeLoad(entityCount, startBuilderIndex);

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
                    _physicsLoader?.AddLoadTask(id);
                    _behaviorLoader?.AddLoadTask(id);
                    _animatorLoader?.AddLoadTask(id);
                    _renderableLoader.AddLoadTask(id);

                    EntityMapping?.AddID(id);
                    index++;
                }
            }

            EntitiesMapped?.Invoke(this, new EntityMappingEventArgs(EntityMapping));

            _physicsLoader?.LoadSync();
            _behaviorLoader?.LoadSync();
            _animatorLoader?.LoadSync();
            _renderableLoader?.LoadSync();

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
