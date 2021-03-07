using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering.Vertices;

namespace SweetGraphicsCore.Vertices
{
    public interface IVertex2D : IVertex
    {
        Vector2 Position { get; }
    }
}
