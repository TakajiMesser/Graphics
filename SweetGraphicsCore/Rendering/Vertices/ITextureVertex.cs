using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SweetGraphicsCore.Vertices
{
    public interface ITextureVertex : IVertex3D
    {
        Vector3 Normal { get; }
        Vector3 Tangent { get; }
        Vector2 TextureCoords { get; }

        ITextureVertex TextureTransformed(Vector3 center, Vector2 translation, float rotation, Vector2 scale);
    }
}
