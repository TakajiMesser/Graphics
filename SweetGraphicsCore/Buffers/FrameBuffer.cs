using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Textures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SweetGraphicsCore.Buffers
{
    public class FrameBuffer : OpenGLObject
    {
        private ITexture _depthStencilTexture = null;
        private Dictionary<FramebufferAttachment, ITexture> _textures = new Dictionary<FramebufferAttachment, ITexture>();
        private Dictionary<FramebufferAttachment, RenderBuffer> _renderBuffers = new Dictionary<FramebufferAttachment, RenderBuffer>();

        public FrameBuffer(IRenderContext renderContext) : base(renderContext) { }

        public override void Load()
        {
            base.Load();

            Bind();
            Attach();
            Unbind();
        }

        protected override int Create() => GL.GenFramebuffer();
        protected override void Delete() => GL.DeleteFramebuffer(Handle);

        public override void Bind() => GL.BindFramebuffer(FramebufferTarget.Framebuffer, Handle);
        public override void Unbind() => GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        public void BindForDraw() => GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, Handle);
        public void UnbindForDraw() => GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

        public void BindForRead() => GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, Handle);
        public void UnbindForRead() => GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, 0);

        public void AddDepthStencilTexture(ITexture texture) => _depthStencilTexture = texture;
        public void Add(FramebufferAttachment attachment, ITexture texture) => _textures.Add(attachment, texture);
        public void Add(FramebufferAttachment attachment, RenderBuffer renderBuffer) => _renderBuffers.Add(attachment, renderBuffer);

        public void Clear()
        {
            _depthStencilTexture = null;
            _textures.Clear();
            _renderBuffers.Clear();
        }

        private void Attach()
        {
            if (_depthStencilTexture != null)
            {
                GL.FramebufferTexture(FramebufferTarget.Framebuffer, InvalidateFramebufferAttachment.DepthStencilAttachment, _depthStencilTexture.Handle, 0);
            }

            foreach (var texture in _textures)
            {
                GL.FramebufferTexture(FramebufferTarget.Framebuffer, texture.Key, texture.Value.Handle, 0);
            }

            foreach (var renderBuffer in _renderBuffers)
            {
                GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, renderBuffer.Key, RenderbufferTarget.Renderbuffer, renderBuffer.Value.Handle);
            }

            // Check if the framebuffer is "complete"
            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferStatus.FramebufferComplete)
            {
                throw new Exception("Error in FrameBuffer: " + GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer));
            }
        }

        public void BindAndDraw()
        {
            BindForDraw();

            GL.DrawBuffers(_textures.Count, _textures.Keys
                .Where(k => k != FramebufferAttachment.DepthAttachment)
                .Select(k => (DrawBufferMode)k)
                .ToArray());

            foreach (var attachment in _renderBuffers)
            {
                GL.DrawBuffer((DrawBufferMode)attachment.Key);
            }
        }

        public void BindAndDraw(params DrawBufferMode[] colorBuffers)
        {
            BindForDraw();
            GL.DrawBuffers(colorBuffers.Length, colorBuffers);
        }

        public void BindAndRead(ReadBufferMode readBuffer)
        {
            BindForRead();
            GL.ReadBuffer(readBuffer);
        }
    }
}
