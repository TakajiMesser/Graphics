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

namespace SpiceEngine.Entities.Builders
{
    public class EntityLoader
    {
        private IEntityProvider _entityProvider;
        private PhysicsManager _physicsManager;
        private BehaviorManager _behaviorManager;
        private List<RenderManager> _renderManagers = new List<RenderManager>();

        private List<IEntityBuilder> _entityBuilders = new List<IEntityBuilder>();
        private List<IShapeBuilder> _shapeBuilders = new List<IShapeBuilder>();
        private List<IBehaviorBuilder> _behaviorBuilders = new List<IBehaviorBuilder>();
        private List<IRenderableBuilder> _renderableBuilders = new List<IRenderableBuilder>();

        private int _rendererWaitCount;
        private TaskCompletionSource<bool>[] _rendererAddedTasks;
        private List<string> _rendererNames = new List<string>();

        // TODO - This is atrocious...
        private Dictionary<int, int> _nameIndexByRendererIndex = new Dictionary<int, int>();

        private object _lock = new object();

        public event EventHandler<RendererLoadEventArgs> RendererLoaded;

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

        public void SetPhysicsManager(PhysicsManager physicsManager)
        {
            lock (_lock)
            {
                _physicsManager = physicsManager;
            }
        }

        public void SetBehaviorManager(BehaviorManager behaviorManager)
        {
            lock (_lock)
            {
                _behaviorManager = behaviorManager;
            }
        }

        public void AddRenderManager(string name, RenderManager renderManager)
        {
            lock (_lock)
            {
                _renderManagers.Add(renderManager);
                
                // TODO - This is atrocious...
                for (var i = 0; i < _rendererNames.Count; i++)
                {
                    if (_rendererNames[i] == name)
                    {
                        _nameIndexByRendererIndex.Add(_renderManagers.Count - 1, i);
                    }
                }

                if (_renderManagers.Count <= _rendererAddedTasks.Length)
                {
                    _rendererAddedTasks[_renderManagers.Count - 1].TrySetResult(true);
                }
            }
        }

