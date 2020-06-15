using SpiceEngineCore.Components;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Rendering;

namespace SpiceEngineCore.Game
{
    public interface ISystemProvider
    {
        IEntityProvider GetEntityProvider();
        IRenderProvider GetRenderProvider();

        IComponentProvider<T> GetComponentProvider<T>() where T : IComponent;
        IComponentProvider<T> GetComponentProviderOrDefault<T>() where T : IComponent;

        T GetGameSystem<T>() where T : IGameSystem;
        T GetGameSystemOrDefault<T>() where T : IGameSystem;

        bool HasComponentProvider<T>() where T : IComponent;
        bool HasGameSystem<T>() where T : IGameSystem;

        IEntity GetEntity(int entityID);
        IEntity GetEntityOrDefault(int entityID);

        IRenderable GetRenderable(int entityID);
        IRenderable GetRenderableOrDefault(int entityID);
        bool HasRenderable(int entityID);

        T GetComponent<T>(int entityID) where T : IComponent;
        T GetComponentOrDefault<T>(int entityID) where T : IComponent;

        bool HasComponent<T>(int entityID) where T : IComponent;
    }
}
