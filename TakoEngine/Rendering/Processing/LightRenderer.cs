using OpenTK.Graphics.OpenGL;
using System;
using System.IO;
using TakoEngine.Entities.Cameras;
using TakoEngine.Entities.Lights;
using TakoEngine.Helpers;
using TakoEngine.Meshes;
using TakoEngine.Outputs;
using TakoEngine.Rendering.Buffers;
using TakoEngine.Rendering.Shaders;
using TakoEngine.Rendering.Textures;

namespace TakoEngine.Rendering.Processing
{
    public class LightRenderer : Renderer
    {
        public Texture DepthStencilTexture { get; protected set; }
        public Texture FinalTexture { get; protected set; }

        internal ShaderProgram _stencilProgram;
        internal ShaderProgram _pointLightProgram;
        internal ShaderProgram _spotLightProgram;
        internal ShaderProgram _simpleProgram;

        private FrameBuffer _frameBuffer = new FrameBuffer();

        private SimpleMesh _pointLightMesh;
        private SimpleMesh _spotLightMesh;

        protected override void LoadPrograms()
        {
            _stencilProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.STENCIL_VERTEX_SHADER_PATH))
            );

            _pointLightProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.LIGHT_VERTEX_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.POINT_LIGHT_FRAGMENT_SHADER_PATH))
            );

            _spotLightProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.LIGHT_VERTEX_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.SPOT_LIGHT_FRAGMENT_SHADER_PATH))
            );

            _simpleProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.SIMPLE_VERTEX_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.SIMPLE_FRAGMENT_SHADER_PATH))
            );
        }

        public override void ResizeTextures(Resolution resolution)
        {
            DepthStencilTexture.Resize(resolution.Width, resolution.Height, 0);
            DepthStencilTexture.Bind();
            DepthStencilTexture.ReserveMemory();

            FinalTexture.Resize(resolution.Width, resolution.Height, 0);
            FinalTexture.Bind();
            FinalTexture.ReserveMemory();
        }

        protected override void LoadTextures(Resolution resolution)
        {
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
        }

        protected override void LoadBuffers()
        {
            _frameBuffer.Clear();
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment0, FinalTexture);
            _frameBuffer.Add(FramebufferAttachment.DepthStencilAttachment, DepthStencilTexture);

            _frameBuffer.Bind(FramebufferTarget.Framebuffer);
            _frameBuffer.AttachAttachments();
            _frameBuffer.Unbind(FramebufferTarget.Framebuffer);

            _pointLightMesh = SimpleMesh.LoadFromFile(FilePathHelper.SPHERE_MESH_PATH, _pointLightProgram);
            _spotLightMesh = SimpleMesh.LoadFromFile(FilePathHelper.CONE_MESH_PATH, _spotLightProgram);
        }

        public void StencilPass(Light light, Camera camera, SimpleMesh mesh)
        {
            _stencilProgram.Use();
            GL.DrawBuffer(DrawBufferMode.None);

            GL.DepthMask(false);
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);

            GL.Clear(ClearBufferMask.StencilBufferBit);

            GL.StencilFunc(StencilFunction.Always, 0, 0);
            GL.StencilOpSeparate(StencilFace.Back, StencilOp.Keep, StencilOp.IncrWrap, StencilOp.Keep);
            GL.StencilOpSeparate(StencilFace.Front, StencilOp.Keep, StencilOp.DecrWrap, StencilOp.Keep);

            camera.Draw(_stencilProgram);
            light.DrawForStencilPass(_stencilProgram);
            mesh.Draw();
        }

        public void LightPass(Resolution resolution, DeferredRenderer deferredRenderer, Light light, Camera camera, SimpleMesh mesh, Texture shadowMap, ShaderProgram program)
        {
            program.Use();

            //GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, deferredRenderer.GBuffer._handle);
            GL.DrawBuffer(DrawBufferMode.ColorAttachment6);

            GL.StencilFunc(StencilFunction.Notequal, 0, 0xFF);
            GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);

            program.BindTexture(deferredRenderer.PositionTexture, "positionMap", 0);
            program.BindTexture(deferredRenderer.ColorTexture, "colorMap", 1);
            program.BindTexture(deferredRenderer.NormalTexture, "normalMap", 2);
            program.BindTexture(deferredRenderer.DiffuseMaterialTexture, "diffuseMaterial", 3);
            program.BindTexture(deferredRenderer.SpecularTexture, "specularMap", 4);
            program.BindTexture(shadowMap, "shadowMap", 5);

            camera.Draw(program);
            program.SetUniform("cameraPosition", camera.Position);

            light.DrawForLightPass(resolution, program);
            mesh.Draw();
        }

        public SimpleMesh GetMeshForLight(Light light)
        {
            switch (light)
            {
                case PointLight p:
                    return _pointLightMesh;
                case SpotLight s:
                    return _spotLightMesh;
                default:
                    throw new NotImplementedException("Could not handle light type " + light.GetType());
            }
        }

        public ShaderProgram GetProgramForLight(Light light)
        {
            switch (light)
            {
                case PointLight p:
                    return _pointLightProgram;
                case SpotLight s:
                    return _spotLightProgram;
                default:
                    throw new NotImplementedException("Could not handle light type " + light.GetType());
            }
        }
    }
}