        public void SetRenderManagerNames(params string[] names)
        {
            lock (_lock)
            {
                _rendererNames.Clear();
                _rendererNames.AddRange(names);
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
            PhysicsManager physicsManager = null;
            BehaviorManager behaviorManager = null;
            var renderManagers = new List<RenderManager>();
            var rendererNames = new List<string>();
            var rendererWaitCount = 0;

            lock (_lock)
            {
                entityCount = _entityBuilders.Count;
                physicsManager = _physicsManager;
                behaviorManager = _behaviorManager;
                renderManagers.AddRange(_renderManagers);
                rendererNames.AddRange(_rendererNames);
                rendererWaitCount = _rendererWaitCount;
            }

            var loadTasks = new List<Task>();
            var loadGameTasks = new List<Task>();
            var loadRenderTasks = new List<Task>[rendererWaitCount];

            for (var i = 0; i < rendererWaitCount; i++)
            {
                loadRenderTasks[i] = new List<Task>();
            }

            var ids = _entityProvider.AssignEntityIDs(_entityBuilders.Take(entityCount));
            var index = 0;

            using (var idIterator = ids.GetEnumerator())
            {
                while (idIterator.MoveNext())
                {
                    var id = idIterator.Current;
                    var currentIndex = index;

                    loadTasks.Add(Task.Run(async () =>
                    {
                        // We need to ensure that the entity builder has been loaded before loading ANYTHING else
                        _entityProvider.LoadEntity(id);

                        // Load the renderable data (which we do NOT need to wait for completion on, but which we do need to track)
                        var renderableBuilder = _renderableBuilders[currentIndex];

                        if (renderableBuilder != null)
                        {
                            for (var i = 0; i < rendererWaitCount; i++)
                            {
                                loadRenderTasks[i].Add(LoadRenderableBuilder(id, renderableBuilder, i, renderManagers));
                            }
                        }

                        // Load the game data (which we NEED to wait for completion on)
                        await LoadGameBuilderAsync(id, currentIndex, physicsManager, behaviorManager);
                    }));

                    index++;

                    //loadTasks.Add(LoadBuilderAsync(idIterator.Current, index, physicsManager, behaviorManager, renderManagers));
                    //index++;
                }
            }

            await Task.WhenAll(loadTasks);

            // Hook up renderable loaders to fire events when all renderable builders have been added for each available render manager
            for (var i = 0; i < rendererWaitCount; i++)
            {
                var rendererIndex = i;
                var a = 3;

                try
                {
                    _ = Task.WhenAll(loadRenderTasks[rendererIndex]).ContinueWith(t =>
                    {
                        string name;

                        lock (_lock)
                        {
                            var nameIndex = _nameIndexByRendererIndex[rendererIndex];
                            name = _rendererNames[nameIndex];
                        }

                        RendererLoaded?.Invoke(this, new RendererLoadEventArgs(name));
                    });
                }
                catch (Exception ex)
                {
                    a = 4;
                }
            }
        }

        private async Task LoadRenderableBuilder(int id, IRenderableBuilder renderableBuilder, int rendererIndex, List<RenderManager> renderManagers)
        {
            if (rendererIndex < renderManagers.Count)
            {
                renderManagers[rendererIndex].AddEntity(id, renderableBuilder);
            }
            else
            {
                var result = await _rendererAddedTasks[rendererIndex].Task;

                if (result)
                {
                    RenderManager renderManager;

                    lock (_lock)
                    {
                        renderManager = _renderManagers[rendererIndex];
                    }

                    renderManager.AddEntity(id, renderableBuilder);
                }
            }
        }

        private async Task LoadGameBuilderAsync(int id, int builderIndex, PhysicsManager physicsManager, BehaviorManager behaviorManager)
        {
            var loadShapeTask = Task.Run(() => LoadShapeBuilder(id, builderIndex, physicsManager));
            var loadBehaviorTask = Task.Run(() => LoadBehaviorBuilder(id, builderIndex, behaviorManager));

            await Task.WhenAll(loadShapeTask, loadBehaviorTask);
        }

        private async Task LoadBuilderAsync(int id, int builderIndex, PhysicsManager physicsManager, BehaviorManager behaviorManager, List<RenderManager> renderManagers)
        {
            _entityProvider.LoadEntity(id);

            var loadBuilderTasks = new List<Task>
            {
                Task.Run(() => LoadShapeBuilder(id, builderIndex, physicsManager)),
                Task.Run(() => LoadBehaviorBuilder(id, builderIndex, behaviorManager)),
                LoadRenderableBuilder(id, builderIndex, renderManagers)
            };

            await Task.WhenAll(loadBuilderTasks);
        }

        private void LoadShapeBuilder(int id, int builderIndex, PhysicsManager physicsManager)
        {
            if (physicsManager != null)
            {
                var shapeBuilder = _shapeBuilders[builderIndex];

                if (shapeBuilder != null)
                {
                    physicsManager.AddEntity(id, shapeBuilder);
                }
            }
        }

        private void LoadBehaviorBuilder(int id, int builderIndex, BehaviorManager behaviorManager)
        {
            if (behaviorManager != null)
            {
                var behaviorBuilder = _behaviorBuilders[builderIndex];

                if (behaviorBuilder != null)
                {
                    behaviorManager.AddEntity(id, behaviorBuilder);
                }
            }
        }

        // TODO - This isn't gonna work at all :(
        /*
            We are adding RenderableBuilders in a loop, so by the time we get to this method, we are looking at a single builder
            Unfortunately, we want to know when ALL RenderableBuilders have been added successfully to a single RenderManager
            (So we can report to that RenderManager that it can now safely invoke all batch loading and shit on the UI thread)
            So... we need to restructure the way these tasks are being split up and managed
        */
        private async Task LoadRenderableBuilder(int id, int builderIndex, List<RenderManager> renderManagers)
        {
            var renderableBuilder = _renderableBuilders[builderIndex];

            if (renderableBuilder != null)
            {
                var addBuilderTasks = new List<Task>();

                for (var i = 0; i < _rendererWaitCount; i++)
                {
                    if (i < renderManagers.Count)
                    {
                        renderManagers[i].AddEntity(id, renderableBuilder);
                    }
                    else
                    {
                        var rendererIndex = i;

                        addBuilderTasks.Add(Task.Run(async () =>
                        {
                            var result = await _rendererAddedTasks[rendererIndex].Task;

                            if (result)
                            {
                                RenderManager renderManager;

                                lock (_lock)
                                {
                                    renderManager = _renderManagers[rendererIndex];
                                }

                                renderManager.AddEntity(id, renderableBuilder);
                            }
                        }));
                    }
                }

                if (addBuilderTasks.Count > 0)
                {
                    await Task.WhenAll(addBuilderTasks);
                }
            }
        }

        public void Load()
        {
            // Only process for builders added by the time we begin loading
            var entityCount = 0;
            PhysicsManager physicsManager = null;
            BehaviorManager behaviorManager = null;
            var renderManagers = new List<RenderManager>();

            lock (_lock)
            {
                entityCount = _entityBuilders.Count;
                physicsManager = _physicsManager;
                behaviorManager = _behaviorManager;
                renderManagers.AddRange(_renderManagers);
            }

            for (var i = 0; i < entityCount; i++)
            {
                var index = i;

                var entityBuilder = _entityBuilders[index];
                var id = _entityProvider.AddEntity(entityBuilder);

                if (physicsManager != null)
                {
                    var shapeBuilder = _shapeBuilders[index];

                    if (shapeBuilder != null)
                    {
                        physicsManager.AddEntity(id, shapeBuilder);
                    }
                }

                if (behaviorManager != null)
                {
                    var behaviorBuilder = _behaviorBuilders[index];

                    if (behaviorBuilder != null)
                    {
                        behaviorManager.AddEntity(id, behaviorBuilder);
                    }
                }

                if (renderManagers.Count > 0)
                {
                    var renderableBuilder = _renderableBuilders[index];

                    if (renderableBuilder != null)
                    {
                        foreach (var renderManager in renderManagers)
                        {
                            renderManager.AddEntity(id, renderableBuilder);
                        }
                    }
                }
            }
        }
    }
}
