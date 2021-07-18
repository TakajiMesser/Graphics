using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering.Vertices;

namespace SweetGraphicsCore.Vertices
{
    public interface IColorVertex : IVertex
    {
        Color4 Color { get; }

        IColorVertex Colored(Color4 color);
    }
}
