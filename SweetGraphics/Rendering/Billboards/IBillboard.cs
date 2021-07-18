using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Textures;
using SweetGraphicsCore.Vertices;
using System.Collections.Generic;

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
