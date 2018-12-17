using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SpiceEngine.Properties;
using System.Collections.Generic;
using System.IO;
using SpiceEngine.Entities;
using SpiceEngine.Helpers;
using SpiceEngine.Outputs;
using SpiceEngine.Rendering.Buffers;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;
using SpiceEngine.Utilities;
using SpiceEngine.Entities.Actors;

namespace SpiceEngine.Rendering.Processing
{
    /// <summary>
    /// Renders all game entities to a texture, with their colors reflecting their given ID's
    /// </summary>
    public class FXAARenderer : Renderer
    {
        public Texture FinalTexture { get; protected set; }

        private ShaderProgram _fxaaProgram;

        private FrameBuffer _frameBuffer = new FrameBuffer();
        private int _vertexArrayHandle;
        private VertexBuffer<Simple3DVertex> _vertexBuffer = new VertexBuffer<Simple3DVertex>();

        protected override void LoadPrograms()
        {
            _fxaaProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, Resources.fxaa_vert),
                new Shader(ShaderType.FragmentShader, Resources.fxaa_frag)
            );
        }

        public override void ResizeTextures(Resolution resolution)
        {
            FinalTexture.Resize(resolution.Width, resolution.Height, 0);
            FinalTexture.Bind();
            FinalTexture.ReserveMemory();
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
            attribute.Set(_fxaaProgram.GetAttributeLocation("vPosition"));

            GL.BindVertexArray(0);
            _vertexBuffer.Unbind();
        }

        public void BindForWriting()
        {
            _frameBuffer.BindAndDraw(DrawBuffersEnum.ColorAttachment0);
            GL.Disable(EnableCap.DepthTest);
            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public void Render(Texture texture)
        {
            BindForWriting();
            GL.Viewport(0, 0, texture.Width, texture.Height);

            _fxaaProgram.Use();
            _fxaaProgram.BindTexture(texture, "filterTexture", 0);
            _fxaaProgram.SetUniform("texelStep", Vector2.One);
            _fxaaProgram.SetUniform("maxThreshold", 100.0f);
            _fxaaProgram.SetUniform("mulReduction", 1.0f / 8.0f);
            _fxaaProgram.SetUniform("minReduction", 1.0f / 128.0f);
            _fxaaProgram.SetUniform("maxSpan", 8.0f);

            GL.BindVertexArray(_vertexArrayHandle);
            _vertexBuffer.Bind();

            _vertexBuffer.DrawQuads();

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
