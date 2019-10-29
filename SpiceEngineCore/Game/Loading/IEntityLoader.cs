using SpiceEngineCore.Game.Loading.Builders;
using System.Threading.Tasks;

namespace SpiceEngineCore.Game.Loading
{
    public interface IEntityLoader<T> where T : IBuilder
    {
        void AddEntity(int id, T builder);
        Task Load();
    }
}
