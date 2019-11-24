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
    public class MultiComponentLoader<T, U> : IMultiComponentLoader<T, U> where T : class, IComponent where U : class, IComponentBuilder<T>
    {
        private List<U> _componentBuilders = new List<U>();
        //protected ConcurrentQueue<Tuple<T, int>> _componentAndIDQueue = new ConcurrentQueue<Tuple<T, int>>();

        private bool _isProcessing = false;
        private int[] _entityIDs;
        //private Task[] _loadTasks;
        private int _taskIndex = 0;

        private int _startBuilderIndex;
        //private int _builderIndex = 0;
        //private readonly object _builderLock = new object();

        protected IEntityProvider _entityProvider;
        protected List<Tuple<T, int>> _componentsAndIDs = new List<Tuple<T, int>>();
        protected Dictionary<int, T> _componentByID = new Dictionary<int, T>();

        private int _loaderWaitCount = 0;
        private Task[][] _loadTasks;
        private TaskCompletionSource<bool>[] _loaderAddedTasks;
        private List<IComponentLoader<T, U>> _loaders = new List<IComponentLoader<T, U>>();
        private readonly object _loaderLock = new object();

        public int LoaderWaitCount
        {
            get => _loaderWaitCount;
            set
            {
                lock (_loaderLock)
                {
                    _loaderWaitCount = value;

                    //_loaderAddedTasks = ArrayExtensions.Initialize(value, new TaskCompletionSource<bool>());
                    _loaderAddedTasks = new TaskCompletionSource<bool>[value];

                    for (var i = 0; i < value; i++)
                    {
                        _loaderAddedTasks[i] = new TaskCompletionSource<bool>();
                    }
                }
            }
        }

        public bool IsLoaded { get; private set; }

        public virtual void SetEntityProvider(IEntityProvider entityProvider) => _entityProvider = entityProvider;

        public void AddLoader(IComponentLoader<T, U> loader)
        {
            lock (_loaderLock)
            {
                _loaders.Add(loader);

                if (_loaders.Count <= _loaderAddedTasks.Length)
                {
                    _loaderAddedTasks[_loaders.Count - 1].TrySetResult(true);
                }
            }
        }

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
            //if (_isProcessing) throw new InvalidOperationException("Components are already being processed");
            _isProcessing = true;
            _taskIndex = 0;

            _entityIDs = new int[entityCount];
            _loadTasks = new Task[_loaderWaitCount][];

            for (var i = 0; i < _loaderWaitCount; i++)
            {
                _loadTasks[i] = new Task[entityCount];
                //renderableLoaders[i].InitializeLoad(entityCount, startIndex);
            }
            
            _startBuilderIndex = startIndex;
        }

        public void AddLoadTask(int entityID)
        {
            _entityIDs[_taskIndex] = entityID;
            //_loadTasks[_taskIndex] = LoadBuilderAsync(_startBuilderIndex + _taskIndex);
            var builderIndex = _startBuilderIndex + _taskIndex;

            for (var i = 0; i < _loaderWaitCount; i++)
            {
                var loaderIndex = i;

                _loadTasks[i][_taskIndex] = Task.Run(async () => //LoadRenderableBuilder(id, index, i, renderableLoaders);
                {
                    var builder = _componentBuilders[builderIndex];

                    //await LoadBuilderAsync(entityID, builder);
                    if (loaderIndex < _loaders.Count)
                    {
                        if (builder != null)
                        {
                            await _loaders[loaderIndex].LoadBuilderAsync(entityID, builder);
                        }
                    }
                    else
                    {
                        var result = await _loaderAddedTasks[loaderIndex].Task;

                        if (result && builder != null)
                        {
                            IComponentLoader<T, U> loader;

                            lock (_loaderLock)
                            {
                                loader = _loaders[loaderIndex];
                            }

                            await loader.LoadBuilderAsync(entityID, builder);
                        }
                    }
                });
            }

            _taskIndex++;
        }

        public async Task LoadAsync()
        {
            var multiLoaderTasks = new Task[_loaderWaitCount];

            // Hook up renderable loaders to fire events when all renderable builders have been added for each available render loader
            for (var i = 0; i < _loaderWaitCount; i++)
            {
                var loaderIndex = i;

                multiLoaderTasks[i] = Task.Run(async () =>
                {
                    await LoadBuildersAsync(loaderIndex);

                    IComponentLoader<T, U> loader;

                    lock (_loaderLock)
                    {
                        loader = _loaders[loaderIndex];
                    }

                    LoadBuildersSync(loaderIndex);
                    await loader.InitializeComponents();
                });
            }

            await Task.WhenAll(multiLoaderTasks);

            // TODO - Do we need to add one to this endIndex?
            RemoveBuilders(_startBuilderIndex, _startBuilderIndex + _loadTasks.Length);
            _isProcessing = false;
        }

        /*public async Task InitializeComponents()
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
        }*/

        public void LoadSync()
        {
            var multiLoaderTasks = new Task[_loaderWaitCount];

            // Hook up renderable loaders to fire events when all renderable builders have been added for each available render loader
            for (var i = 0; i < _loaderWaitCount; i++)
            {
                var loaderIndex = i;

                multiLoaderTasks[i] = Task.Run(() =>
                {
                    LoadBuildersAsync(loaderIndex).Wait();

                    IComponentLoader<T, U> loader;

                    lock (_loaderLock)
                    {
                        loader = _loaders[loaderIndex];
                    }

                    LoadBuildersSync(loaderIndex);
                    loader.InitializeComponents().Wait();
                });
            }

            Task.WaitAny(multiLoaderTasks);

            Task.WhenAll(multiLoaderTasks).ContinueWith(t =>
            {
                RemoveBuilders(_startBuilderIndex, _startBuilderIndex + _loadTasks.Length);
                _isProcessing = false;
            });
        }

        protected async Task LoadBuildersAsync(int loaderIndex) => await Task.WhenAll(_loadTasks[loaderIndex]);

        protected virtual void LoadBuildersSync(int loaderIndex)
        {
            for (var i = 0; i < _loadTasks[loaderIndex].Length; i++)
            {
                var entityID = _entityIDs[i];
                var builder = _componentBuilders[i + _startBuilderIndex];

                if (builder != null)
                {
                    _loaders[loaderIndex].LoadBuilderSync(entityID, builder);
                }
            }
        }

        /*protected virtual Task LoadInitial() => Task.Run(() => { });

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
        }*/
    }
}
