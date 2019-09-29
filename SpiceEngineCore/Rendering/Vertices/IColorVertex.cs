using OpenTK.Graphics;

namespace SpiceEngineCore.Rendering.Vertices
{
    public interface IColorVertex : IVertex
    {
        Color4 Color { get; }

        IColorVertex Colored(Color4 color);
    }
}
