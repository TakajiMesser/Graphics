﻿using Graphics.Lighting;
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

        private Dictionary<FramebufferAttachment, Texture> _textures = new Dictionary<FramebufferAttachment, Texture>();
        private Dictionary<FramebufferAttachment, RenderBuffer> _renderBuffers = new Dictionary<FramebufferAttachment, RenderBuffer>();

        public FrameBuffer()
        {
            _handle = GL.GenFramebuffer();
        }

        public void Add(FramebufferAttachment attachment, Texture texture) => _textures.Add(attachment, texture);
        public void Add(FramebufferAttachment attachment, RenderBuffer renderBuffer) => _renderBuffers.Add(attachment, renderBuffer);

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
                throw new GraphicsErrorException("Error in FrameBuffer: " + GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer));
            }
        }

        public void Bind() => GL.BindFramebuffer(FramebufferTarget.Framebuffer, _handle);

        public void Draw()
        {
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, _handle);

            foreach (var attachment in _textures)
            {
                GL.DrawBuffer((DrawBufferMode)attachment.Key);
            }

            foreach (var attachment in _renderBuffers)
            {
                GL.DrawBuffer((DrawBufferMode)attachment.Key);
            }
        }

        public void Unbind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
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