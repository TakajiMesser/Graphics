using SpiceEngineCore.Rendering;

namespace SweetGraphicsCore.Renderers
{
    public abstract class Renderer : IRenderer
    {
        public void Load(IRenderContextProvider contextProvider, Resolution resolution)
        {
            resolution.ResolutionChanged += (s, args) => Resize(args.Resolution);

            LoadPrograms(contextProvider);
            LoadTextures(contextProvider, resolution);
            LoadBuffers(contextProvider);
        }

        protected abstract void Resize(Resolution resolution);

        protected abstract void LoadPrograms(IRenderContextProvider contextProvider);
        protected abstract void LoadTextures(IRenderContextProvider contextProvider, Resolution resolution);
        protected abstract void LoadBuffers(IRenderContextProvider contextProvider);
    }
}
