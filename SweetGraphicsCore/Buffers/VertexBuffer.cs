using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Vertices;
using SpiceEngineCore.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SweetGraphicsCore.Buffers
{
    public class VertexBuffer<T> : OpenGLObject where T : struct, IVertex
    {
        private readonly int _vertexSize;
        private List<T> _vertices = new List<T>();

        public VertexBuffer(IRenderContextProvider contextProvider) : base(contextProvider) => _vertexSize = UnitConversions.SizeOf(typeof(T));

        public int Count => _vertices.Count;

        protected override int Create() => GL.GenBuffer();
        protected override void Delete() => GL.DeleteBuffer(Handle);

        public override void Bind() => GL.BindBuffer(BufferTargetARB.ArrayBuffer, Handle);
        public override void Unbind() => GL.BindBuffer(BufferTargetARB.ArrayBuffer, 0);

        public void AddVertex(T vertex) => _vertices.Add(vertex);
        public void AddVertices(params T[] vertices) => _vertices.AddRange(vertices);
        public void AddVertices(IEnumerable<T> vertices) => _vertices.AddRange(vertices);

        public void InsertVertex(int index, T vertex) => _vertices.Insert(index, vertex);

        public void SetVertex(int index, T vertex) => _vertices[index] = vertex;

        public T GetVertex(int index) => _vertices[index];

        public void Clear() => _vertices.Clear();

        public void Buffer() => GL.BufferData(BufferTargetARB.ArrayBuffer, _vertexSize * _vertices.Count, _vertices.ToArray(), BufferUsageARB.StreamDraw);

        public void DrawPoints() => GL.DrawArrays(PrimitiveType.Points, 0, _vertices.Count);

        public void DrawTriangles() => GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Count);

        public void DrawTriangleStrips() => GL.DrawArrays(PrimitiveType.TriangleStrip, 0, _vertices.Count);

        public void DrawQuads() => GL.DrawArrays(PrimitiveType.Quads, 0, _vertices.Count);
    }
}
