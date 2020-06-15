using SpiceEngineCore.Components;

namespace SpiceEngineCore.Game
{
    public interface IComponentProvider { }
    public interface IComponentProvider<TComponent> : IComponentProvider where TComponent : IComponent
    {
        TComponent GetComponent(int entityID);
        TComponent GetComponentOrDefault(int entityID);

        bool HasComponent(int entityID);
    }
}
