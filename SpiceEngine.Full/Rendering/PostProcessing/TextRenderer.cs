using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngine.Full.Properties;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Batches;
using SpiceEngineCore.Rendering.Textures;
using StarchUICore;
using SweetGraphicsCore.Buffers;
using SweetGraphicsCore.Helpers;
using SweetGraphicsCore.Renderers;
using SweetGraphicsCore.Rendering.Batches;
using SweetGraphicsCore.Rendering.Textures;
using SweetGraphicsCore.Shaders;
using SweetGraphicsCore.Vertices;
using System.Collections.Generic;
using System.IO;

namespace SpiceEngine.Rendering.PostProcessing
{
    public class TextRenderer : Renderer
    {
        public static string FONT_PATH = Directory.GetCurrentDirectory() + @"\..\..\.." + @"\SampleGameProject\Resources\Fonts\Roboto-Regular.ttf";

        private ShaderProgram _textProgram;

        private VertexBuffer<TextureVertex2D> _vertexBuffer;
        private VertexArray<TextureVertex2D> _vertexArray;
        private FrameBuffer _frameBuffer;

        public Texture FinalTexture { get; protected set; }

        protected override void LoadPrograms(IRenderContext renderContext)
        {
            _textProgram = ShaderHelper.LoadProgram(renderContext,
                new[] { ShaderType.VertexShader, ShaderType.FragmentShader },
                new[] { Resources.text_vert, Resources.text_frag });
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

            /*var bitmapPath = Path.GetDirectoryName(FONT_PATH) + "\\" + Path.GetFileNameWithoutExtension(FONT_PATH) + ".png";
            SaveFontBitmap(FONT_PATH, bitmapPath, 14);
            FontTexture = TextureHelper.LoadFromBitmap(bitmapPath, false, false);*/
        }

        protected override void LoadBuffers(IRenderContext renderContext)
        {
            _vertexBuffer = new VertexBuffer<TextureVertex2D>(renderContext);
            _vertexArray = new VertexArray<TextureVertex2D>(renderContext);
            _frameBuffer = new FrameBuffer(renderContext);

            _frameBuffer.Add(FramebufferAttachment.ColorAttachment0, FinalTexture);
            _frameBuffer.Load();

            _vertexBuffer.Load();
            _vertexBuffer.Bind();
            _vertexArray.Load();
            _vertexBuffer.Unbind();
        }

        protected override void Resize(Resolution resolution) => FinalTexture.Resize(resolution.Width, resolution.Height, 0);

        public void Render(IBatcher batcher, IUIProvider uiProvider)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.Disable(EnableCap.DepthTest);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

            batcher.CreateBatchAction()
                .SetShader(_textProgram)
                .SetUniform("halfResolution", new Vector2(FinalTexture.Width / 2, FinalTexture.Height / 2))
                .SetRenderType(RenderTypes.TransparentText)
                .SetEntityIDOrder(uiProvider.GetDrawOrder())
                .Render()
                .Execute();

            GL.Disable(EnableCap.Blend);
        }

        /// <summary>
        /// Renders text to the screen.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="fontScale"></param>
        /// <returns>Height of the rendered text.</returns>
        public int RenderText(IFont font, string text, int x, int y, float fontScale = 1.0f, bool wordWrap = false)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.Disable(EnableCap.DepthTest);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

            var uStep = (float)font.GlyphWidth / font.Texture.Width;
            var vStep = (float)font.GlyphHeight / font.Texture.Height;

            var width = (int)(font.GlyphWidth * fontScale);
            var height = (int)(font.GlyphHeight * fontScale);

            var initialX = x;

            _vertexBuffer.Clear();
            for (var i = 0; i < text.Length; i++)
            {
                char character = text[i];

                var u = (character % font.GlyphsPerLine) * uStep;
                var v = (character / font.GlyphsPerLine) * vStep;

                if (wordWrap && x + width > FinalTexture.Width)
                {
                    x = initialX;
                    y += height + font.YSpacing;
                }

                // SHADER ORDER   - TL, BL, TR, BR
                // Position Order - TR, TL, BL, BR
                // Texture Order  - BR, BL, TL, TR
                var ptr = new Vector2(x + width, y + height);
                var ptl = new Vector2(x, y + height);
                var pbl = new Vector2(x, y);
                var pbr = new Vector2(x + width, y);

                var tbr = new Vector2(u + uStep, v);
                var tbl = new Vector2(u, v);
                var ttl = new Vector2(u, v + vStep);
                var ttr = new Vector2(u + uStep, v + vStep);

                _vertexBuffer.AddVertices(new List<TextureVertex2D>()
                {
                    new TextureVertex2D(ptr, ttr),
                    new TextureVertex2D(ptl, ttl),
                    new TextureVertex2D(pbr, tbr),
                    new TextureVertex2D(pbl, tbl)
                });

                /*_vertexBuffer.AddVertex(new TextureVertex2D(new Vector2(x + width, y + height), new Vector2(u + uStep, v)));
                _vertexBuffer.AddVertex(new TextureVertex2D(new Vector2(x, y + height), new Vector2(u, v)));
                _vertexBuffer.AddVertex(new TextureVertex2D(new Vector2(x, y), new Vector2(u, v + vStep)));
                _vertexBuffer.AddVertex(new TextureVertex2D(new Vector2(x + width, y), new Vector2(u + uStep, v + vStep)));*/

                x += font.XSpacing + 20;
            }

            _textProgram.Bind();
            _textProgram.BindTexture(font.Texture, "textureSampler", 0);
            _textProgram.SetUniform("halfResolution", new Vector2(FinalTexture.Width / 2, FinalTexture.Height / 2));

            _vertexArray.Bind();
            _vertexBuffer.Bind();
            _vertexBuffer.Buffer();

            _vertexBuffer.DrawTriangleStrips();

            _vertexArray.Unbind();
            _vertexBuffer.Unbind();

            GL.Disable(EnableCap.Blend);

            return y;
        }
    }
}
