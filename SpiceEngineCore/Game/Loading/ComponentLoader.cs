using SpiceEngineCore.Components;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Maps;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpiceEngineCore.Game.Loading
{
    public abstract class ComponentLoader<T, U> : UpdateManager, IComponentLoader<T, U> where T : class, IComponent where U : class, IComponentBuilder<T>
    {
        private List<U> _componentBuilders = new List<U>();
        protected ConcurrentQueue<Tuple<T, int>> _componentAndIDQueue = new ConcurrentQueue<Tuple<T, int>>();

        private bool _isProcessing = false;
        private int[] _entityIDs;
        private Task[] _loadTasks;
        private int _taskIndex = 0;

        private int _startBuilderIndex;
        //private int _builderIndex = 0;
        //private readonly object _builderLock = new object();

        protected IEntityProvider _entityProvider;
        protected List<Tuple<T, int>> _componentsAndIDs = new List<Tuple<T, int>>();
        protected Dictionary<int, T> _componentByID = new Dictionary<int, T>();

        public bool IsLoaded { get; private set; }

        public virtual void SetEntityProvider(IEntityProvider entityProvider) => _entityProvider = entityProvider;

        public void AddBuilder(IMapEntity mapEntity) => _componentBuilders.Add(mapEntity is U builder ? builder : null);

        private void RemoveBuilders(int startIndex, int endIndex)
        {
            for (var i = startIndex; i < endIndex; i++)
            {
                _componentBuilders[i] = null;
            }
        }

        public void InitializeLoad(int entityCount, int startIndex)
        {
            if (_isProcessing) throw new InvalidOperationException("Components are already being processed");
            _isProcessing = true;

            _taskIndex = 0;

            _entityIDs = new int[entityCount];
            _loadTasks = new Task[entityCount];
            
            _startBuilderIndex = startIndex;
        }

        public void AddLoadTask(int entityID)
        {
            _entityIDs[_taskIndex] = entityID;
            //_loadTasks[_taskIndex] = LoadBuilderAsync(_startBuilderIndex + _taskIndex);

            var builderIndex = _startBuilderIndex + _taskIndex;
            _loadTasks[_taskIndex] = Task.Run(async () =>
            {
                var builder = _componentBuilders[builderIndex];

                if (builder != null)
                {
                    await LoadBuilderAsync(entityID, builder);
                }
            });

            _taskIndex++;
        }

        public async Task LoadAsync()
        {
            await LoadBuildersAsync();
            LoadBuildersSync();

            await InitializeComponents();

            // TODO - Do we need to add one to this endIndex?
            RemoveBuilders(_startBuilderIndex, _startBuilderIndex + _loadTasks.Length);
            _isProcessing = false;
        }

        public void LoadSync()
        {
            LoadBuildersAsync().Wait();
            LoadBuildersSync();

            if (!IsLoaded)
            {
                LoadInitial().Wait();
                IsLoaded = true;
            }

            LoadComponents();

            RemoveBuilders(_startBuilderIndex, _startBuilderIndex + _loadTasks.Length);
            _isProcessing = false;
        }

        protected async Task LoadBuildersAsync() => await Task.WhenAll(_loadTasks);

        protected virtual void LoadBuildersSync()
        {
            for (var i = 0; i < _loadTasks.Length; i++)
            {
                var entityID = _entityIDs[i];
                var builder = _componentBuilders[i + _startBuilderIndex];

                if (builder != null)
                {
                    LoadBuilderSync(entityID, builder);
                }
            }
        }

        public virtual Task LoadBuilderAsync(int entityID, U builder) => Task.Run(() =>
        {
            var component = builder.ToComponent();

            if (component != null)
            {
                _componentAndIDQueue.Enqueue(Tuple.Create(component, entityID));
            }
        });

        public virtual void LoadBuilderSync(int entityID, U builder) { }

        public async Task InitializeComponents()
        {
            if (!IsLoaded)
            {
                await LoadInitial();
                LoadComponents();
                IsLoaded = true;
            }
            else
            {
                LoadComponents();
            }
        }

        protected virtual Task LoadInitial() => Task.Run(() => { });

        protected virtual void LoadComponents()
        {
            while (_componentAndIDQueue.TryDequeue(out Tuple<T, int> componentAndID))
            {
                LoadComponent(componentAndID.Item2, componentAndID.Item1);
            }
        }

        protected virtual void LoadComponent(int entityID, T component)
        {
            _componentsAndIDs.Add(Tuple.Create(component, entityID));
            _componentByID.Add(entityID, component);
        }
    }
}
