using SpiceEngineCore.Components;

namespace SpiceEngineCore.Game.Loading.Builders
{
    public interface IComponentBuilder
    {

    }

    public interface IComponentBuilder<T> : IComponentBuilder where T : IComponent
    {
        
    }
}
