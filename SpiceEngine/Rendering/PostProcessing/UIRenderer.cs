using OpenTK.Graphics.OpenGL;
using SpiceEngine.Properties;
using SpiceEngine.Rendering.Textures;
using SpiceEngineCore.Outputs;
using SpiceEngineCore.Rendering.Buffers;
using SpiceEngineCore.Rendering.Processing;
using SpiceEngineCore.Rendering.Shaders;

namespace SpiceEngine.Rendering.PostProcessing
{
    public class UIRenderer : Renderer
    {
        private ShaderProgram _uiProgram;
        private FrameBuffer _frameBuffer = new FrameBuffer();

        public Texture FinalTexture { get; protected set; }
        
        protected override void LoadPrograms()
        {
            _uiProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, Resources.ui_vert),
                new Shader(ShaderType.FragmentShader, Resources.ui_frag)
            );
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
        }

        protected override void LoadBuffers()
        {
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment0, FinalTexture);
            _frameBuffer.Bind(FramebufferTarget.Framebuffer);
            _frameBuffer.AttachAttachments();
            _frameBuffer.Unbind(FramebufferTarget.Framebuffer);
        }

        public override void ResizeTextures(Resolution resolution)
        {
            FinalTexture.Resize(resolution.Width, resolution.Height, 0);
            FinalTexture.Bind();
            FinalTexture.ReserveMemory();
        }
    }
}
