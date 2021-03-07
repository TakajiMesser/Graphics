using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Utilities;
using SweetGraphicsCore.Buffers;
using SweetGraphicsCore.Helpers;
using SweetGraphicsCore.Properties;
using SweetGraphicsCore.Rendering.Textures;
using SweetGraphicsCore.Shaders;
using SweetGraphicsCore.Vertices;

namespace SweetGraphicsCore.Renderers.PostProcessing
{
    public class Blur : Renderer
    {
        private ShaderProgram _blurProgram;

        private int _vertexArrayHandle;
        private VertexBuffer<Simple3DVertex> _vertexBuffer;
        private FrameBuffer _frameBuffer;

        public Texture FinalTexture { get; protected set; }

        protected override void LoadPrograms(IRenderContextProvider contextProvider)
        {
            _blurProgram = ShaderHelper.LoadProgram(contextProvider,
                new[] { ShaderType.VertexShader, ShaderType.FragmentShader },
                new[] { Resources.render_2D_vert, Resources.myBlur_frag });
        }

        protected override void LoadTextures(IRenderContextProvider contextProvider, Resolution resolution)
        {
            FinalTexture = new Texture(contextProvider, resolution.Width, resolution.Height, 0)
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

        protected override void LoadBuffers(IRenderContextProvider contextProvider)
        {
            _frameBuffer = new FrameBuffer(contextProvider);
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment0, FinalTexture);
            _frameBuffer.Load();

            _vertexBuffer = new VertexBuffer<Simple3DVertex>(contextProvider);
            _vertexBuffer.Load();

            _vertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayHandle);
            _vertexBuffer.AddVertices(new[]
            {
                new Simple3DVertex(1.0f, 1.0f, 0.0f),
                new Simple3DVertex(-1.0f, 1.0f, 0.0f),
                new Simple3DVertex(1.0f, -1.0f, 0.0f),
                new Simple3DVertex(-1.0f, -1.0f, 0.0f)
            });
            _vertexBuffer.Bind();
            _vertexBuffer.Buffer();

            var attribute = new VertexAttribute("vPosition", 3, VertexAttribPointerType.Float, UnitConversions.SizeOf<Vector3>(), 0);
            attribute.Set(_blurProgram.GetAttributeLocation("vPosition"));

            GL.BindVertexArray(0);
            _vertexBuffer.Unbind();
        }

        protected override void Resize(Resolution resolution) => FinalTexture.Resize(resolution.Width, resolution.Height, 0);

        public void Render(Texture scene, Texture velocity, float fps)
        {
            _frameBuffer.BindAndDraw();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _blurProgram.Bind();

            _blurProgram.BindTexture(scene, "sceneTexture", 0);
            _blurProgram.BindTexture(velocity, "velocityTexture", 1);

            GL.BindVertexArray(_vertexArrayHandle);
            _vertexBuffer.Bind();

            _vertexBuffer.DrawTriangleStrips();

            GL.BindVertexArray(0);
            _vertexBuffer.Unbind();
        }
    }
}
