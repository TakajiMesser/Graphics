using SpiceEngineCore.Components;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Rendering;
using System;
using System.Collections.Generic;

namespace SpiceEngineCore.Game
{
    public class SystemProvider : ISystemProvider
    {
        private IEntityProvider _entityProvider;
        private IRenderProvider _renderProvider;
        
        private Dictionary<Type, IComponentProvider> _componentProviderByType = new Dictionary<Type, IComponentProvider>();
        private Dictionary<Type, IGameSystem> _gameSystemByType = new Dictionary<Type, IGameSystem>();

        public void SetEntityProvider(IEntityProvider entityProvider) => _entityProvider = entityProvider;
        public void SetRenderProvider(IRenderProvider renderProvider) => _renderProvider = renderProvider;

        public void AddComponentProvider<T>(IComponentProvider componentProvider) where T : IComponent
        {
            if (!(componentProvider is IComponentProvider<T>)) throw new ArgumentException("Component Provider must provide type " + typeof(T).Name);
            _componentProviderByType.Add(typeof(T), componentProvider);
        }

        public void AddGameSystem<T>(T gameSystem) where T : IGameSystem => _gameSystemByType.Add(typeof(T), gameSystem);

        public IEntityProvider GetEntityProvider() => _entityProvider;
        public IRenderProvider GetRenderProvider() => _renderProvider;

        public IComponentProvider<T> GetComponentProvider<T>() where T : IComponent => (IComponentProvider<T>)_componentProviderByType[typeof(T)];
        public IComponentProvider<T> GetComponentProviderOrDefault<T>() where T : IComponent => HasComponentProvider<T>() ? GetComponentProvider<T>() : default;

        public T GetGameSystem<T>() where T : IGameSystem => (T)_gameSystemByType[typeof(T)];
        public T GetGameSystemOrDefault<T>() where T : IGameSystem => HasGameSystem<T>() ? GetGameSystem<T>() : default;

        public bool HasComponentProvider<T>() where T : IComponent => _componentProviderByType.ContainsKey(typeof(T));
        public bool HasGameSystem<T>() where T : IGameSystem => _gameSystemByType.ContainsKey(typeof(T));

        public IEntity GetEntity(int entityID) => _entityProvider.GetEntity(entityID);
        public IEntity GetEntityOrDefault(int entityID) => _entityProvider.GetEntityOrDefault(entityID);

        public IRenderable GetRenderable(int entityID) => _renderProvider.GetRenderable(entityID);
        public IRenderable GetRenderableOrDefault(int entityID) => _renderProvider.GetRenderableOrDefault(entityID);
        public bool HasRenderable(int entityID) => _renderProvider.HasRenderable(entityID);

        public T GetComponent<T>(int entityID) where T : IComponent => GetComponentProvider<T>().GetComponent(entityID);
        public T GetComponentOrDefault<T>(int entityID) where T : IComponent
        {
            var provider = GetComponentProviderOrDefault<T>();
            return provider != null
                ? provider.GetComponentOrDefault(entityID)
                : default;
        }

        public bool HasComponent<T>(int entityID) where T : IComponent
        {
            var provider = GetComponentProviderOrDefault<T>();
            return provider != null && provider.HasComponent(entityID);
        }
    }
}
