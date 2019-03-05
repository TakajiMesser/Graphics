using OpenTK;
using OpenTK.Graphics.OpenGL;
using SpiceEngine.Outputs;
using SpiceEngine.Properties;
using SpiceEngine.Rendering.Buffers;
using SpiceEngine.Rendering.Processing;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;
using SpiceEngine.Utilities;

namespace SpiceEngine.Rendering.PostProcessing
{
    public class Blur : Renderer
    {
        public Texture FinalTexture { get; protected set; }

        private ShaderProgram _blurProgram;

        private int _vertexArrayHandle;
        private VertexBuffer<Simple3DVertex> _vertexBuffer = new VertexBuffer<Simple3DVertex>();
        private FrameBuffer _frameBuffer = new FrameBuffer();

        protected override void LoadPrograms()
        {
            _blurProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, Resources.render_2D_vert),
                new Shader(ShaderType.FragmentShader, Resources.myBlur_frag)
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

            _vertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayHandle);
            _vertexBuffer.AddVertices(new[]
            {
                new Simple3DVertex(1.0f, 1.0f, 0.0f),
                new Simple3DVertex(-1.0f, 1.0f, 0.0f),
                new Simple3DVertex(-1.0f, -1.0f, 0.0f),
                new Simple3DVertex(1.0f, -1.0f, 0.0f)
            });
            _vertexBuffer.Bind();
            _vertexBuffer.Buffer();

            var attribute = new VertexAttribute("vPosition", 3, VertexAttribPointerType.Float, UnitConversions.SizeOf<Vector3>(), 0);
            attribute.Set(_blurProgram.GetAttributeLocation("vPosition"));

            GL.BindVertexArray(0);
            _vertexBuffer.Unbind();
        }

        public override void ResizeTextures(Resolution resolution)
        {
            FinalTexture.Resize(resolution.Width, resolution.Height, 0);
            FinalTexture.Bind();
            FinalTexture.ReserveMemory();
        }

        public void Render(Texture scene, Texture velocity, float fps)
        {
            _frameBuffer.BindAndDraw();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _blurProgram.Use();

            _blurProgram.BindTexture(scene, "sceneTexture", 0);
            _blurProgram.BindTexture(velocity, "velocityTexture", 1);

            GL.BindVertexArray(_vertexArrayHandle);
            _vertexBuffer.Bind();

            _vertexBuffer.DrawQuads();

            GL.BindVertexArray(0);
            _vertexBuffer.Unbind();
        }
    }
}
