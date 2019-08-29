using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Maps;
using SpiceEngine.Maps.Builders;
using SpiceEngine.Physics;
using SpiceEngine.Rendering;
using SpiceEngine.Scripting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpiceEngine.Entities.Builders
{
    public class EntityLoader
    {
        private IEntityProvider _entityProvider;
        private PhysicsManager _physicsManager;
        private BehaviorManager _behaviorManager;
        private List<RenderManager> _renderManager = new List<RenderManager>();

        private List<IEntityBuilder> _entityBuilders = new List<IEntityBuilder>();
        private List<IShapeBuilder> _shapeBuilders = new List<IShapeBuilder>();
        private List<IBehaviorBuilder> _behaviorBuilders = new List<IBehaviorBuilder>();
        private List<IRenderableBuilder> _renderableBuilders = new List<IRenderableBuilder>();

        private object _lock = new object();

        public EntityLoader(IEntityProvider entityProvider) => _entityProvider = entityProvider;

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
                _renderManager.Add(renderManager);
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
                renderManagers.AddRange(_renderManager);
            }

            var loadTasks = new List<Task>();

            for (var i = 0; i < entityCount; i++)
            {
                var index = i;

                loadTasks.Add(Task.Run(() =>
                {
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
                }));
            }

            await Task.WhenAll(loadTasks);
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
                renderManagers.AddRange(_renderManager);
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
