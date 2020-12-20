using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Textures;
using SweetGraphicsCore.Vertices;
using System.Collections.Generic;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SweetGraphicsCore.Rendering.Billboards
{
    public interface IBillboard : IRenderable
    {
        IEnumerable<IVertex3D> Vertices { get; }
        int TextureIndex { get; }

        void LoadTexture(ITextureProvider textureProvider);
        void Combine(IBillboard billboard);

        void AddVertex(Vector3 position, Color4 color);

        void Transform(Transform transform);
        void Transform(Transform transform, int offset, int count);

        void SetColor(Color4 color);
        void SetColor(Color4 color, int offset, int count);

        IBillboard Duplicate();
    }
}
