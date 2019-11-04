using SpiceEngineCore.Game.Loading.Builders;
using System.Threading.Tasks;

namespace SpiceEngineCore.Game.Loading
{
    public interface IComponentLoader { }
    public interface IComponentLoader<T> : IComponentLoader where T : IComponentBuilder
    {
        void AddComponent(int entityID, T builder);
        Task Load();
    }
}
