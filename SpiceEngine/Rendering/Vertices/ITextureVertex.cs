using OpenTK;

namespace SpiceEngine.Rendering.Vertices
{
    public interface ITextureVertex : IVertex
    {
        Vector3 Normal { get; }
        Vector3 Tangent { get; }
        Vector2 TextureCoords { get; }

        ITextureVertex TextureTransformed(Vector3 center, Vector2 translation, float rotation, Vector2 scale);
    }
}
