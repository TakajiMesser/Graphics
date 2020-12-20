using OpenTK.Graphics.OpenGL;
using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Batches;
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

        private FrameBuffer _frameBuffer = new FrameBuffer();

        private ShaderProgram _geometryProgram;
        private ShaderProgram _jointGeometryProgram;

        protected override void LoadPrograms()
        {
            _geometryProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, Resources.deferred_vert),
                //new Shader(ShaderType.TessControlShader, File.ReadAllText(FilePathHelper.DEFERRED_TESS_CONTROL_SHADER_PATH)),
                //new Shader(ShaderType.TessEvaluationShader, File.ReadAllText(FilePathHelper.DEFERRED_TESS_EVAL_SHADER_PATH)),
                //new Shader(ShaderType.GeometryShader, File.ReadAllText(FilePathHelper.DEFERRED_GEOMETRY_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, Resources.deferred_frag)
            );

            _jointGeometryProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, Resources.deferred_skinning_vert),
                //new Shader(ShaderType.TessControlShader, File.ReadAllText(FilePathHelper.DEFERRED_TESS_CONTROL_SHADER_PATH)),
                //new Shader(ShaderType.TessEvaluationShader, File.ReadAllText(FilePathHelper.DEFERRED_TESS_EVAL_SHADER_PATH)),
                //new Shader(ShaderType.GeometryShader, File.ReadAllText(FilePathHelper.DEFERRED_GEOMETRY_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, Resources.deferred_frag)
            );
        }

        public override void Resize(Resolution resolution)
        {
            PositionTexture.Resize(resolution.Width, resolution.Height, 0);
            PositionTexture.Bind();
            PositionTexture.ReserveMemory();

            ColorTexture.Resize(resolution.Width, resolution.Height, 0);
            ColorTexture.Bind();
            ColorTexture.ReserveMemory();

            NormalTexture.Resize(resolution.Width, resolution.Height, 0);
            NormalTexture.Bind();
            NormalTexture.ReserveMemory();

            DiffuseMaterialTexture.Resize(resolution.Width, resolution.Height, 0);
            DiffuseMaterialTexture.Bind();
            DiffuseMaterialTexture.ReserveMemory();

            SpecularTexture.Resize(resolution.Width, resolution.Height, 0);
            SpecularTexture.Bind();
            SpecularTexture.ReserveMemory();

            VelocityTexture.Resize(resolution.Width, resolution.Height, 0);
            VelocityTexture.Bind();
            VelocityTexture.ReserveMemory();

            FinalTexture.Resize(resolution.Width, resolution.Height, 0);
            FinalTexture.Bind();
            FinalTexture.ReserveMemory();

            DepthStencilTexture.Resize(resolution.Width, resolution.Height, 0);
            DepthStencilTexture.Bind();
            DepthStencilTexture.ReserveMemory();
        }

        protected override void LoadTextures(Resolution resolution)
        {
            PositionTexture = new Texture(resolution.Width, resolution.Height, 0)
            {
                Target = TextureTarget.Texture2D,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.Rgb16f,
                PixelFormat = PixelFormat.Rgb,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            PositionTexture.Bind();
            PositionTexture.ReserveMemory();

            ColorTexture = new Texture(resolution.Width, resolution.Height, 0)
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
            ColorTexture.Bind();
            ColorTexture.ReserveMemory();

            NormalTexture = new Texture(resolution.Width, resolution.Height, 0)
            {
                Target = TextureTarget.Texture2D,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.Rgb16f,
                PixelFormat = PixelFormat.Rgb,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            NormalTexture.Bind();
            NormalTexture.ReserveMemory();

            DiffuseMaterialTexture = new Texture(resolution.Width, resolution.Height, 0)
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
            DiffuseMaterialTexture.Bind();
            DiffuseMaterialTexture.ReserveMemory();

            SpecularTexture = new Texture(resolution.Width, resolution.Height, 0)
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
            SpecularTexture.Bind();
            SpecularTexture.ReserveMemory();

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

            DepthStencilTexture = new Texture(resolution.Width, resolution.Height, 0)
            {
                Target = TextureTarget.Texture2D,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.Depth32fStencil8,
                PixelFormat = PixelFormat.DepthComponent,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            DepthStencilTexture.Bind();
            DepthStencilTexture.ReserveMemory();
        }

        protected override void LoadBuffers()
        {
            _frameBuffer.Clear();
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment0, PositionTexture);
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment1, ColorTexture);
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment2, NormalTexture);
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment3, DiffuseMaterialTexture);
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment4, SpecularTexture);
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment5, VelocityTexture);
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment6, FinalTexture);
            _frameBuffer.Add(FramebufferAttachment.DepthStencilAttachment, DepthStencilTexture);

            _frameBuffer.Bind(FramebufferTarget.Framebuffer);
            _frameBuffer.AttachAttachments();
            _frameBuffer.Unbind(FramebufferTarget.Framebuffer);
        }

        public void BindForGeometryWriting()
        {
            _frameBuffer.BindAndDraw(new DrawBuffersEnum[]
            {
                DrawBuffersEnum.ColorAttachment0,
                DrawBuffersEnum.ColorAttachment1,
                DrawBuffersEnum.ColorAttachment2,
                DrawBuffersEnum.ColorAttachment3,
                DrawBuffersEnum.ColorAttachment4,
                DrawBuffersEnum.ColorAttachment5,
                DrawBuffersEnum.ColorAttachment6
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

        public void BindForTransparentWriting()
        {
            _frameBuffer.BindAndDraw(new DrawBuffersEnum[]
            {
                DrawBuffersEnum.ColorAttachment0,
                DrawBuffersEnum.ColorAttachment1
            });
        }

        public void BindForLitTransparentWriting()
        {
            _frameBuffer.BindAndDraw(new DrawBuffersEnum[]
            {
                DrawBuffersEnum.ColorAttachment0,
                DrawBuffersEnum.ColorAttachment6
            });
        }

        public void BindForDiffuseWriting()
        {
            _frameBuffer.BindAndDraw(DrawBuffersEnum.ColorAttachment1);
        }

        public void BindForLitWriting()
        {
            _frameBuffer.BindAndDraw(DrawBuffersEnum.ColorAttachment6);
        }

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
