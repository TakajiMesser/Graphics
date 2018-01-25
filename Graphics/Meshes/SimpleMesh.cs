using Graphics.Lighting;
using Graphics.Materials;
using Graphics.Rendering.Buffers;
using Graphics.Rendering.Shaders;
using Graphics.Rendering.Vertices;
using Graphics.Utilities;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Meshes
{
    public class SimpleMesh : IDisposable
    {
        private int _vertexArrayHandle;
        private VertexBuffer<Vector3> _vertexBuffer = new VertexBuffer<Vector3>();
        private VertexIndexBuffer _indexBuffer = new VertexIndexBuffer();

        public SimpleMesh(List<Vector3> vertices, List<int> triangleIndices, ShaderProgram program)
        {
            if (triangleIndices.Count % 3 != 0)
            {
                throw new ArgumentException(nameof(triangleIndices) + " must be divisible by three");
            }

            _vertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayHandle);

            _vertexBuffer.AddVertices(vertices);
            _vertexBuffer.Bind();
            //_vertexBuffer.Buffer();

            var attribute = new VertexAttribute("vPosition", 3, VertexAttribPointerType.Float, UnitConversions.SizeOf<Vector3>(), 0);
            attribute.Set(program.GetAttributeLocation("vPosition"));

            GL.BindVertexArray(0);
            _vertexBuffer.Unbind();

            _indexBuffer.AddIndices(triangleIndices.ConvertAll(i => (ushort)i));
        }
        
        public void Draw()
        {
            GL.BindVertexArray(_vertexArrayHandle);
            _vertexBuffer.Bind();
            _indexBuffer.Bind();

            _vertexBuffer.Buffer();
            _indexBuffer.Buffer();

            _indexBuffer.Draw();

            GL.BindVertexArray(0);
            _vertexBuffer.Unbind();
            _indexBuffer.Unbind();
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

        ~SimpleMesh()
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
