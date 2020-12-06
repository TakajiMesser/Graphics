using OpenTK.Graphics.OpenGL;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Entities.Lights;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Batches;
using SpiceEngineCore.Rendering.Textures;
using SweetGraphicsCore.Buffers;
using SweetGraphicsCore.Properties;
using SweetGraphicsCore.Renderers.Shaders;
using SweetGraphicsCore.Rendering.Textures;

namespace SweetGraphicsCore.Renderers.Processing
{
    public class ShadowRenderer : Renderer
    {
        public const int SHADOW_SIZE = 1024;

        private ShaderProgram _pointShadowProgram;
        private ShaderProgram _pointShadowJointProgram;
        private ShaderProgram _spotShadowProgram;
        private ShaderProgram _spotShadowJointProgram;

        private FrameBuffer _pointFrameBuffer = new FrameBuffer();
        private FrameBuffer _spotFrameBuffer = new FrameBuffer();

        public ShadowRenderer(ITextureProvider textureProvider) : base(textureProvider) { }

        public Texture PointDepthCubeMap { get; protected set; }
        public Texture SpotDepthTexture { get; protected set; }

        protected override void LoadShaders()
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
            _pointShadowProgram.Use();
            _pointShadowProgram.RenderLightFromCameraPerspective(light, camera);
            _pointShadowProgram.SetUniform("lightRadius", light.Radius);
            _pointShadowProgram.SetUniform("lightPosition", light.Position);

            foreach (var batch in batcher.GetBatches(RenderTypes.OpaqueStatic))
            {
                RenderBatch(_pointShadowProgram, batcher, batch); // Draw all geometry, but only the positions
            }

            _pointShadowJointProgram.Use();
            _pointShadowJointProgram.RenderLightFromCameraPerspective(light, camera);
            _pointShadowJointProgram.SetUniform("lightRadius", light.Radius);
            _pointShadowJointProgram.SetUniform("lightPosition", light.Position);

            foreach (var batch in batcher.GetBatches(RenderTypes.OpaqueAnimated))
            {
                RenderBatch(_pointShadowJointProgram, batcher, batch); // Draw all geometry, but only the positions
            }

            _pointFrameBuffer.Unbind(FramebufferTarget.DrawFramebuffer);
        }

        private void SpotLightPass(ICamera camera, SpotLight light, IBatcher batcher)
        {
            _spotShadowProgram.Use();
            _spotShadowProgram.RenderLightFromCameraPerspective(light, camera);

            foreach (var batch in batcher.GetBatches(RenderTypes.OpaqueStatic))
            {
                RenderBatch(_spotShadowProgram, batcher, batch); // Draw all geometry, but only the positions
            }

            _spotShadowJointProgram.Use();
            _spotShadowJointProgram.RenderLightFromCameraPerspective(light, camera);

            foreach (var batch in batcher.GetBatches(RenderTypes.OpaqueAnimated))
            {
                RenderBatch(_spotShadowJointProgram, batcher, batch); // Draw all geometry, but only the positions
            }

            _spotFrameBuffer.Unbind(FramebufferTarget.DrawFramebuffer);
        }
    }
}
