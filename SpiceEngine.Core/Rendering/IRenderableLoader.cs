using SpiceEngineCore.Entities;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Maps;
using System.Threading.Tasks;

namespace SpiceEngineCore.Rendering
{
    public interface IRenderableLoader
    {
        bool IsLoaded { get; }

        void SetEntityProvider(IEntityProvider entityProvider);
        void AddBuilder(IMapEntity builder);

        Task LoadBuilderAsync(int entityID, IRenderableBuilder builder);
        void LoadBuilderSync(int entityID, IRenderableBuilder builder);

        void InitializeLoad(int entityCount, int startIndex);
        void AddLoadTask(int entityID);

        Task LoadAsync();
        void LoadSync();

        Task InitializeComponents();
    }
}
