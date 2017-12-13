using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES20;
using Graphics.TwoDimensional;
using Graphics.Helpers;
using Graphics.Utilities;

namespace Graphics.Vertices
{
    public class VertexBuffer<T> : IDisposable where T : struct
    {
        private readonly int _handle;
        private readonly int _vertexSize;
        private List<T> _vertices = new List<T>();

        public VertexBuffer()
        {
            _handle = GL.GenBuffer();
            _vertexSize = UnitConversions.SizeOf<T>();
        }

        public void AddVertex(T vertex)
        {
            _vertices.Add(vertex);
        }

        public void AddVertices(IEnumerable<T> vertices)
        {
            _vertices.AddRange(vertices);
        }

        public void Clear()
        {
            _vertices.Clear();
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _handle);
        }

        public void Buffer()
        {
            GL.BufferData(BufferTarget.ArrayBuffer, _vertexSize * _vertices.Count, _vertices.ToArray(), BufferUsageHint.StreamDraw);
        }

        public void Draw()
        {
            //GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Count);
            //GL.DrawElements(PrimitiveType.Triangles, 0, DrawElementsType.UnsignedShort, )

            /*glDrawElements(
                 GL_TRIANGLES,      // mode
                 indices.size(),    // count
                 GL_UNSIGNED_INT,   // type
                 (void*)0           // element array buffer offset
             );*/
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
