using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Entities.Lights;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Batches;
using SweetGraphicsCore.Buffers;
using SweetGraphicsCore.Helpers;
using SweetGraphicsCore.Properties;
using SweetGraphicsCore.Rendering.Batches;
using SweetGraphicsCore.Rendering.Textures;
using SweetGraphicsCore.Shaders;

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

        private FrameBuffer _pointFrameBuffer;
        private FrameBuffer _spotFrameBuffer;

        protected override void LoadPrograms(IRenderContextProvider contextProvider)
        {
            _pointShadowProgram = ShaderHelper.LoadProgram(contextProvider,
                new[] { ShaderType.VertexShader, ShaderType.GeometryShader, ShaderType.FragmentShader },
                new[] { Resources.point_shadow_vert, Resources.point_shadow_geom, Resources.point_shadow_frag });

            _pointShadowJointProgram = ShaderHelper.LoadProgram(contextProvider,
                new[] { ShaderType.VertexShader, ShaderType.GeometryShader, ShaderType.FragmentShader },
                new[] { Resources.point_shadow_skinning_vert, Resources.point_shadow_geom, Resources.point_shadow_frag });

            _spotShadowProgram = ShaderHelper.LoadProgram(contextProvider,
                new[] { ShaderType.VertexShader, ShaderType.FragmentShader },
                new[] { Resources.spot_shadow_vert, Resources.spot_shadow_frag });

            _spotShadowJointProgram = ShaderHelper.LoadProgram(contextProvider,
                new[] { ShaderType.VertexShader, ShaderType.FragmentShader },
                new[] { Resources.spot_shadow_skinning_vert, Resources.spot_shadow_frag });
        }

        protected override void LoadTextures(IRenderContextProvider contextProvider, Resolution resolution)
        {
            PointDepthCubeMap = new Texture(contextProvider, SHADOW_SIZE, SHADOW_SIZE, 6)
            {
                Target = TextureTarget.TextureCubeMap,
                EnableMipMap = false,
                EnableAnisotropy = false,
                InternalFormat = InternalFormat.DepthComponent,
                PixelFormat = PixelFormat.DepthComponent,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.ClampToEdge
            };
            PointDepthCubeMap.Load();

            SpotDepthTexture = new Texture(contextProvider, resolution.Width, resolution.Height, 0)
            {
                Target = TextureTarget.Texture2d,
                EnableMipMap = false,
                EnableAnisotropy = false,
                InternalFormat = InternalFormat.DepthComponent,
                PixelFormat = PixelFormat.DepthComponent,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.ClampToEdge
            };
            SpotDepthTexture.Load();
        }

        protected override void LoadBuffers(IRenderContextProvider contextProvider)
        {
            _pointFrameBuffer = new FrameBuffer(contextProvider);
            _pointFrameBuffer.Add(FramebufferAttachment.DepthAttachment, PointDepthCubeMap);
            _pointFrameBuffer.Load();

            _spotFrameBuffer = new FrameBuffer(contextProvider);
            _spotFrameBuffer.Add(FramebufferAttachment.DepthAttachment, SpotDepthTexture);
            _spotFrameBuffer.Load();
        }

        protected override void Resize(Resolution resolution)
        {
            PointDepthCubeMap.Resize(SHADOW_SIZE, SHADOW_SIZE, 0);
            SpotDepthTexture.Resize(resolution.Width, resolution.Height, 0);
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
            _pointFrameBuffer.BindAndDraw(DrawBufferMode.None);

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
            _spotFrameBuffer.BindAndDraw(DrawBufferMode.None);

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

            _pointFrameBuffer.UnbindForDraw();
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

            _spotFrameBuffer.UnbindForDraw();
        }
    }
}
