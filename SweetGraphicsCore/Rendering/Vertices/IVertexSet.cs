using SpiceEngineCore.Geometry.Vectors;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Vertices;
using System;
using System.Collections.Generic;

namespace SweetGraphicsCore.Vertices
{
    public interface IVertexSet : IRenderable
    {
        IEnumerable<IVertex3D> Vertices { get; }
        float Alpha { get; set; }

        void Combine(IVertexSet vertexSet);

        void Transform(Transform transform);
        void Transform(Transform transform, int offset, int count);

        void TransformTexture(Vector3 center, Vector2 translation, float rotation, Vector2 scale);
        void TransformTexture(Vector3 center, Vector2 translation, float rotation, Vector2 scale, int offset, int count);

        void Update(Func<IVertex, IVertex> vertexUpdate);
        void Update(Func<IVertex, IVertex> vertexUpdate, int offset, int count);

        IVertexSet Duplicate();
    }
}
