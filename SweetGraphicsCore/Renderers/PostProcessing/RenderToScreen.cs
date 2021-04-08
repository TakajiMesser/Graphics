using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering;
using SweetGraphicsCore.Buffers;
using SweetGraphicsCore.Helpers;
using SweetGraphicsCore.Properties;
using SweetGraphicsCore.Rendering.Textures;
using SweetGraphicsCore.Shaders;
using SweetGraphicsCore.Vertices;
using System;

namespace SweetGraphicsCore.Renderers.PostProcessing
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

        private VertexArray<Simple2DVertex> _vertexArray;
        private VertexBuffer<Simple2DVertex> _vertexBuffer;

        protected override void LoadPrograms(IRenderContext renderContext)
        {
            _render1DProgram = ShaderHelper.LoadProgram(renderContext,
                new[] { ShaderType.FragmentShader },
                new[] { Resources.render_1D_frag });

            _render2DProgram = ShaderHelper.LoadProgram(renderContext,
                new[] { ShaderType.VertexShader, ShaderType.FragmentShader },
                new[] { Resources.render_2D_vert, Resources.render_2D_frag });

            _render2DArrayProgram = ShaderHelper.LoadProgram(renderContext,
                new[] { ShaderType.FragmentShader },
                new[] { Resources.render_2D_array_frag });

            _render3DProgram = ShaderHelper.LoadProgram(renderContext,
                new[] { ShaderType.FragmentShader },
                new[] { Resources.render_3D_frag });

            _renderCubeProgram = ShaderHelper.LoadProgram(renderContext,
                new[] { ShaderType.VertexShader, ShaderType.FragmentShader },
                new[] { Resources.render_cube_vert, Resources.render_cube_frag });

            _renderCubeArrayProgram = ShaderHelper.LoadProgram(renderContext,
                new[] { ShaderType.VertexShader, ShaderType.FragmentShader },
                new[] { Resources.render_cube_vert, Resources.render_cube_array_frag });
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
            _vertexBuffer = new VertexBuffer<Simple2DVertex>(renderContext);
            _vertexArray = new VertexArray<Simple2DVertex>(renderContext);

            _vertexBuffer.Load();
            _vertexBuffer.Bind();
            _vertexArray.Load();
            _vertexBuffer.Unbind();

            _vertexBuffer.AddVertex(new Simple2DVertex(new Vector2(1.0f, 1.0f)));
            _vertexBuffer.AddVertex(new Simple2DVertex(new Vector2(-1.0f, 1.0f)));
            _vertexBuffer.AddVertex(new Simple2DVertex(new Vector2(1.0f, -1.0f)));
            _vertexBuffer.AddVertex(new Simple2DVertex(new Vector2(-1.0f, -1.0f)));
        }

        protected override void Resize(Resolution resolution) => FinalTexture.Resize(resolution.Width, resolution.Height, 0);

        public void Render(Texture texture, int channel = -1)
        {
            //GL.Disable(EnableCap.DepthTest);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

            //GL.ClearColor(Color4.Blue);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Viewport(0, 0, FinalTexture.Width, FinalTexture.Height);

            switch (texture.Target)
            {
                case TextureTarget.Texture1d:
                    _render1DProgram.Bind();
                    _render1DProgram.BindTexture(texture, "textureSampler", 0);
                    break;
                case TextureTarget.Texture2d:
                    _render2DProgram.Bind();
                    _render2DProgram.BindTexture(texture, "textureSampler", 0);

                    _render2DProgram.SetUniform("gamma", Gamma);
                    _render2DProgram.SetUniform("channel", channel);
                    break;
                case TextureTarget.Texture3d:
                    break;
                case TextureTarget.Texture2dArray:
                    break;
                case TextureTarget.TextureCubeMapArray:
                    break;
                default:
                    throw new NotImplementedException("Cannot render texture target type " + texture.Target);
            }

            _vertexArray.Bind();
            _vertexBuffer.Bind();

            _vertexBuffer.Buffer();
            _vertexBuffer.DrawTriangleStrips();

            _vertexArray.Unbind();
            _vertexBuffer.Unbind();
        }
    }
}
