using OpenTK;
using OpenTK.Graphics;

namespace SpiceEngine.Rendering.Vertices
{
    public interface IVertex2D : IVertex
    {
        Vector2 Position { get; }
    }
}
