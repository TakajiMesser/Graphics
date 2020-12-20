using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Vertices;
using System;
using System.Collections.Generic;

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
