using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Batches;
using SweetGraphicsCore.Buffers;
using SweetGraphicsCore.Helpers;
using SweetGraphics.Properties;
using SweetGraphicsCore.Rendering.Batches;
using SweetGraphicsCore.Rendering.Textures;
using SweetGraphicsCore.Shaders;
using System.Collections.Generic;

namespace SweetGraphicsCore.Renderers.Processing
{
    public class DeferredRenderer : Renderer
    {
        public Texture PositionTexture { get; protected set; }
        public Texture ColorTexture { get; protected set; }
        public Texture NormalTexture { get; protected set; }
        public Texture DiffuseMaterialTexture { get; protected set; }
        public Texture SpecularTexture { get; protected set; }
        public Texture VelocityTexture { get; protected set; }
        public Texture FinalTexture { get; protected set; }
        public Texture DepthStencilTexture { get; protected set; }

        private ShaderProgram _geometryProgram;
        private ShaderProgram _jointGeometryProgram;

        private FrameBuffer _frameBuffer;

        protected override void LoadPrograms(IRenderContext renderContext)
        {
            _geometryProgram = ShaderHelper.LoadProgram(renderContext,
                new[] { ShaderType.VertexShader, ShaderType.FragmentShader },
                new[] { Resources.deferred_vert, Resources.deferred_frag });

            _jointGeometryProgram = ShaderHelper.LoadProgram(renderContext,
                new[] { ShaderType.VertexShader, ShaderType.FragmentShader },
                new[] { Resources.deferred_skinning_vert, Resources.deferred_frag });
        }

        protected override void LoadTextures(IRenderContext renderContext, Resolution resolution)
        {
            PositionTexture = new Texture(renderContext, resolution.Width, resolution.Height, 0)
            {
                Target = TextureTarget.Texture2d,
                EnableMipMap = false,
                EnableAnisotropy = false,
                InternalFormat = InternalFormat.Rgb16f,
                PixelFormat = PixelFormat.Rgb,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            PositionTexture.Load();

            ColorTexture = new Texture(renderContext, resolution.Width, resolution.Height, 0)
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
            ColorTexture.Load();

            NormalTexture = new Texture(renderContext, resolution.Width, resolution.Height, 0)
            {
                Target = TextureTarget.Texture2d,
                EnableMipMap = false,
                EnableAnisotropy = false,
                InternalFormat = InternalFormat.Rgb16f,
                PixelFormat = PixelFormat.Rgb,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            NormalTexture.Load();

            DiffuseMaterialTexture = new Texture(renderContext, resolution.Width, resolution.Height, 0)
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
            DiffuseMaterialTexture.Load();

            SpecularTexture = new Texture(renderContext, resolution.Width, resolution.Height, 0)
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
            SpecularTexture.Load();

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

            DepthStencilTexture = new Texture(renderContext, resolution.Width, resolution.Height, 0)
            {
                Target = TextureTarget.Texture2d,
                EnableMipMap = false,
                EnableAnisotropy = false,
                InternalFormat = InternalFormat.Depth32fStencil8,
                PixelFormat = PixelFormat.DepthComponent,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            DepthStencilTexture.Load();
        }

        protected override void LoadBuffers(IRenderContext renderContext)
        {
            _frameBuffer = new FrameBuffer(renderContext);
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment0, PositionTexture);
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment1, ColorTexture);
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment2, NormalTexture);
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment3, DiffuseMaterialTexture);
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment4, SpecularTexture);
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment5, VelocityTexture);
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment6, FinalTexture);
            //_frameBuffer.Add(FramebufferAttachment.StencilAttachment, DepthStencilTexture);
            _frameBuffer.AddDepthStencilTexture(DepthStencilTexture);
            //InvalidateFramebufferAttachment.DepthStencilAttachment;

            _frameBuffer.Load();
        }

        protected override void Resize(Resolution resolution)
        {
            PositionTexture.Resize(resolution.Width, resolution.Height, 0);
            ColorTexture.Resize(resolution.Width, resolution.Height, 0);
            NormalTexture.Resize(resolution.Width, resolution.Height, 0);
            DiffuseMaterialTexture.Resize(resolution.Width, resolution.Height, 0);
            SpecularTexture.Resize(resolution.Width, resolution.Height, 0);
            VelocityTexture.Resize(resolution.Width, resolution.Height, 0);
            FinalTexture.Resize(resolution.Width, resolution.Height, 0);
            DepthStencilTexture.Resize(resolution.Width, resolution.Height, 0);
        }

        public void BindForGeometryWriting()
        {
            _frameBuffer.BindAndDraw(new []
            {
                DrawBufferMode.ColorAttachment0,
                DrawBufferMode.ColorAttachment1,
                DrawBufferMode.ColorAttachment2,
                DrawBufferMode.ColorAttachment3,
                DrawBufferMode.ColorAttachment4,
                DrawBufferMode.ColorAttachment5,
                DrawBufferMode.ColorAttachment6
            });
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);
            GL.DepthMask(true);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            //GL.Disable(EnableCap.CullFace);
        }

        public void BindForTransparentWriting() => _frameBuffer.BindAndDraw(new[]
        {
            DrawBufferMode.ColorAttachment0,
            DrawBufferMode.ColorAttachment1
        });

        public void BindForLitTransparentWriting() => _frameBuffer.BindAndDraw(new[]
        {
            DrawBufferMode.ColorAttachment0,
            DrawBufferMode.ColorAttachment6
        });

        public void BindForDiffuseWriting() => _frameBuffer.BindAndDraw(DrawBufferMode.ColorAttachment1);

        public void BindForLitWriting() => _frameBuffer.BindAndDraw(DrawBufferMode.ColorAttachment6);

        public void GeometryPass(ICamera camera, IBatcher batcher)
        {
            batcher.CreateBatchAction()
                .SetShader(_geometryProgram)
                .SetCamera(camera)
                .SetUniform("cameraPosition", camera.Position)
                .SetRenderType(RenderTypes.OpaqueStatic)
                .Render()
                .SetShader(_jointGeometryProgram)
                .SetCamera(camera)
                .SetUniform("cameraPosition", camera.Position)
                .SetRenderType(RenderTypes.OpaqueAnimated)
                .Render()
                .Execute();

            GL.Enable(EnableCap.CullFace);
        }

        public void TransparentGeometryPass(ICamera camera, IBatcher batcher)
        {
            batcher.CreateBatchAction()
                .SetShader(_geometryProgram)
                .SetCamera(camera)
                .SetUniform("cameraPosition", camera.Position)
                .PerformAction(() => _geometryProgram.UnbindTextures())
                .SetRenderType(RenderTypes.TransparentStatic)
                .Render()
                .SetShader(_jointGeometryProgram)
                .SetCamera(camera)
                .SetUniform("cameraPosition", camera.Position)
                .SetRenderType(RenderTypes.TransparentAnimated)
                .Render()
                .Execute();

            //GL.Enable(EnableCap.Blend);
            //GL.BlendEquation(BlendEquationMode.FuncAdd);
            //GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);

            //GL.Disable(EnableCap.Blend);
        }

        private IEnumerable<Actor> PerformFrustumCulling(IEnumerable<Actor> actors)
        {
            // TODO - Don't render meshes that are not in the camera's view

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
            // TODO - Don't render meshes that are obscured by closer meshes
            foreach (var actor in actors)
            {
                Vector3 position = actor.Position;
            }

            return actors;
        }
    }
}
