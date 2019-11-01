using SpiceEngineCore.Game.Loading.Builders;
using System.Threading.Tasks;

namespace SpiceEngineCore.Game.Loading
{
    public abstract class ComponentLoader<T> : UpdateManager, IComponentLoader<T> where T : IComponentBuilder
    {
        public bool IsLoaded { get; private set; }

        // TODO - Add builders to a load queue
        public abstract void AddComponent(int id, T builder);

        public async Task Load()
        {
            LoadComponents();

            if (!IsLoaded)
            {
                await LoadInternal();
                IsLoaded = true;
            }
        }

        protected abstract Task LoadInternal();
        protected abstract void LoadComponents();
    }
}
