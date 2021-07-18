using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Utilities;
using SweetGraphicsCore.Buffers;
using SweetGraphicsCore.Helpers;
using SweetGraphics.Properties;
using SweetGraphicsCore.Rendering.Textures;
using SweetGraphicsCore.Shaders;
using SweetGraphicsCore.Vertices;
using System.Collections.Generic;

namespace SweetGraphicsCore.Renderers.Processing
{
    /// <summary>
    /// Renders all game entities to a texture, with their colors reflecting their given ID's
    /// </summary>
    public class FXAARenderer : Renderer
    {
        public Texture FinalTexture { get; protected set; }

        private ShaderProgram _fxaaProgram;

        private FrameBuffer _frameBuffer;
        private int _vertexArrayHandle;
        private VertexBuffer<Simple3DVertex> _vertexBuffer;

        protected override void LoadPrograms(IRenderContext renderContext)
        {
            _fxaaProgram = ShaderHelper.LoadProgram(renderContext,
                new[] { ShaderType.VertexShader, ShaderType.FragmentShader },
                new[] { Resources.fxaa_vert, Resources.fxaa_frag });
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

        protected override void LoadBuffers(IRenderContext renderContext)
        {
            _frameBuffer = new FrameBuffer(renderContext);
            _vertexArrayHandle = GL.GenVertexArray(); // TODO - This vertex array is never de-allocated, which is terrible...
            _vertexBuffer = new VertexBuffer<Simple3DVertex>(renderContext);

            _frameBuffer.Add(FramebufferAttachment.ColorAttachment0, FinalTexture);
            _frameBuffer.Load();

            GL.BindVertexArray(_vertexArrayHandle);
            _vertexBuffer.Load();
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
            attribute.Set(_fxaaProgram.GetAttributeLocation("vPosition"));

            GL.BindVertexArray(0);
            _vertexBuffer.Unbind();
        }

        protected override void Resize(Resolution resolution) => FinalTexture.Resize(resolution.Width, resolution.Height, 0);

        public void BindForWriting()
        {
            _frameBuffer.BindAndDraw(DrawBufferMode.ColorAttachment0);
            GL.Disable(EnableCap.DepthTest);
            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public void Render(Texture texture)
        {
            BindForWriting();
            GL.Viewport(0, 0, texture.Width, texture.Height);

            _fxaaProgram.Bind();
            _fxaaProgram.BindTexture(texture, "filterTexture", 0);
            _fxaaProgram.SetUniform("texelStep", Vector2.One);
            _fxaaProgram.SetUniform("maxThreshold", 100.0f);
            _fxaaProgram.SetUniform("mulReduction", 1.0f / 8.0f);
            _fxaaProgram.SetUniform("minReduction", 1.0f / 128.0f);
            _fxaaProgram.SetUniform("maxSpan", 8.0f);

            GL.BindVertexArray(_vertexArrayHandle);
            _vertexBuffer.Bind();

            _vertexBuffer.DrawTriangleStrips();

            GL.BindVertexArray(0);
            _vertexBuffer.Unbind();
        }

        private IEnumerable<Actor> PerformFrustumCulling(IEnumerable<Actor> actors)
        {
            // Don't render meshes that are not in the camera's view

            // Using the position of the actor, determine if we should render the mesh
            // We will also need a bounding sphere or bounding box from the mesh to determine this
            foreach (var actor in actors)
            {
                Vector3 position = actor.Position;
            }

            return actors;
        }

        private IEnumerable<Actor> PerformOcclusionCulling(IEnumerable<Actor> actors)
        {
            // Don't render meshes that are obscured by closer meshes
            foreach (var actor in actors)
            {
                Vector3 position = actor.Position;
            }

            return actors;
        }
    }
}
