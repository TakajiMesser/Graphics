using SpiceEngineCore.Components;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Maps;
using System.Threading.Tasks;

namespace SpiceEngineCore.Game.Loading
{
    public interface IComponentLoader { }
    public interface IComponentLoader<TComponent, TBuilder> : IComponentLoader where TComponent : IComponent where TBuilder : IComponentBuilder<TComponent>
    {
        bool IsLoaded { get; }

        void SetEntityProvider(IEntityProvider entityProvider);
        void AddBuilder(IMapEntity builder);

        Task LoadBuilderAsync(int entityID, TBuilder builder);
        void LoadBuilderSync(int entityID, TBuilder builder);

        void InitializeLoad(int entityCount, int startIndex);
        void AddLoadTask(int entityID);

        Task LoadAsync();
        void LoadSync();

        Task InitializeComponents();
    }
}
