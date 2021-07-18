using SpiceEngineCore.Rendering;

namespace SweetGraphicsCore.Renderers
{
    public abstract class Renderer : IRenderer
    {
        public void Load(IRenderContext renderContext, Resolution resolution)
        {
            resolution.ResolutionChanged += (s, args) => Resize(args.Resolution);

            LoadPrograms(renderContext);
            LoadTextures(renderContext, resolution);
            LoadBuffers(renderContext);
        }

        protected abstract void Resize(Resolution resolution);

        protected abstract void LoadPrograms(IRenderContext renderContext);
        protected abstract void LoadTextures(IRenderContext renderContext, Resolution resolution);
        protected abstract void LoadBuffers(IRenderContext renderContext);
    }
}
