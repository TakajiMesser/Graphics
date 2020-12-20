using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

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
