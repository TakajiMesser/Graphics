using SpiceEngineCore.Maps;
using System.Threading.Tasks;

namespace SpiceEngineCore.Game.Loading
{
    public interface IComponentLoader
    {
        bool IsLoaded { get; }

        void AddBuilder(IMapEntity builder);

        void InitializeLoad(int entityCount, int startIndex);
        void AddLoadTask(int entityID);

        Task LoadAsync();
        void LoadSync();

        Task InitializeComponents();
    }
}
