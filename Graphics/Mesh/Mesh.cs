using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Mesh
{
    public class Mesh : IDisposable
    {
        private List<MeshVertex> _vertices = new List<MeshVertex>();
        private List<int> _triangleIndices = new List<int>();

        private VertexArray<MeshVertex> _vertexArray;
        private VertexBuffer<MeshVertex> _buffer;
        private List<VertexBuffer<MeshVertex>> _buffers = new List<VertexBuffer<MeshVertex>>();

        public Mesh(List<MeshVertex> vertices, List<int> triangleIndices, ShaderProgram program)
        {
            if (triangleIndices.Count % 3 != 0)
            {
                throw new ArgumentException(nameof(triangleIndices) + " must be divisible by three");
            }

            _vertices = vertices;
            _triangleIndices = triangleIndices;

            _buffer = new VertexBuffer<MeshVertex>();
            _buffer.AddVertices(_vertices);

            _vertexArray = new VertexArray<MeshVertex>(_buffer, program);
        }

        public void Draw()
        {
            _buffer.Bind();
            _vertexArray.Bind();

            _buffer.Buffer();
            //_buffer.Draw();
            //GL.BindVertexArray(_vertexArray.Handle);

            _buffer.Draw();
            //GL.DrawElements(BeginMode.Triangles, _triangleIndices.Count, DrawElementsType.UnsignedInt, 0);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
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

                //GL.DeleteShader(Handle);
                disposedValue = true;
            }
        }

        ~Mesh()
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
