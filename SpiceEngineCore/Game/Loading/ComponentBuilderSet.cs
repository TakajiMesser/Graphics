using SpiceEngineCore.Components;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Maps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpiceEngineCore.Game.Loading
{
    public class ComponentBuilderSet<T> where T : class, IComponentBuilder
    {
        private IComponentLoader<T> _loader;
        private List<T> _builders = new List<T>();

        private readonly object _lock = new object();

        public void SetLoader(IComponentLoader<T> componentLoader)
        {
            lock (_lock)
            {
                _loader = componentLoader;
            }
        }

        public IComponentLoader<T> GetLoader()
        {
            IComponentLoader<T> loader;

            lock (_lock)
            {
                loader = _loader;
            }

            return loader;
        }

        public void AddBuilder(IMapEntity3D mapEntity) => _builders.Add(mapEntity is T t ? t : null);

        public void LoadBuilder(int id, int builderIndex, IComponentLoader<T> componentLoader)
        {
            if (componentLoader != null)
            {
                var shapeBuilder = _builders[builderIndex];

                if (shapeBuilder != null)
                {
                    componentLoader.AddComponent(id, shapeBuilder);
                }
            }
        }

        private bool _isProcessing = false;
        private IComponentLoader<T> _currentLoader;
        private Task[] _loadTasks;
        private int _startIndex;

        public void Begin(int entityCount, int startIndex)
        {
            if (_isProcessing) throw new InvalidOperationException("Components are already being processed");
            _isProcessing = true;

            _loadTasks = new Task[entityCount];
            _startIndex = startIndex;

            _currentLoader = GetLoader();
        }

        public void ProcessTask(int taskIndex, int builderIndex, int entityID)
        {
            _loadTasks[taskIndex] = Task.Run(() => LoadBuilder(entityID, builderIndex, _currentLoader));
        }

        public async Task ProcessTasks()
        {
            var a = "";
            try
            {
                await Task.WhenAll(_loadTasks);
                await _currentLoader?.Load();
            }
            catch (Exception ex)
            {
                a = ex.Message;
                //LogManager.LogToScreen("EXCEPTION: " + ex);
            }

            // TODO - Do we need to add one to this endIndex?
            RemoveBuilders(_startIndex, _startIndex + _loadTasks.Length);
            _isProcessing = false;
        }

        public void RemoveBuilders(int startIndex, int endIndex)
        {
            for (var i = startIndex; i < endIndex; i++)
            {
                _builders[i] = null;
            }
        }
    }
}
