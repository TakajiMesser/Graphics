using Graphics.GameObjects;
using Graphics.Helpers;
using Graphics.Lighting;
using Graphics.Meshes;
using Graphics.Outputs;
using Graphics.Rendering.Buffers;
using Graphics.Rendering.Shaders;
using Graphics.Rendering.Textures;
using Graphics.Rendering.Vertices;
using Graphics.Utilities;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Rendering.Processing
{
    public class ShadowRenderer
    {
        public Resolution Resolution { get; private set; }

        public Texture PointShadowDepthTexture { get; protected set; }
        public Texture PointShadowTexture { get; protected set; }

        internal ShaderProgram _pointShadowProgram;

        private FrameBuffer _frameBuffer = new FrameBuffer();

        public ShadowRenderer(Resolution resolution)
        {
            Resolution = resolution;
        }

        public void Load()
        {
            LoadPrograms();
            LoadBuffers();
        }

        protected void LoadPrograms()
        {
            _pointShadowProgram = new ShaderProgram(new[] {
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.LIGHT_VERTEX_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.POINT_LIGHT_FRAGMENT_SHADER_PATH))
            });
        }

        public void ResizeTextures()
        {
            PointShadowDepthTexture.Resize(Resolution.Width, Resolution.Height, 0);
            PointShadowDepthTexture.Bind();
            PointShadowDepthTexture.ReserveMemory();

            PointShadowTexture.Resize(Resolution.Width, Resolution.Height, 0);
            PointShadowTexture.Bind();
            PointShadowTexture.ReserveMemory();
        }

        protected void LoadBuffers()
        {
            PointShadowDepthTexture = new Texture(Resolution.Width, Resolution.Height, 0)
            {
                Target = TextureTarget.Texture2D,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.DepthComponent32f,
                PixelFormat = PixelFormat.DepthComponent,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.ClampToEdge
            };
            PointShadowDepthTexture.Bind();
            PointShadowDepthTexture.ReserveMemory();

            PointShadowTexture = new Texture(Resolution.Width, Resolution.Height, 0)
            {
                Target = TextureTarget.Texture2D,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.DepthComponent32f,
                PixelFormat = PixelFormat.DepthComponent,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.ClampToEdge
            };
            PointShadowTexture.Bind();
            PointShadowTexture.ReserveMemory();

            _frameBuffer.Clear();
            _frameBuffer.Add(FramebufferAttachment.DepthAttachment, PointShadowDepthTexture);
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment0, PointShadowTexture);

            _frameBuffer.Bind(FramebufferTarget.Framebuffer);
            _frameBuffer.AttachAttachments();
            _frameBuffer.Unbind(FramebufferTarget.Framebuffer);
        }

        private void PointLightPass()
        {
            _frameBuffer.Bind(FramebufferTarget.DrawFramebuffer);
            _frameBuffer.Draw(DrawBuffersEnum.None);
        }

        private void SpotLightPass()
        {

        }

        public void Render(Texture color, Texture normalDepth, Texture ambient, Texture diffuseMaterial, Texture specular, FrameBuffer gBuffer, Camera camera, IEnumerable<PointLight> pointLights)
        {
            PointLightPass();
        }
    }
}
