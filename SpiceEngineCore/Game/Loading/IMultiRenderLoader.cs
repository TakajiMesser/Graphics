using SpiceEngineCore.Maps;
using SpiceEngineCore.Rendering;
using System.Threading.Tasks;

namespace SpiceEngineCore.Game.Loading
{
    public interface IMultiRenderLoader : IComponentLoader
    {
        int LoaderWaitCount { get; set; }

        void AddLoader(IRenderableLoader loader);
        void AddBuilder(IMapEntity builder);

        void InitializeLoad(int entityCount, int startIndex);
        void AddLoadTask(int entityID);

        Task LoadAsync();
        void LoadSync();
    }
}
