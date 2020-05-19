using SpiceEngineCore.Entities;
using SpiceEngineCore.Maps;
using SpiceEngineCore.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpiceEngineCore.Game.Loading
{
    public class MultiRenderLoader : IMultiRenderLoader
    {
        private List<IRenderableLoader> _loaders = new List<IRenderableLoader>();

        private bool _isProcessing = false;
        private int _loaderWaitCount = 0;
        private TaskCompletionSource<bool>[] _loaderAddedTasks;

        private List<Action>[] _loadActions;

        protected IEntityProvider _entityProvider;

        private readonly object _loaderLock = new object();
        private readonly object _mapEntityLock = new object();
        private readonly object _entityIDLock = new object();

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

                    _loadActions = new List<Action>[_loaderWaitCount];

                    for (var i = 0; i < _loaderWaitCount; i++)
                    {
                        _loadActions[i] = new List<Action>();
                    }
                }
            }
        }

        public bool IsLoaded { get; private set; }

        public virtual void SetEntityProvider(IEntityProvider entityProvider) => _entityProvider = entityProvider;

        public void AddLoader(IRenderableLoader loader)
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

        public void AddBuilder(IMapEntity mapEntity)
        {
            int loaderCount;

            lock (_loaderLock)
            {
                loaderCount = _loaders.Count;
            }

            for (var i = 0; i < _loaderWaitCount; i++)
            {
                var loaderIndex = i;

                /*if (loaderIndex < loaderCount)
                {
                    _loaders[loaderIndex].AddBuilder(mapEntity);
                }
                else
                {*/
                _loadActions[loaderIndex].Add(() => _loaders[loaderIndex].AddBuilder(mapEntity));
                //}
            }
        }

        public void InitializeLoad(int entityCount, int startIndex)
        {
            //if (_isProcessing) throw new InvalidOperationException("Components are already being processed");
            _isProcessing = true;

            int loaderCount;

            lock (_loaderLock)
            {
                loaderCount = _loaders.Count;
            }

            for (var i = 0; i < _loaderWaitCount; i++)
            {
                var loaderIndex = i;

                /*if (loaderIndex < loaderCount)
                {
                    _loaders[loaderIndex].InitializeLoad(entityCount, startIndex);
                }
                else
                {*/
                _loadActions[loaderIndex].Add(() =>
                {
                    _loaders[loaderIndex].InitializeLoad(entityCount, startIndex);
                });
                //}
            }
        }

        public void AddLoadTask(int entityID)
        {
            int loaderCount;

            lock (_loaderLock)
            {
                loaderCount = _loaders.Count;
            }

            for (var i = 0; i < _loaderWaitCount; i++)
            {
                var loaderIndex = i;

                /*if (i < loaderCount)
                {
                    _loaders[i].AddLoadTask(entityID);
                }
                else
                {*/
                _loadActions[loaderIndex].Add(() => _loaders[loaderIndex].AddLoadTask(entityID));
                //}
            }
        }

        public async Task LoadAsync()
        {
            int loaderCount;

            lock (_loaderLock)
            {
                loaderCount = _loaders.Count;
            }

            var multiLoaderTasks = new Task[_loaderWaitCount];

            for (var i = 0; i < _loaderWaitCount; i++)
            {
                var loaderIndex = i;

                /*if (loaderIndex < loaderCount)
                {
                    multiLoaderTasks[loaderIndex] = Task.Run(async () =>
                    {
                        foreach (var loadAction in _loadActions[loaderIndex])
                        {
                            loadAction();
                        }

                        await _loaders[loaderIndex].LoadAsync();
                    });
                }
                else
                {*/
                multiLoaderTasks[loaderIndex] = Task.Run(async () =>
                {
                    var result = await _loaderAddedTasks[loaderIndex].Task;

                    if (result)
                    {
                        foreach (var loadAction in _loadActions[loaderIndex])
                        {
                            loadAction();
                        }

                        IRenderableLoader loader;

                        lock (_loaderLock)
                        {
                            loader = _loaders[loaderIndex];
                        }

                        await loader.LoadAsync();
                    }
                });
                //}
            }

            await Task.WhenAll(multiLoaderTasks);
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
            /*var multiLoaderTasks = new Task[_loaderWaitCount];

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
            });*/
        }

        /*protected async Task LoadBuildersAsync(int loaderIndex) => await Task.WhenAll(_loadTasks[loaderIndex]);

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
        }*/
    }
}
