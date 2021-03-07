using SpiceEngineCore.Rendering;

namespace SweetGraphicsCore.Renderers
{
    public interface IRenderer
    {
        void Load(IRenderContextProvider contextProvider, Resolution resolution);
    }
}
