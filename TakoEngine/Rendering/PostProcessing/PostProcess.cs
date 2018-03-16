using OpenTK;
using OpenTK.Graphics.OpenGL;
using TakoEngine.Outputs;
using TakoEngine.Rendering.Buffers;
using TakoEngine.Rendering.Shaders;
using TakoEngine.Rendering.Textures;
using TakoEngine.Rendering.Vertices;
using TakoEngine.Utilities;

namespace TakoEngine.Rendering.PostProcessing
{
    public abstract class PostProcess
    {
        public string Name { get; private set; }
        public bool Enabled { get; set; } = true;
        public Resolution Resolution { get; set; }

        public Texture FinalTexture { get; protected set; }

        //protected ShaderProgram _program;
        private int _vertexArrayHandle;
        private VertexBuffer<Vector3> _vertexBuffer = new VertexBuffer<Vector3>();
        protected FrameBuffer _frameBuffer = new FrameBuffer();

        public PostProcess(string name, Resolution resolution)
        {
            Name = name;
            Resolution = resolution;
        }

        public void Load()
        {
            LoadProgram();
            LoadBuffers();
        }

        protected abstract void LoadProgram();

        protected void LoadQuad(ShaderProgram program)
        {
            _vertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayHandle);
            _vertexBuffer.AddVertices(new[]
            {
                new Vector3(1.0f, 1.0f, 0.0f),
                new Vector3(-1.0f, 1.0f, 0.0f),
                new Vector3(-1.0f, -1.0f, 0.0f),
                new Vector3(1.0f, -1.0f, 0.0f)
            });
            _vertexBuffer.Bind();
            _vertexBuffer.Buffer();

            var attribute = new VertexAttribute("vPosition", 3, VertexAttribPointerType.Float, UnitConversions.SizeOf<Vector3>(), 0);
            attribute.Set(program.GetAttributeLocation("vPosition"));

            GL.BindVertexArray(0);
            _vertexBuffer.Unbind();
        }

        protected void LoadFinalTexture()
        {
            FinalTexture = new Texture(Resolution.Width, Resolution.Height, 0)
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

        protected abstract void LoadBuffers();

        public virtual void ResizeTextures()
        {
            FinalTexture.Resize(Resolution.Width, Resolution.Height, 0);
            FinalTexture.Bind();
            FinalTexture.ReserveMemory();
        }

        protected void RenderQuad()
        {
            GL.BindVertexArray(_vertexArrayHandle);
            _vertexBuffer.Bind();

            _vertexBuffer.DrawQuads();

            GL.BindVertexArray(0);
            _vertexBuffer.Unbind();
        }

        //public abstract void Render();
        //public abstract void Render();
    }
}
