using OpenTK;
using SpiceEngineCore.Rendering;

namespace SpiceEngineCore.Game.Loading
{
    public interface IRenderableBuilder : IBuilder
    {
        Vector3 Position { get; set; }

        IRenderable ToRenderable();
    }
}
