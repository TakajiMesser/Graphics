using SpiceEngineCore.Rendering;

namespace SweetGraphicsCore.Renderers
{
    public abstract class Renderer : IRenderer
    {
        public void Load(Resolution resolution)
        {
            resolution.ResolutionChanged += (s, args) => Resize(args.Resolution);

            LoadPrograms();
            LoadTextures(resolution);
            LoadBuffers();
        }

        protected abstract void Resize(Resolution resolution);

        protected abstract void LoadPrograms();
        protected abstract void LoadTextures(Resolution resolution);
        protected abstract void LoadBuffers();
    }
}
