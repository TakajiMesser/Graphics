using OpenTK.Graphics.OpenGL;
using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Batches;
using SpiceEngineCore.Rendering.Textures;
using SweetGraphicsCore.Buffers;
using SweetGraphicsCore.Properties;
using SweetGraphicsCore.Rendering.Batches;
using SweetGraphicsCore.Rendering.Textures;
using SweetGraphicsCore.Shaders;
using System.Collections.Generic;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SweetGraphicsCore.Renderers.Processing
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

        protected override void Resize(Resolution resolution)
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

        public void Render(Resolution resolution, ICamera camera, IBatcher batcher)
        {
            // TODO - Where does frame buffer binding and GL value setting fit into BatchAction?
            //_program.Use();
            _frameBuffer.BindAndDraw();

            GL.ClearColor(OpenTK.Graphics.Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, resolution.Width, resolution.Height);

            batcher.CreateBatchAction()
                .SetShader(_program)
                .SetCamera(camera)
                .SetRenderType(RenderTypes.OpaqueStatic)
                .Render();
        }

        private void BindTextures(ITextureProvider textureProvider, TextureMapping textureMapping)
        {
            // TODO - Order brush rendering in a way that allows us to not re-bind duplicate textures repeatedly
            // Check brush's texture mapping to see which textures we need to bind
            var diffuseMap = textureProvider.RetrieveTexture(textureMapping.DiffuseIndex);
            GL.Uniform1(_program.GetUniformLocation("useDiffuseMap"), (diffuseMap != null) ? 1 : 0);
            if (diffuseMap != null)
            {
                _program.BindTexture(diffuseMap, "diffuseMap", 0);
            }

            var normalMap = textureProvider.RetrieveTexture(textureMapping.NormalIndex);
            GL.Uniform1(_program.GetUniformLocation("useNormalMap"), (normalMap != null) ? 1 : 0);
            if (normalMap != null)
            {
                _program.BindTexture(normalMap, "normalMap", 1);
            }

            var specularMap = textureProvider.RetrieveTexture(textureMapping.SpecularIndex);
            GL.Uniform1(_program.GetUniformLocation("useSpecularMap"), (specularMap != null) ? 1 : 0);
            if (specularMap != null)
            {
                _program.BindTexture(specularMap, "specularMap", 2);
            }

            var parallaxMap = textureProvider.RetrieveTexture(textureMapping.ParallaxIndex);
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
                Vector3 position = actor.Position;
            }

            return actors;
        }

        private IEnumerable<Actor> PerformOcclusionCulling(IEnumerable<Actor> actors)
        {
            // Don't render meshes that are obscured by closer meshes
            foreach (var actor in actors)
            {
                Vector3 position = actor.Position;
            }

            return actors;
        }
    }
}
