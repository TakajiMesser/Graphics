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

        private object _lock = new object();

        public EntityLoader(IEntityProvider entityProvider) => _entityProvider = entityProvider;

        public int RendererWaitCount
        {
            get => _rendererWaitCount;
            set
            {
                lock (_lock)
                {
                    _rendererWaitCount = value;
                    _rendererAddedTasks = ArrayExtensions.Initialize(value, new TaskCompletionSource<bool>());
                }
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

        public void AddRenderManager(RenderManager renderManager)
        {
            lock (_lock)
            {
                _renderManagers.Add(renderManager);

                if (_renderManagers.Count <= _rendererAddedTasks.Length)
                {
                    _rendererAddedTasks[_renderManagers.Count - 1].TrySetResult(true);
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

            var loadTasks = new List<Task>();

            var ids = _entityProvider.AssignEntityIDs(_entityBuilders.Take(entityCount));
            var index = 0;

            using (var idIterator = ids.GetEnumerator())
            {
                while (idIterator.MoveNext())
                {
                    loadTasks.Add(LoadBuilderAsync(idIterator.Current, index, physicsManager, behaviorManager, renderManagers));
                    index++;
                }
            }

            await Task.WhenAll(loadTasks);
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
