using SpiceEngine.Game;
using System.Threading.Tasks;

namespace SpiceEngine.Entities.Builders
{
    public abstract class EntityLoader<T> : UpdateManager, IEntityLoader<T> where T : IBuilder
    {
        public bool IsLoaded { get; private set; }

        public abstract void AddEntity(int id, T builder);

        public async Task Load()
        {
            LoadEntities();

            if (!IsLoaded)
            {
                await LoadInternal();
                IsLoaded = true;
            }
        }

        protected abstract Task LoadInternal();
        protected abstract void LoadEntities();
    }
}
