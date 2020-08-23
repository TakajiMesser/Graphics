using SpiceEngineCore.Rendering;

namespace SweetGraphicsCore.Renderers
{
    public abstract class Renderer : IRenderer
    {
        public void Load(Resolution resolution)
        {
            LoadPrograms();
            LoadTextures(resolution);
            LoadBuffers();
        }

        public abstract void Resize(Resolution resolution);

        protected abstract void LoadPrograms();
        protected abstract void LoadTextures(Resolution resolution);
        protected abstract void LoadBuffers();
    }
}
