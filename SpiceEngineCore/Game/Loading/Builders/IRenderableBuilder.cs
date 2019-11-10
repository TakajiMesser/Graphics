using OpenTK;
using SpiceEngineCore.Rendering;

namespace SpiceEngineCore.Game.Loading.Builders
{
    public interface IRenderableBuilder : IComponentBuilder<IRenderable>
    {
        Vector3 Position { get; set; }
    }
}
