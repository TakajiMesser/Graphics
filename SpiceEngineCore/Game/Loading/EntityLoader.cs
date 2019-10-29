using SpiceEngineCore.Game.Loading.Builders;
using System.Threading.Tasks;

namespace SpiceEngineCore.Game.Loading
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
