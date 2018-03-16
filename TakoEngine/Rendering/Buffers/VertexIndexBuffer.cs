using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace TakoEngine.Rendering.Buffers
{
    public class VertexIndexBuffer : IDisposable, IBindable
    {
        private int _handle;
        private List<ushort> _indices = new List<ushort>();

        public VertexIndexBuffer()
        {
            _handle = GL.GenBuffer();
        }

        public void AddIndex(ushort index) => _indices.Add(index);

        public void AddIndices(IEnumerable<ushort> indices) => _indices.AddRange(indices);

        public void Clear() => _indices.Clear();

        public void Buffer()
        {
            GL.BufferData(BufferTarget.ElementArrayBuffer, Marshal.SizeOf<ushort>() * _indices.Count, _indices.ToArray(), BufferUsageHint.StreamDraw);
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _handle);
        }

        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _handle);
        }

        public void Draw()
        {
            GL.DrawElements(PrimitiveType.Triangles, _indices.Count, DrawElementsType.UnsignedShort, IntPtr.Zero);
        }

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

        ~VertexIndexBuffer()
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
