using OpenTK;
using SpiceEngineCore.Rendering;

namespace SpiceEngineCore.Game.Loading.Builders
{
    public interface IRenderableBuilder
    {
        Vector3 Position { get; set; }

        IRenderable ToRenderable();
    }
}
