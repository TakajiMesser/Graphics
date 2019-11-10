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
    public abstract class MultiComponentLoader<T, U> : UpdateManager, IMultiComponentLoader<T, U> where T : class, IComponent where U : class, IComponentBuilder<T>
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

        private int _multiLoaderWaitCount;
        private Task[][] _multiLoadTasks;
        private TaskCompletionSource<bool>[] _multiLoaderAddedTasks;
        private List<IComponentLoader<T, U>> _multiLoaders = new List<IComponentLoader<T, U>>();
        private readonly object _multiLoaderLock = new object();

        public int MultiLoaderWaitCount
        {
            get => _multiLoaderWaitCount;
            set
            {
                lock (_multiLoaderLock)
                {
                    _multiLoaderWaitCount = value;

                    //_loaderAddedTasks = ArrayExtensions.Initialize(value, new TaskCompletionSource<bool>());
                    _multiLoaderAddedTasks = new TaskCompletionSource<bool>[value];

                    for (var i = 0; i < value; i++)
                    {
                        _multiLoaderAddedTasks[i] = new TaskCompletionSource<bool>();
                    }
                }
            }
        }

        public bool IsLoaded { get; private set; }

        public virtual void SetEntityProvider(IEntityProvider entityProvider) => _entityProvider = entityProvider;

        public void AddLoader(IComponentLoader<T, U> loader)
        {
            lock (_multiLoaderLock)
            {
                _multiLoaders.Add(loader);

                if (_multiLoaders.Count <= _multiLoaderAddedTasks.Length)
                {
                    _multiLoaderAddedTasks[_multiLoaders.Count - 1].TrySetResult(true);
                }
            }
        }

        public void AddBuilder(IMapEntity3D mapEntity) => _componentBuilders.Add(mapEntity is U builder ? builder : null);

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

            _entityIDs = new int[entityCount];
            _loadTasks = new Task[entityCount];
            _multiLoadTasks = new Task[_multiLoaderWaitCount][];

            for (var i = 0; i < _multiLoaderWaitCount; i++)
            {
                _multiLoadTasks[i] = new Task[entityCount];
                //renderableLoaders[i].InitializeLoad(entityCount, startIndex);
            }
            
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

            for (var i = 0; i < _multiLoaderWaitCount; i++)
            {
                var loaderIndex = i;

                _multiLoadTasks[i][_taskIndex] = Task.Run(async () => //LoadRenderableBuilder(id, index, i, renderableLoaders);
                {
                    var builder = _componentBuilders[builderIndex];

                    if (builder != null)
                    {
                        //await LoadBuilderAsync(entityID, builder);
                        if (loaderIndex < _multiLoaders.Count)
                        {
                            await _multiLoaders[loaderIndex].LoadBuilderAsync(entityID, builder);
                        }
                        else
                        {
                            var result = await _multiLoaderAddedTasks[loaderIndex].Task;

                            if (result)
                            {
                                IComponentLoader<T, U> loader;

                                lock (_multiLoaderLock)
                                {
                                    loader = _multiLoaders[loaderIndex];
                                }

                                await loader.LoadBuilderAsync(entityID, builder);
                            }
                        }
                    }
                });
            }

            _taskIndex++;
        }

        public async Task LoadAsync()
        {
            var loadTasks = new List<Task>
            {
                Task.Run(async () =>
                {
                    await LoadBuildersAsync();
                    LoadBuildersSync();

                    if (!IsLoaded)
                    {
                        await LoadInitial();
                        IsLoaded = true;
                    }

                    LoadComponents();
                })
            };

            // Hook up renderable loaders to fire events when all renderable builders have been added for each available render loader
            for (var i = 0; i < _multiLoaderWaitCount; i++)
            {
                var loaderIndex = i;

                loadTasks.Add(Task.Run(async () =>
                {
                    await Task.WhenAll(_multiLoadTasks[loaderIndex]);

                    IComponentLoader<T, U> loader;

                    lock (_multiLoaderLock)
                    {
                        loader = _multiLoaders[loaderIndex];
                    }

                    await loader.LoadAsync();
                }));
            }

            await Task.WhenAll(loadTasks);

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

            var multiLoaderTasks = new Task[_multiLoaderWaitCount];
            for (var i = 0; i < _multiLoaderWaitCount; i++)
            {
                var loaderIndex = i;

                multiLoaderTasks[i] = Task.Run(async () =>
                {
                    await Task.WhenAll(_multiLoadTasks[loaderIndex]);

                    IComponentLoader<T, U> loader;

                    lock (_multiLoaderLock)
                    {
                        loader = _multiLoaders[loaderIndex];
                    }

                    await loader.LoadAsync();
                });
            }

            Task.WhenAll(multiLoaderTasks).ContinueWith(t =>
            {
                RemoveBuilders(_startBuilderIndex, _startBuilderIndex + _loadTasks.Length);
                _isProcessing = false;
            });
        }

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

        protected async Task LoadBuildersAsync()
        {
            var a = 3;
            try
            {
                await Task.WhenAll(_loadTasks);
            }
            catch (Exception ex)
            {
                a = 4;
            }
        }

        protected virtual void LoadBuilderSync(int entityID, U builder) { }

        public virtual Task LoadBuilderAsync(int entityID, U builder) => Task.Run(() =>
        {
            var component = builder.ToComponent();

            if (component != null)
            {
                _componentAndIDQueue.Enqueue(Tuple.Create(component, entityID));
            }
        });

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
