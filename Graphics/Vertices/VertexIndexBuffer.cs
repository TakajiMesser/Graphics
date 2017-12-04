using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Graphics.TwoDimensional;
using Graphics.Helpers;
using System.Runtime.InteropServices;

namespace Graphics
{
    public class VertexIndexBuffer : IDisposable
    {
        internal readonly int _handle;
        private List<ushort> _indices = new List<ushort>();

        public VertexIndexBuffer()
        {
            GL.GenBuffers(1, out _handle);
            //_handle = GL.GenBuffer();
        }

        public void AddIndex(ushort index)
        {
            _indices.Add(index);
        }

        public void AddIndices(IEnumerable<ushort> indices)
        {
            _indices.AddRange(indices);
        }

        public void Clear()
        {
            _indices.Clear();
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _handle);
        }

        public void Buffer()
        {
            GL.BufferData(BufferTarget.ElementArrayBuffer, Marshal.SizeOf<ushort>() * _indices.Count, _indices.ToArray(), BufferUsageHint.StreamDraw);
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
