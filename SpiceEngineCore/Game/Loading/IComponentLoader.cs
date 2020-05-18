using SpiceEngineCore.Components;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Maps;
using System.Threading.Tasks;

namespace SpiceEngineCore.Game.Loading
{
    public interface IComponentLoader { }
    public interface IComponentLoader<T, U> : IComponentLoader where T : IComponent where U : IComponentBuilder<T>
    {
        bool IsLoaded { get; }

        void SetEntityProvider(IEntityProvider entityProvider);
        void AddBuilder(IMapEntity builder);
        //void RemoveBuilders(int startIndex, int endIndex);

        Task LoadBuilderAsync(int entityID, U builder);
        void LoadBuilderSync(int entityID, U builder);

        //void AddComponent(int entityID, IComponentBuilder<T> builder);

        void InitializeLoad(int entityCount, int startIndex);
        void AddLoadTask(int entityID);

        Task LoadAsync();
        void LoadSync();
        
        //Task LoadAsync2();

        Task InitializeComponents();
    }
}
