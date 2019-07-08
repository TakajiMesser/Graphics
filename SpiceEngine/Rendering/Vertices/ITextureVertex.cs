using OpenTK;

namespace SpiceEngine.Rendering.Vertices
{
    public interface ITextureVertex : IVertex
    {
        Vector3 Normal { get; }
        Vector3 Tangent { get; }
        Vector2 TextureCoords { get; }
    }
}
