using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SweetGraphicsCore.Rendering;
using System;

namespace SweetGraphicsCore.Buffers
{
    public class RenderBuffer : IDisposable, IBindable
    {
        public int Handle { get; }

        public RenderbufferTarget Target { get; private set; }
        public RenderbufferStorage Storage { get; set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public RenderBuffer(RenderbufferTarget target, int width, int height)
        {
            Handle = GL.GenRenderbuffer();

            Target = target;
            Width = width;
            Height = height;
        }

        public void Bind() => GL.BindRenderbuffer(Target, Handle);

        public void Unbind() => GL.BindRenderbuffer(Target, 0);

        public void ReserveMemory() => GL.RenderbufferStorage(Target, Storage, Width, Height);

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

                GL.DeleteRenderbuffer(Handle);
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
