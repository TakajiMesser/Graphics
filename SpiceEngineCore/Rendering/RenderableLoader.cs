using SpiceEngineCore.Entities;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Maps;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpiceEngineCore.Rendering
{
    public abstract class RenderableLoader : IRenderableLoader
    {
        private List<IRenderableBuilder> _componentBuilders = new List<IRenderableBuilder>();
        protected ConcurrentQueue<Tuple<IRenderable, int>> _componentAndIDQueue = new ConcurrentQueue<Tuple<IRenderable, int>>();

        private bool _isProcessing = false;
        private int[] _entityIDs;
        private Task[] _loadTasks;
        private int _taskIndex = 0;

        private int _startBuilderIndex;
        //private int _builderIndex = 0;
        //private readonly object _builderLock = new object();
        private int _currentTick = 0;

        protected IEntityProvider _entityProvider;
        protected List<Tuple<IRenderable, int>> _componentsAndIDs = new List<Tuple<IRenderable, int>>();
        protected Dictionary<int, IRenderable> _componentByID = new Dictionary<int, IRenderable>();

        public bool IsLoaded { get; private set; }
        public int TickRate { get; set; } = 1;

        public event EventHandler<EventArgs> Ticked;
        public event EventHandler<EventArgs> Updated;

        public virtual void SetEntityProvider(IEntityProvider entityProvider) => _entityProvider = entityProvider;

        public void AddBuilder(IMapEntity builder) => _componentBuilders.Add(builder is IRenderableBuilder componentBuilder ? componentBuilder : null);

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

        public virtual Task LoadBuilderAsync(int entityID, IRenderableBuilder builder) => Task.Run(() =>
        {
            var component = builder.ToRenderable();

            if (component != null)
            {
                _componentAndIDQueue.Enqueue(Tuple.Create(component, entityID));
            }
        });

        public virtual void LoadBuilderSync(int entityID, IRenderableBuilder builder) { }

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
            while (_componentAndIDQueue.TryDequeue(out Tuple<IRenderable, int> componentAndID))
            {
                LoadComponent(componentAndID.Item2, componentAndID.Item1);
            }
        }

        protected virtual void LoadComponent(int entityID, IRenderable component)
        {
            _componentsAndIDs.Add(Tuple.Create(component, entityID));
            _componentByID.Add(entityID, component);
        }

        public void Tick()
        {
            _currentTick++;

            if (_currentTick >= TickRate)
            {
                _currentTick = 0;
                Update();
                Updated?.Invoke(this, new EventArgs());
            }

            Ticked?.Invoke(this, new EventArgs());
        }

        protected abstract void Update();
    }
}
