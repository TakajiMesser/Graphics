using Graphics.Lighting;
using Graphics.Rendering.Shaders;
using Graphics.Rendering.Textures;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Rendering.Buffers
{
    public class FrameBuffer : IDisposable, IBindable
    {
        private readonly int _handle;
        private readonly string _name;

        private Dictionary<FramebufferAttachment, Texture> _attachments = new Dictionary<FramebufferAttachment, Texture>();

        public FrameBuffer(string name)
        {
            _handle = GL.GenBuffer();
            _name = name;
        }

        public void Buffer()
        {
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, 0, _handle);
        }

        public void Bind()
        {
            //GL.BindBuffer(BufferTarget.UniformBuffer, _handle);
            //GL.BufferData(BufferTarget.UniformBuffer, _size * _lights.Count, _lights.ToArray(), BufferUsageHint.DynamicDraw);
        }

        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
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

        ~FrameBuffer()
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
