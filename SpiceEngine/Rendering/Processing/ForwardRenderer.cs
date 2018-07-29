using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SpiceEngine.Properties;
using System.Collections.Generic;
using System.IO;
using SpiceEngine.Entities;
using SpiceEngine.Entities.Cameras;
using SpiceEngine.Helpers;
using SpiceEngine.Outputs;
using SpiceEngine.Rendering.Buffers;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;

namespace SpiceEngine.Rendering.Processing
{
    public class ForwardRenderer : Renderer
    {
        public Texture FinalTexture { get; protected set; }
        public Texture VelocityTexture { get; protected set; }
        public Texture DepthTexture { get; protected set; }

        internal ShaderProgram _program;
        internal FrameBuffer _frameBuffer = new FrameBuffer();

        protected override void LoadPrograms()
        {
            _program = new ShaderProgram(
                new Shader(ShaderType.VertexShader, Resources.forward_vert),
                new Shader(ShaderType.FragmentShader, Resources.forward_frag)
            );
        }

        public override void ResizeTextures(Resolution resolution)
        {
            FinalTexture.Resize(resolution.Width, resolution.Height, 0);
            FinalTexture.Bind();
            FinalTexture.ReserveMemory();

            VelocityTexture.Resize(resolution.Width, resolution.Height, 0);
            VelocityTexture.Bind();
            VelocityTexture.ReserveMemory();

            DepthTexture.Resize(resolution.Width, resolution.Height, 0);
            DepthTexture.Bind();
            DepthTexture.ReserveMemory();
        }

        protected override void LoadTextures(Resolution resolution)
        {
            FinalTexture = new Texture(resolution.Width, resolution.Height, 0)
            {
                Target = TextureTarget.Texture2D,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.Rgba16f,
                PixelFormat = PixelFormat.Rgba,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            FinalTexture.Bind();
            FinalTexture.ReserveMemory();

            VelocityTexture = new Texture(resolution.Width, resolution.Height, 0)
            {
                Target = TextureTarget.Texture2D,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.Rg16f,
                PixelFormat = PixelFormat.Rg,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            VelocityTexture.Bind();
            VelocityTexture.ReserveMemory();

            DepthTexture = new Texture(resolution.Width, resolution.Height, 0)
            {
                Target = TextureTarget.Texture2D,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.DepthComponent32f,
                PixelFormat = PixelFormat.DepthComponent,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            DepthTexture.Bind();
            DepthTexture.ReserveMemory();
        }

        protected override void LoadBuffers()
        {
            _frameBuffer.Clear();
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment0, FinalTexture);
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment1, VelocityTexture);
            //_frameBuffer.Add(FramebufferAttachment.DepthStencilAttachment, new RenderBuffer(RenderbufferTarget.Renderbuffer, Resolution.Width, Resolution.Height));
            _frameBuffer.Add(FramebufferAttachment.DepthAttachment, DepthTexture);

            _frameBuffer.Bind(FramebufferTarget.Framebuffer);
            _frameBuffer.AttachAttachments();
            _frameBuffer.Unbind(FramebufferTarget.Framebuffer);
        }

        public void Render(Resolution resolution, TextureManager textureManager, Camera camera, IEnumerable<Brush> brushes, IEnumerable<Actor> actors)
        {
            _program.Use();
            _frameBuffer.BindAndDraw();

            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, resolution.Width, resolution.Height);

            camera.SetUniforms(_program);

            foreach (var brush in brushes)
            {
                brush.SetUniforms(_program, textureManager);
                brush.Draw();
            }

            foreach (var actor in actors)
            {
                //actor.SetUniforms(_program, textureManager);
                //actor.Draw();
                actor.SetUniformsAndDraw(_program, textureManager);
            }
        }

        private void BindTextures(TextureManager textureManager, TextureMapping textureMapping)
        {
            // TODO - Order brush rendering in a way that allows us to not re-bind duplicate textures repeatedly
            // Check brush's texture mapping to see which textures we need to bind
            var diffuseMap = textureManager.RetrieveTexture(textureMapping.DiffuseMapID);
            GL.Uniform1(_program.GetUniformLocation("useDiffuseMap"), (diffuseMap != null) ? 1 : 0);
            if (diffuseMap != null)
            {
                _program.BindTexture(diffuseMap, "diffuseMap", 0);
            }

            var normalMap = textureManager.RetrieveTexture(textureMapping.NormalMapID);
            GL.Uniform1(_program.GetUniformLocation("useNormalMap"), (normalMap != null) ? 1 : 0);
            if (normalMap != null)
            {
                _program.BindTexture(normalMap, "normalMap", 1);
            }

            var specularMap = textureManager.RetrieveTexture(textureMapping.SpecularMapID);
            GL.Uniform1(_program.GetUniformLocation("useSpecularMap"), (specularMap != null) ? 1 : 0);
            if (specularMap != null)
            {
                _program.BindTexture(specularMap, "specularMap", 2);
            }

            var parallaxMap = textureManager.RetrieveTexture(textureMapping.ParallaxMapID);
            GL.Uniform1(_program.GetUniformLocation("useParallaxMap"), (parallaxMap != null) ? 1 : 0);
            if (parallaxMap != null)
            {
                _program.BindTexture(parallaxMap, "parallaxMap", 3);
            }
        }

        private IEnumerable<Actor> PerformFrustumCulling(IEnumerable<Actor> actors)
        {
            // Don't render meshes that are not in the camera's view

            // Using the position of the actor, determine if we should render the mesh
            // We will also need a bounding sphere or bounding box from the mesh to determine this
            foreach (var actor in actors)
            {
                Vector3 position = actor.Model.Position;
            }

            return actors;
        }

        private IEnumerable<Actor> PerformOcclusionCulling(IEnumerable<Actor> actors)
        {
            // Don't render meshes that are obscured by closer meshes
            foreach (var actor in actors)
            {
                Vector3 position = actor.Model.Position;
            }

            return actors;
        }
    }
}
