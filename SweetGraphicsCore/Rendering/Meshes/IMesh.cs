using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Vertices;
using SweetGraphicsCore.Vertices;
using System;
using System.Collections.Generic;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SweetGraphicsCore.Rendering.Meshes
{
    public interface IMesh : IRenderable
    {
        IEnumerable<IVertex3D> Vertices { get; }
        IEnumerable<int> TriangleIndices { get; }
        float Alpha { get; set; }

        void Combine(IMesh mesh);

        void Transform(Transform transform);
        void Transform(Transform transform, int offset, int count);

        void TransformTexture(Vector3 center, Vector2 translation, float rotation, Vector2 scale);
        void TransformTexture(Vector3 center, Vector2 translation, float rotation, Vector2 scale, int offset, int count);

        void Update(Func<IVertex, IVertex> vertexUpdate);
        void Update(Func<IVertex, IVertex> vertexUpdate, int offset, int count);

        IMesh Duplicate();
    }
}
