using OpenTK.Graphics.OpenGL;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Entities.Lights;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Batches;
using SpiceEngineCore.Rendering.Shaders;
using SweetGraphicsCore.Buffers;
using SweetGraphicsCore.Properties;
using SweetGraphicsCore.Rendering.Batches;
using SweetGraphicsCore.Rendering.Textures;

namespace SweetGraphicsCore.Renderers.Processing
{
    public class ShadowRenderer : Renderer
    {
        public const int SHADOW_SIZE = 1024;

        public Texture PointDepthCubeMap { get; protected set; }
        public Texture SpotDepthTexture { get; protected set; }

        private ShaderProgram _pointShadowProgram;
        private ShaderProgram _pointShadowJointProgram;
        private ShaderProgram _spotShadowProgram;
        private ShaderProgram _spotShadowJointProgram;

        private FrameBuffer _pointFrameBuffer = new FrameBuffer();
        private FrameBuffer _spotFrameBuffer = new FrameBuffer();

        protected override void LoadPrograms()
        {
            _pointShadowProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, Resources.point_shadow_vert),
                new Shader(ShaderType.GeometryShader, Resources.point_shadow_geom),
                new Shader(ShaderType.FragmentShader, Resources.point_shadow_frag)
            );

            _pointShadowJointProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, Resources.point_shadow_skinning_vert),
                new Shader(ShaderType.GeometryShader, Resources.point_shadow_geom),
                new Shader(ShaderType.FragmentShader, Resources.point_shadow_frag)
            );

            _spotShadowProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, Resources.spot_shadow_vert),
                new Shader(ShaderType.FragmentShader, Resources.spot_shadow_frag)
            );

            _spotShadowJointProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, Resources.spot_shadow_skinning_vert),
                new Shader(ShaderType.FragmentShader, Resources.spot_shadow_frag)
            );
        }

        public override void Resize(Resolution resolution)
        {
            PointDepthCubeMap.Resize(SHADOW_SIZE, SHADOW_SIZE, 0);
            PointDepthCubeMap.Bind();
            PointDepthCubeMap.ReserveMemory();

            SpotDepthTexture.Resize(resolution.Width, resolution.Height, 0);
            SpotDepthTexture.Bind();
            SpotDepthTexture.ReserveMemory();
        }

        protected override void LoadTextures(Resolution resolution)
        {
            PointDepthCubeMap = new Texture(SHADOW_SIZE, SHADOW_SIZE, 6)
            {
                Target = TextureTarget.TextureCubeMap,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.DepthComponent,
                PixelFormat = PixelFormat.DepthComponent,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.ClampToEdge
            };
            PointDepthCubeMap.Bind();
            PointDepthCubeMap.ReserveMemory();

            SpotDepthTexture = new Texture(resolution.Width, resolution.Height, 0)
            {
                Target = TextureTarget.Texture2D,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.DepthComponent,
                PixelFormat = PixelFormat.DepthComponent,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.ClampToEdge
            };
            SpotDepthTexture.Bind();
            SpotDepthTexture.ReserveMemory();
        }

        protected override void LoadBuffers()
        {
            _pointFrameBuffer.Clear();
            _pointFrameBuffer.Add(FramebufferAttachment.DepthAttachment, PointDepthCubeMap);

            _pointFrameBuffer.Bind(FramebufferTarget.Framebuffer);
            _pointFrameBuffer.AttachAttachments();
            _pointFrameBuffer.Unbind(FramebufferTarget.Framebuffer);

            _spotFrameBuffer.Clear();
            _spotFrameBuffer.Add(FramebufferAttachment.DepthAttachment, SpotDepthTexture);

            _spotFrameBuffer.Bind(FramebufferTarget.Framebuffer);
            _spotFrameBuffer.AttachAttachments();
            _spotFrameBuffer.Unbind(FramebufferTarget.Framebuffer);
        }

        public void Render(ICamera camera, ILight light, IBatcher batcher)
        {
            switch (light)
            {
                case PointLight pLight:
                    BindForPointShadowDrawing();
                    PointLightPass(camera, pLight, batcher);
                    break;
                case SpotLight sLight:
                    BindForSpotShadowDrawing();
                    SpotLightPass(camera, sLight, batcher);
                    break;
            }
        }

        private void BindForPointShadowDrawing()
        {
            _pointFrameBuffer.BindAndDraw(DrawBuffersEnum.None);

            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.StencilTest);

            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, SHADOW_SIZE, SHADOW_SIZE);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.StencilFunc(StencilFunction.Notequal, 0, 0xFF);
        }

        private void BindForSpotShadowDrawing()
        {
            _spotFrameBuffer.BindAndDraw(DrawBuffersEnum.None);

            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.StencilTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.StencilFunc(StencilFunction.Notequal, 0, 0xFF);
            GL.Clear(ClearBufferMask.DepthBufferBit);
        }

        private void PointLightPass(ICamera camera, PointLight light, IBatcher batcher)
        {
            batcher.CreateBatchAction()
                .SetShader(_pointShadowProgram)
                .SetLightView(camera, light) // Draw camera from the point light's perspective
                .SetUniform("lightRadius", light.Radius)
                .SetUniform("lightPosition", light.Position)
                .SetRenderType(RenderTypes.OpaqueStatic)
                .Render() // Draw all geometry, but only the positions
                .SetShader(_pointShadowJointProgram)
                .SetLightView(camera, light)
                .SetUniform("lightRadius", light.Radius)
                .SetUniform("lightPosition", light.Position)
                .SetRenderType(RenderTypes.OpaqueAnimated)
                .Render()
                .Execute();

            _pointFrameBuffer.Unbind(FramebufferTarget.DrawFramebuffer);
        }

        private void SpotLightPass(ICamera camera, SpotLight light, IBatcher batcher)
        {
            batcher.CreateBatchAction()
                .SetShader(_spotShadowProgram)
                .SetLightView(camera, light) // Draw camera from the point light's perspective
                .SetRenderType(RenderTypes.OpaqueStatic)
                .Render() // Draw all geometry, but only the positions
                .SetShader(_spotShadowJointProgram)
                .SetLightView(camera, light)
                .SetRenderType(RenderTypes.OpaqueAnimated)
                .Render()
                .Execute();

            _spotFrameBuffer.Unbind(FramebufferTarget.DrawFramebuffer);
        }
    }
}
