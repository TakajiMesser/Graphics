using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Maps;
using SpiceEngine.Entities.Builders;
using SpiceEngine.Physics;
using SpiceEngine.Rendering;
using SpiceEngine.Scripting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using SpiceEngine.Utilities;
using SpiceEngine.Rendering.PostProcessing;

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

        private object _lock = new object();

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
                _renderableLoader.Add(renderableLoader);

                if (_renderableLoader.Count <= _rendererAddedTasks.Length)
                {
                    _rendererAddedTasks[_renderableLoader.Count - 1].TrySetResult(true);
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
            var loadRenderTasks = new Task[rendererWaitCount, entityCount];

            var ids = _entityProvider.AssignEntityIDs(_entityBuilders.Take(entityCount));
            var index = 0;

            using (var idIterator = ids.GetEnumerator())
            {
                while (idIterator.MoveNext())
                {
                    var id = idIterator.Current;
                    var currentIndex = index;

                    loadEntityTasks.Add(Task.Run(() =>
                    {
                        // We need to ensure that the entity builder has been loaded before loading ANYTHING else
                        _entityProvider.LoadEntity(id);

                        // Load the renderable data (which we do NOT need to wait for completion on, but which we do need to track)
                        var renderableBuilder = _renderableBuilders[currentIndex];

                        if (renderableBuilder != null)
                        {
                            for (var i = 0; i < rendererWaitCount; i++)
                            {
                                loadRenderTasks[i][currentIndex] = LoadRenderableBuilder(id, renderableBuilder, i, renderableLoaders);
                            }
                        }

                        // Load the game data (which we NEED to wait for completion on)
                        loadShapeTasks.Add(Task.Run(() => LoadShapeBuilder(id, builderIndex, physicsLoader)));
                        loadBehaviorTasks.Add(Task.Run(() => LoadBehaviorBuilder(id, builderIndex, behaviorLoader)));
                    }));

                    index++;
                }
            }

            await Task.WhenAll(loadEntityTasks);

            var loadTasks = new List<Task>();
            loadTasks.Add(Task.WhenAll(loadShapeTasks).ContinueWith(t => physicsLoader.Load()));
            loadTasks.Add(Task.WhenAll(loadBehaviorTasks).ContinueWith(t => behaviorLoader.Load()));

            // Hook up renderable loaders to fire events when all renderable builders have been added for each available render loader
            for (var i = 0; i < rendererWaitCount; i++)
            {
                var rendererIndex = i;

                loadTasks.Add(Task.WhenAll(loadRenderTasks[rendererIndex]).ContinueWith(t =>
                {
                    IEntityLoader<IRenderableBuilder> renderableLoader;

                    lock (_lock)
                    {
                        renderableLoader = _renderableLoaders[rendererIndex];
                    }

                    renderableLoader.Load();
                }));
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

        private async Task LoadRenderableBuilder(int id, IRenderableBuilder renderableBuilder, int rendererIndex, IEntityLoader<IRenderableBuilder>[] renderableLoaders)
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
