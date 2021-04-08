using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Utilities;
using SweetGraphicsCore.Buffers;
using SweetGraphicsCore.Renderers;
using SweetGraphicsCore.Rendering.Textures;
using SweetGraphicsCore.Shaders;
using SweetGraphicsCore.Vertices;

namespace SweetGraphicsCore.Rendering.Processing.Post
{
    public abstract class PostProcess : Renderer
    {
        private int _vertexArrayHandle;
        private VertexBuffer<Simple3DVertex> _vertexBuffer;

        public PostProcess(string name) => Name = name;

        public string Name { get; private set; }
        public bool Enabled { get; set; } = true;
        public Texture FinalTexture { get; protected set; }

        protected void LoadBuffers(ShaderProgram program)
        {
            _vertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayHandle);
            _vertexBuffer.AddVertices(new[]
            {
                new Simple3DVertex(1.0f, 1.0f, 0.0f),
                new Simple3DVertex(-1.0f, 1.0f, 0.0f),
                new Simple3DVertex(1.0f, -1.0f, 0.0f),
                new Simple3DVertex(-1.0f, -1.0f, 0.0f)
            });

            _vertexBuffer.Load();
            _vertexBuffer.Bind();
            _vertexBuffer.Buffer();

            var attribute = new VertexAttribute("vPosition", 3, VertexAttribPointerType.Float, UnitConversions.SizeOf<Vector3>(), 0);
            attribute.Set(program.GetAttributeLocation("vPosition"));

            GL.BindVertexArray(0);
            _vertexBuffer.Unbind();
        }

        protected override void LoadTextures(IRenderContext renderContext, Resolution resolution)
        {
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
        }

        protected override void Resize(Resolution resolution) => FinalTexture.Resize(resolution.Width, resolution.Height, 0);

        protected void RenderQuad()
        {
            GL.BindVertexArray(_vertexArrayHandle);
            _vertexBuffer.Bind();

            _vertexBuffer.DrawTriangleStrips();

            GL.BindVertexArray(0);
            _vertexBuffer.Unbind();
        }

        //public abstract void Render();
        //public abstract void Render();
    }
}
