using System.Threading.Tasks;

namespace SpiceEngine.Entities.Builders
{
    public interface IEntityLoader<T> where T : IBuilder
    {
        void AddEntity(int id, T builder);
        Task Load();
    }
}
