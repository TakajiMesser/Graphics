﻿using OpenTK.Graphics;
using OpenTK.Graphics.ES20;
using SpiceEngineCore.Rendering.Vertices;
using SpiceEngineCore.Utilities;
using SweetGraphicsCore.Rendering;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SweetGraphicsCore.Buffers
{
    public class VertexBuffer<T> : IDisposable, IBindable where T : IVertex
    {
        public int Count => _vertices.Count;

        private readonly int _handle;
        private readonly int _vertexSize;
        private List<T> _vertices = new List<T>();

        public VertexBuffer()
        {
            _handle = GL.GenBuffer();
            _vertexSize = UnitConversions.SizeOf(typeof(T));
        }

        public void AddVertex(T vertex) => _vertices.Add(vertex);
        public void AddVertices(params T[] vertices) => _vertices.AddRange(vertices);
        public void AddVertices(IEnumerable<T> vertices) => _vertices.AddRange(vertices);

        public void InsertVertex(int index, T vertex) => _vertices.Insert(index, vertex);

        public void SetVertex(int index, T vertex) => _vertices[index] = vertex;

        public T GetVertex(int index) => _vertices[index];

        public void Clear() => _vertices.Clear();

        public void Buffer()
        {
            //GL.BufferData(BufferTarget.ArrayBuffer, _vertexSize * _vertices.Count, _vertices.ToArray(), BufferUsageHint.StreamDraw);
            var handle = GCHandle.Alloc(_vertices.ToArray(), GCHandleType.Pinned);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertexSize * _vertices.Count, handle.AddrOfPinnedObject(), BufferUsageHint.StreamDraw);
            handle.Free();
        }

        public void Bind() => GL.BindBuffer(BufferTarget.ArrayBuffer, _handle);

        public void Unbind() => GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        public void DrawTriangles() => GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Count);

        public void DrawQuads() => GL.DrawArrays(PrimitiveType.Quads, 0, _vertices.Count);

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue && GraphicsContext.CurrentContext != null && !GraphicsContext.CurrentContext.IsDisposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                GL.DeleteBuffer(_handle);
                disposedValue = true;
            }
        }

        ~VertexBuffer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
