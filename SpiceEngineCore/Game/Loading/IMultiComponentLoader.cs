using SpiceEngineCore.Components;
using SpiceEngineCore.Game.Loading.Builders;

namespace SpiceEngineCore.Game.Loading
{
    public interface IMultiComponentLoader : IComponentLoader { }
    public interface IMultiComponentLoader<T, U> : IMultiComponentLoader, IComponentLoader<T, U> where T : IComponent where U : IComponentBuilder<T>
    {
        int MultiLoaderWaitCount { get; set; }

        void AddLoader(IComponentLoader<T, U> loader);
    }
}
