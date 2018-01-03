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
    public class RenderBuffer : IDisposable, IBindable
    {
        private readonly int _handle;
        public int Handle => _handle;

        public RenderbufferTarget Target { get; private set; }
        public RenderbufferStorage Storage { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public RenderBuffer(RenderbufferTarget target, int width, int height)
        {
            _handle = GL.GenRenderbuffer();

            Target = target;
            Width = width;
            Height = height;
        }

        public void Bind()
        {
            GL.BindRenderbuffer(Target, _handle);
        }

        public void Unbind()
        {
            GL.BindRenderbuffer(Target, 0);
        }

        public void ReserveMemory()
        {
            GL.RenderbufferStorage(Target, Storage, Width, Height);
        }

        public void Load(IntPtr pixels)
        {
            //Specify(pixels);
            //SetTextureParameters();
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

                GL.DeleteRenderbuffer(_handle);
                disposedValue = true;
            }
        }

        ~RenderBuffer()
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
