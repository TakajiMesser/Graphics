using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES30;
using Graphics.Helpers;
using Graphics.Rendering.Buffers;
using Graphics.Rendering.Shaders;

namespace Graphics.Rendering.Vertices
{
    public class VertexArray<T> : IDisposable, IBindable where T : struct
    {
        private readonly int _handle;
        private readonly VertexBuffer<T> _buffer;
        private bool _generated = false;

        public int Handle => _handle;

        public VertexArray(VertexBuffer<T> buffer, ShaderProgram program)
        {
            _buffer = buffer;

            if (_generated)
            {
                GL.DeleteVertexArrays(1, ref _handle);
            }

            _handle = GL.GenVertexArray();
            _generated = true;

            Bind();
            buffer.Bind();

            foreach (var attribute in VertexHelper.GetAttributes<T>())
            {
                attribute.Set(program);
            }

            Unbind();
            buffer.Unbind();
        }

        public void Bind()
        {
            GL.BindVertexArray(_handle);
        }

        public void Unbind()
        {
            GL.BindVertexArray(0);
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

                GL.DeleteVertexArray(_handle);
                disposedValue = true;
            }
        }

        ~VertexArray()
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
