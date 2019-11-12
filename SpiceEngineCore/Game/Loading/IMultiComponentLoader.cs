using SpiceEngineCore.Components;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Maps;
using System.Threading.Tasks;

namespace SpiceEngineCore.Game.Loading
{
    public interface IMultiComponentLoader : IComponentLoader { }
    public interface IMultiComponentLoader<T, U> : IMultiComponentLoader where T : IComponent where U : IComponentBuilder<T>
    {
        int LoaderWaitCount { get; set; }

        void AddLoader(IComponentLoader<T, U> loader);
        void AddBuilder(IMapEntity3D mapEntity);

        void InitializeLoad(int entityCount, int startIndex);
        void AddLoadTask(int entityID);

        Task LoadAsync();
        void LoadSync();
    }
}
