using OpenTK;
using OpenTK.Graphics;

namespace TakoEngine.Rendering.Vertices
{
    public interface IVertex
    {
        Vector3 Position { get; }
        Vector3 Normal { get; }
        Vector3 Tangent { get; }
        Color4 Color { get; }
        Vector2 TextureCoords { get; }
        int MaterialIndex { get; }
    }
}
