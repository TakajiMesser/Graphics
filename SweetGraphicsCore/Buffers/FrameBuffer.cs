using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SpiceEngineCore.Rendering.Textures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SweetGraphicsCore.Buffers
{
    public class FrameBuffer : IDisposable
    {
        private readonly int _handle;

        private Dictionary<FramebufferAttachment, ITexture> _textures = new Dictionary<FramebufferAttachment, ITexture>();
        private Dictionary<FramebufferAttachment, RenderBuffer> _renderBuffers = new Dictionary<FramebufferAttachment, RenderBuffer>();

        public FrameBuffer() => _handle = GL.GenFramebuffer();

        public void Add(FramebufferAttachment attachment, ITexture texture) => _textures.Add(attachment, texture);
        public void Add(FramebufferAttachment attachment, RenderBuffer renderBuffer) => _renderBuffers.Add(attachment, renderBuffer);

        public void Clear()
        {
            _textures.Clear();
            _renderBuffers.Clear();
        }

        public void AttachAttachments()
        {
            foreach (var texture in _textures)
            {
                GL.FramebufferTexture(FramebufferTarget.Framebuffer, texture.Key, texture.Value.Handle, 0);
            }

            foreach (var renderBuffer in _renderBuffers)
            {
                GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, renderBuffer.Key, RenderbufferTarget.Renderbuffer, renderBuffer.Value.Handle);
            }

            // Check if the framebuffer is "complete"
            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            {
                throw new Exception("Error in FrameBuffer: " + GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer));
            }
        }

        public void Bind(FramebufferTarget target) => GL.BindFramebuffer(target, _handle);
        public void Unbind(FramebufferTarget target) => GL.BindFramebuffer(target, 0);

        public void BindAndDraw()
        {
            Bind(FramebufferTarget.DrawFramebuffer);

            GL.DrawBuffers(_textures.Count, _textures.Keys
                .Where(k => k != FramebufferAttachment.DepthStencilAttachment && k != FramebufferAttachment.DepthAttachment)
                .Select(k => (DrawBuffersEnum)k)
                .ToArray());

            foreach (var attachment in _renderBuffers)
            {
                GL.DrawBuffer((DrawBufferMode)attachment.Key);
            }
        }

        public void BindAndDraw(params DrawBuffersEnum[] colorBuffers)
        {
            Bind(FramebufferTarget.DrawFramebuffer);
            GL.DrawBuffers(colorBuffers.Length, colorBuffers);
        }

        public void BindAndRead(ReadBufferMode readBuffer)
        {
            Bind(FramebufferTarget.ReadFramebuffer);
            GL.ReadBuffer(readBuffer);
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

                GL.DeleteFramebuffer(_handle);
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
