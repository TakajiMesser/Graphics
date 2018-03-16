using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.IO;
using TakoEngine.Helpers;
using TakoEngine.Outputs;
using TakoEngine.Rendering.Buffers;
using TakoEngine.Rendering.Processing;
using TakoEngine.Rendering.Shaders;
using TakoEngine.Rendering.Textures;
using TakoEngine.Rendering.Vertices;
using TakoEngine.Utilities;

namespace TakoEngine.Rendering.PostProcessing
{
    public class RenderToScreen : Renderer
    {
        public Texture FinalTexture { get; protected set; }
        public float Gamma { get; set; } = 2.2f;

        private ShaderProgram _render1DProgram;
        private ShaderProgram _render2DProgram;
        private ShaderProgram _render2DArrayProgram;
        private ShaderProgram _render3DProgram;
        private ShaderProgram _renderCubeProgram;
        private ShaderProgram _renderCubeArrayProgram;

        private int _vertexArrayHandle;
        private VertexBuffer<Vector3> _vertexBuffer = new VertexBuffer<Vector3>();
        protected FrameBuffer _frameBuffer = new FrameBuffer();

        protected override void LoadPrograms()
        {
            _render1DProgram = new ShaderProgram(new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.RENDER_1D_PATH)));
            _render2DProgram = new ShaderProgram(new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.RENDER_2D_VERTEX_PATH)), 
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.RENDER_2D_FRAGMENT_PATH)));
            _render2DArrayProgram = new ShaderProgram(new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.RENDER_2D_ARRAY_PATH)));
            _render3DProgram = new ShaderProgram(new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.RENDER_3D_PATH)));
            _renderCubeProgram = new ShaderProgram(new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.RENDER_CUBE_VERTEX_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.RENDER_CUBE_FRAGMENT_PATH)));
            _renderCubeArrayProgram = new ShaderProgram(new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.RENDER_CUBE_VERTEX_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.RENDER_CUBE_ARRAY_PATH)));
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
                new Vector3(1.0f, 1.0f, 0.0f),
                new Vector3(-1.0f, 1.0f, 0.0f),
                new Vector3(-1.0f, -1.0f, 0.0f),
                new Vector3(1.0f, -1.0f, 0.0f)
            });
            _vertexBuffer.Bind();
            _vertexBuffer.Buffer();

            var attribute = new VertexAttribute("vPosition", 3, VertexAttribPointerType.Float, UnitConversions.SizeOf<Vector3>(), 0);
            attribute.Set(_render2DProgram.GetAttributeLocation("vPosition"));

            GL.BindVertexArray(0);
            _vertexBuffer.Unbind();
        }

        public override void ResizeTextures(Resolution resolution)
        {
            FinalTexture.Resize(resolution.Width, resolution.Height, 0);
            FinalTexture.Bind();
            FinalTexture.ReserveMemory();
        }

        public void Render(Texture texture, int channel = -1)
        {
            GL.Disable(EnableCap.DepthTest);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Viewport(0, 0, texture.Width, texture.Height);

            switch (texture.Target)
            {
                case TextureTarget.Texture1D:
                    _render1DProgram.Use();
                    _render1DProgram.BindTexture(texture, "textureSampler", 0);
                    break;
                case TextureTarget.Texture2D:
                    _render2DProgram.Use();
                    _render2DProgram.BindTexture(texture, "textureSampler", 0);

                    _render2DProgram.SetUniform("gamma", Gamma);
                    _render2DProgram.SetUniform("channel", channel);
                    break;
                case TextureTarget.Texture3D:
                    break;
                case TextureTarget.Texture2DArray:
                    break;
                case TextureTarget.TextureCubeMapArray:
                    break;
                default:
                    throw new NotImplementedException("Cannot render texture target type " + texture.Target);
            }

            GL.BindVertexArray(_vertexArrayHandle);
            _vertexBuffer.Bind();

            _vertexBuffer.DrawQuads();

            GL.BindVertexArray(0);
            _vertexBuffer.Unbind();
        }
    }
}
