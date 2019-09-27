using SpiceEngine.Entities;
using SpiceEngine.Entities.Builders;
using SpiceEngine.Maps;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpiceEngine.Game
{
    public class GameLoader
    {
        private IEntityProvider _entityProvider;
        private IEntityLoader<IShapeBuilder> _physicsLoader;
        private IEntityLoader<IBehaviorBuilder> _behaviorLoader;
        private List<IEntityLoader<IRenderableBuilder>> _renderableLoaders = new List<IEntityLoader<IRenderableBuilder>>();

        private List<IEntityBuilder> _entityBuilders = new List<IEntityBuilder>();
        private List<IShapeBuilder> _shapeBuilders = new List<IShapeBuilder>();
        private List<IBehaviorBuilder> _behaviorBuilders = new List<IBehaviorBuilder>();
        private List<IRenderableBuilder> _renderableBuilders = new List<IRenderableBuilder>();

        private int _rendererWaitCount;
        private TaskCompletionSource<bool>[] _rendererAddedTasks;

        private readonly object _lock = new object();

        private int _brushStartIndex;
        private int _actorStartIndex;
        private int _volumeStartIndex;

        public bool TrackEntityMapping { get; set; }
        public EntityMapping EntityMapping { get; private set; }

        public int RendererWaitCount
        {
            get => _rendererWaitCount;
            set
            {
                lock (_lock)
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

        public void SetEntityProvider(IEntityProvider entityProvider)
        {
            lock (_lock)
            {
                _entityProvider = entityProvider;
            }
        }

        public void SetPhysicsLoader(IEntityLoader<IShapeBuilder> physicsLoader)
        {
            lock (_lock)
            {
                _physicsLoader = physicsLoader;
            }
        }

        public void SetBehaviorLoader(IEntityLoader<IBehaviorBuilder> behaviorLoader)
        {
            lock (_lock)
            {
                _behaviorLoader = behaviorLoader;
            }
        }

        public void AddRenderableLoader(IEntityLoader<IRenderableBuilder> renderableLoader)
        {
            lock (_lock)
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
            lock (_lock)
            {
                _entityBuilders.Add(entityBuilder);
                _shapeBuilders.Add(shapeBuilder);
                _behaviorBuilders.Add(behaviorBuilder);
                _renderableBuilders.Add(renderableBuilder);
            }
        }

        public void AddFromMap(Map map)
        {
            lock (_lock)
            {
                foreach (var light in map.Lights)
                {
                    _entityBuilders.Add(light);
                    _shapeBuilders.Add(null);
                    _behaviorBuilders.Add(null);
                    _renderableBuilders.Add(null);
                }

                foreach (var brush in map.Brushes)
                {
                    _entityBuilders.Add(brush);
                    _shapeBuilders.Add(brush);
                    _behaviorBuilders.Add(null);
                    _renderableBuilders.Add(brush);
                }

                foreach (var actor in map.Actors)
                {
                    _entityBuilders.Add(actor);
                    _shapeBuilders.Add(actor);
                    _behaviorBuilders.Add(actor);
                    _renderableBuilders.Add(actor);
                }

                foreach (var volume in map.Volumes)
                {
                    _entityBuilders.Add(volume);
                    _shapeBuilders.Add(volume);
                    _behaviorBuilders.Add(null);
                    _renderableBuilders.Add(null);
                }

                if (TrackEntityMapping)
                {
                    _brushStartIndex = map.Lights.Count;
                    _actorStartIndex = map.Brushes.Count;
                    _volumeStartIndex = map.Actors.Count;
                }
            }
        }

        public async Task LoadAsync()
        {
            // Only process for builders added by the time we begin loading
            var entityCount = 0;
            IEntityLoader<IShapeBuilder> physicsLoader = null;
            IEntityLoader<IBehaviorBuilder> behaviorLoader = null;
            IEntityLoader<IRenderableBuilder>[] renderableLoaders = null;
            var rendererWaitCount = 0;

            lock (_lock)
            {
                entityCount = _entityBuilders.Count;
                physicsLoader = _physicsLoader;
                behaviorLoader = _behaviorLoader;
                renderableLoaders = _renderableLoaders.ToArray();
                rendererWaitCount = _rendererWaitCount;
            }

            var loadEntityTasks = new Task[entityCount];
            var loadShapeTasks = new Task[entityCount];
            var loadBehaviorTasks = new Task[entityCount];
            var loadRenderTasks = new Task[rendererWaitCount][];

            for (var i = 0; i < rendererWaitCount; i++)
            {
                loadRenderTasks[i] = new Task[entityCount];
            }

            var ids = _entityProvider.AssignEntityIDs(_entityBuilders.Take(entityCount));
            var index = 0;

            List<int> lightIDs = null;
            List<int> brushIDs = null;
            List<int> actorIDs = null;
            List<int> volumeIDs = null;

            if (TrackEntityMapping)
            {
                lightIDs = new List<int>();
                brushIDs = new List<int>();
                actorIDs = new List<int>();
                volumeIDs = new List<int>();
            }

            using (var idIterator = ids.GetEnumerator())
            {
                while (idIterator.MoveNext())
                {
                    var id = idIterator.Current;
                    var currentIndex = index;

                    loadEntityTasks[currentIndex] = Task.Run(() =>
                    {
                        // We need to ensure that the entity builder has been loaded before loading ANYTHING else
                        _entityProvider.LoadEntity(id);

                        // Load the renderable data (which we do NOT need to wait for completion on, but which we do need to track)
                        /*var renderableBuilder = _renderableBuilders[currentIndex];

                        if (renderableBuilder != null)
                        {
                            for (var i = 0; i < rendererWaitCount; i++)
                            {
                                loadRenderTasks[i][currentIndex] = LoadRenderableBuilder(id, renderableBuilder, i, renderableLoaders);
                            }
                        }*/

                        // TODO - Pretty gross to perform the same null check repeatedly in this loop...
                        for (var i = 0; i < rendererWaitCount; i++)
                        {
                            loadRenderTasks[i][currentIndex] = LoadRenderableBuilder(id, currentIndex, i, renderableLoaders);
                        }

                        // Load the game data (which we NEED to wait for completion on)
                        loadShapeTasks[currentIndex] = Task.Run(() => LoadShapeBuilder(id, currentIndex, physicsLoader));
                        loadBehaviorTasks[currentIndex] = Task.Run(() => LoadBehaviorBuilder(id, currentIndex, behaviorLoader));
                    });

                    if (TrackEntityMapping)
                    {
                        if (currentIndex >= _volumeStartIndex)
                        {
                            volumeIDs.Add(id);
                        }
                        else if (currentIndex >= _actorStartIndex)
                        {
                            actorIDs.Add(id);
                        }
                        else if (currentIndex >= _brushStartIndex)
                        {
                            brushIDs.Add(id);
                        }
                        else
                        {
                            lightIDs.Add(id);
                        }
                    }

                    index++;
                }
            }

            if (TrackEntityMapping)
            {
                EntityMapping = new EntityMapping(actorIDs, brushIDs, volumeIDs, lightIDs);
            }

            await Task.WhenAll(loadEntityTasks);

            var loadTasks = new List<Task>
            {
                Task.Run(async () =>
                {
                    await Task.WhenAll(loadShapeTasks);
                    await physicsLoader.Load();
                }),
                Task.Run(async () =>
                {
                    await Task.WhenAll(loadBehaviorTasks);
                    await behaviorLoader.Load();
                }),
                //Task.WhenAll(loadShapeTasks).ContinueWith(async t => await physicsLoader.Load()),
                //Task.WhenAll(loadBehaviorTasks).ContinueWith(async t => await behaviorLoader.Load())
            };

            // Hook up renderable loaders to fire events when all renderable builders have been added for each available render loader
            for (var i = 0; i < rendererWaitCount; i++)
            {
                var rendererIndex = i;

                loadTasks.Add(Task.Run(async () =>
                {
                    await Task.WhenAll(loadRenderTasks[rendererIndex]);

                    IEntityLoader<IRenderableBuilder> renderableLoader;

                    lock (_lock)
                    {
                        renderableLoader = _renderableLoaders[rendererIndex];
                    }

                    await renderableLoader.Load();
                }));

                /*loadTasks.Add(Task.WhenAll(loadRenderTasks[rendererIndex]).ContinueWith(async t =>
                {
                    IEntityLoader<IRenderableBuilder> renderableLoader;

                    lock (_lock)
                    {
                        renderableLoader = _renderableLoaders[rendererIndex];
                    }

                    await renderableLoader.Load();
                }));*/
            }

            await Task.WhenAll(loadTasks);

            // TODO - For now, just clear out builders when we're done with them. 
            // Later, we might want to allow some additional loading even while this task is running...
            lock (_lock)
            {
                _entityBuilders.Clear();
                _shapeBuilders.Clear();
                _behaviorBuilders.Clear();
                _renderableBuilders.Clear();
            }
        }

        private async Task LoadRenderableBuilder(int id, int builderIndex, int rendererIndex, IEntityLoader<IRenderableBuilder>[] renderableLoaders)
        {
            IRenderableBuilder renderableBuilder = _renderableBuilders[builderIndex];

            if (renderableBuilder != null)
            {
                if (rendererIndex < renderableLoaders.Length)
                {
                    renderableLoaders[rendererIndex].AddEntity(id, renderableBuilder);
                }
                else
                {
                    var result = await _rendererAddedTasks[rendererIndex].Task;

                    if (result)
                    {
                        IEntityLoader<IRenderableBuilder> renderableLoader;

                        lock (_lock)
                        {
                            renderableLoader = _renderableLoaders[rendererIndex];
                        }

                        renderableLoader.AddEntity(id, renderableBuilder);
                    }
                }
            }
        }

        private void LoadShapeBuilder(int id, int builderIndex, IEntityLoader<IShapeBuilder> physicsLoader)
        {
            if (physicsLoader != null)
            {
                var shapeBuilder = _shapeBuilders[builderIndex];

                if (shapeBuilder != null)
                {
                    physicsLoader.AddEntity(id, shapeBuilder);
                }
            }
        }

        private void LoadBehaviorBuilder(int id, int builderIndex, IEntityLoader<IBehaviorBuilder> behaviorLoader)
        {
            if (behaviorLoader != null)
            {
                var behaviorBuilder = _behaviorBuilders[builderIndex];

                if (behaviorBuilder != null)
                {
                    behaviorLoader.AddEntity(id, behaviorBuilder);
                }
            }
        }

        public void Load()
        {
            // Only process for builders added by the time we begin loading
            var entityCount = 0;
            IEntityLoader<IShapeBuilder> physicsLoader = null;
            IEntityLoader<IBehaviorBuilder> behaviorLoader = null;
            IEntityLoader<IRenderableBuilder>[] renderableLoaders;

            lock (_lock)
            {
                entityCount = _entityBuilders.Count;
                physicsLoader = _physicsLoader;
                behaviorLoader = _behaviorLoader;
                renderableLoaders = _renderableLoaders.ToArray();
            }

            for (var i = 0; i < entityCount; i++)
            {
                var index = i;

                var entityBuilder = _entityBuilders[index];
                var id = _entityProvider.AddEntity(entityBuilder);

                if (physicsLoader != null)
                {
                    var shapeBuilder = _shapeBuilders[index];

                    if (shapeBuilder != null)
                    {
                        physicsLoader.AddEntity(id, shapeBuilder);
                    }
                }

                if (behaviorLoader != null)
                {
                    var behaviorBuilder = _behaviorBuilders[index];

                    if (behaviorBuilder != null)
                    {
                        behaviorLoader.AddEntity(id, behaviorBuilder);
                    }
                }

                if (renderableLoaders.Length > 0)
                {
                    var renderableBuilder = _renderableBuilders[index];

                    if (renderableBuilder != null)
                    {
                        foreach (var renderableLoader in renderableLoaders)
                        {
                            renderableLoader.AddEntity(id, renderableBuilder);
                        }
                    }
                }
            }
        }
    }
}
