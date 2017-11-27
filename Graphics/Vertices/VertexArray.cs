using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES30;
//using OpenTK.Graphics.ES20;
using Graphics.TwoDimensional;
using Graphics.Helpers;

namespace Graphics
{
    public class VertexArray<T> : IDisposable where T : struct
    {
        private readonly int _handle;
        private readonly VertexBuffer<T> _buffer;
        private bool _generated = false;

        public int Handle => _handle;

        public VertexArray(VertexBuffer<T> buffer, ShaderProgram program)
        {
            if (_generated)
            {
                GL.DeleteVertexArrays(1, ref _handle);
            }

            GL.GenVertexArrays(1, out _handle);
            _generated = true;
            _buffer = buffer;

            Bind();
            buffer.Bind();

            foreach (var attribute in VertexHelper.GetAttributes<T>())
            {
                attribute.Set(program);
            }

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void Bind()
        {
            GL.BindVertexArray(_handle);
        }

        public void UnBind()
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

                GL.DeleteBuffer(_handle);
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
