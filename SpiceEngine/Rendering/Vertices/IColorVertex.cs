using OpenTK.Graphics;

namespace SpiceEngine.Rendering.Vertices
{
    public interface IColorVertex : IVertex
    {
        Color4 Color { get; }

        IColorVertex Colored(Color4 color);
    }
}
