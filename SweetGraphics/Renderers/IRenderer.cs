using SpiceEngineCore.Rendering;

namespace SweetGraphicsCore.Renderers
{
    public interface IRenderer
    {
        void Load(IRenderContext renderContext, Resolution resolution);
    }
}
