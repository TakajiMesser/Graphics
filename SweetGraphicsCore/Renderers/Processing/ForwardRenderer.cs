using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Batches;
using SpiceEngineCore.Rendering.Textures;
using SweetGraphicsCore.Buffers;
using SweetGraphicsCore.Helpers;
using SweetGraphicsCore.Properties;
using SweetGraphicsCore.Rendering.Batches;
using SweetGraphicsCore.Rendering.Textures;
using SweetGraphicsCore.Shaders;
using System.Collections.Generic;

namespace SweetGraphicsCore.Renderers.Processing
{
    public class ForwardRenderer : Renderer
    {
        public Texture FinalTexture { get; protected set; }
        public Texture VelocityTexture { get; protected set; }
        public Texture DepthTexture { get; protected set; }

        internal ShaderProgram _program;
        internal FrameBuffer _frameBuffer;

        protected override void LoadPrograms(IRenderContext renderContext)
        {
            _program = ShaderHelper.LoadProgram(renderContext,
                new[] { ShaderType.VertexShader, ShaderType.FragmentShader },
                new[] { Resources.forward_vert, Resources.forward_frag });
        }

        protected override void LoadTextures(IRenderContext renderContext, Resolution resolution)
        {
            FinalTexture = new Texture(renderContext, resolution.Width, resolution.Height, 0)
            {
                Target = TextureTarget.Texture2d,
                EnableMipMap = false,
                EnableAnisotropy = false,
                InternalFormat = InternalFormat.Rgba16f,
                PixelFormat = PixelFormat.Rgba,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            FinalTexture.Load();

            VelocityTexture = new Texture(renderContext, resolution.Width, resolution.Height, 0)
            {
                Target = TextureTarget.Texture2d,
                EnableMipMap = false,
                EnableAnisotropy = false,
                InternalFormat = InternalFormat.Rg16f,
                PixelFormat = PixelFormat.Rg,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            VelocityTexture.Load();

            DepthTexture = new Texture(renderContext, resolution.Width, resolution.Height, 0)
            {
                Target = TextureTarget.Texture2d,
                EnableMipMap = false,
                EnableAnisotropy = false,
                InternalFormat = InternalFormat.DepthComponent32f,
                PixelFormat = PixelFormat.DepthComponent,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            DepthTexture.Load();
        }

        protected override void LoadBuffers(IRenderContext renderContext)
        {
            _frameBuffer = new FrameBuffer(renderContext);
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment0, FinalTexture);
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment1, VelocityTexture);
            //_frameBuffer.Add(FramebufferAttachment.DepthStencilAttachment, new RenderBuffer(RenderbufferTarget.Renderbuffer, Resolution.Width, Resolution.Height));
            _frameBuffer.Add(FramebufferAttachment.DepthAttachment, DepthTexture);

            _frameBuffer.Load();
        }

        protected override void Resize(Resolution resolution)
        {
            FinalTexture.Resize(resolution.Width, resolution.Height, 0);
            VelocityTexture.Resize(resolution.Width, resolution.Height, 0);
            DepthTexture.Resize(resolution.Width, resolution.Height, 0);
        }

        public void Render(Resolution resolution, ICamera camera, IBatcher batcher)
        {
            // TODO - Where does frame buffer binding and GL value setting fit into BatchAction?
            //_program.Use();
            _frameBuffer.BindAndDraw();

            GL.ClearColor(Color4.Black);
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
            GL.Uniform1i(_program.GetUniformLocation("useDiffuseMap"), (diffuseMap != null) ? 1 : 0);
            if (diffuseMap != null)
            {
                _program.BindTexture(diffuseMap, "diffuseMap", 0);
            }

            var normalMap = textureProvider.RetrieveTexture(textureMapping.NormalIndex);
            GL.Uniform1i(_program.GetUniformLocation("useNormalMap"), (normalMap != null) ? 1 : 0);
            if (normalMap != null)
            {
                _program.BindTexture(normalMap, "normalMap", 1);
            }

            var specularMap = textureProvider.RetrieveTexture(textureMapping.SpecularIndex);
            GL.Uniform1i(_program.GetUniformLocation("useSpecularMap"), (specularMap != null) ? 1 : 0);
            if (specularMap != null)
            {
                _program.BindTexture(specularMap, "specularMap", 2);
            }

            var parallaxMap = textureProvider.RetrieveTexture(textureMapping.ParallaxIndex);
            GL.Uniform1i(_program.GetUniformLocation("useParallaxMap"), (parallaxMap != null) ? 1 : 0);
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
