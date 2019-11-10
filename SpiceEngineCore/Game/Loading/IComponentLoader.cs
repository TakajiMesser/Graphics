using SpiceEngineCore.Components;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Maps;
using System.Threading.Tasks;

namespace SpiceEngineCore.Game.Loading
{
    public interface IComponentLoader { }
    public interface IComponentLoader<T, U> : IComponentLoader where T : IComponent where U : IComponentBuilder<T>
    {
        bool IsLoaded { get; }

        void SetEntityProvider(IEntityProvider entityProvider);
        void AddBuilder(IMapEntity3D mapEntity);
        //void RemoveBuilders(int startIndex, int endIndex);

        Task LoadBuilderAsync(int entityID, U builder);

        //void AddComponent(int entityID, IComponentBuilder<T> builder);

        void InitializeLoad(int entityCount, int startIndex);
        void AddLoadTask(int entityID);

        void LoadSync();
        Task LoadAsync();
        //Task LoadAsync2();
    }
}
