using OpenTK;
using OpenTK.Graphics.OpenGL;
using SpiceEngine.Properties;
using SpiceEngine.Rendering.Buffers;
using SpiceEngine.Rendering.Textures;
using SpiceEngineCore.Outputs;
using SpiceEngineCore.Rendering.Processing;
using SpiceEngineCore.Rendering.Shaders;
using SpiceEngineCore.Rendering.Vertices;
using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;

namespace SpiceEngine.Rendering.PostProcessing
{
    public class TextRenderer : Renderer
    {
        public const int GLYPHS_PER_LINE = 16;
        public const int GLYPH_LINE_COUNT = 16;
        public const int GLYPH_WIDTH = 24;
        public const int GLYPH_HEIGHT = 32;
        public const int X_SPACING = 4;
        public const int Y_SPACING = 5;

        public static string FONT_PATH = Directory.GetCurrentDirectory() + @"\..\..\.." + @"\SampleGameProject\Resources\Fonts\Roboto-Regular.ttf";

        public Texture FontTexture { get; protected set; }
        public Texture FinalTexture { get; protected set; }

        private ShaderProgram _textProgram;

        private VertexBuffer<TextureVertex2D> _vertexBuffer = new VertexBuffer<TextureVertex2D>();
        private VertexArray<TextureVertex2D> _vertexArray = new VertexArray<TextureVertex2D>();
        private FrameBuffer _frameBuffer = new FrameBuffer();

        protected override void LoadPrograms()
        {
            _textProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, Resources.text_vert),
                new Shader(ShaderType.FragmentShader, Resources.text_frag)
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

            var bitmapPath = Path.GetDirectoryName(FONT_PATH) + "\\" + Path.GetFileNameWithoutExtension(FONT_PATH) + ".png";
            SaveFontBitmap(FONT_PATH, bitmapPath, 14);
            FontTexture = Texture.LoadFromBitmap(bitmapPath, false, false);
        }

        protected override void LoadBuffers()
        {
            _frameBuffer.Add(FramebufferAttachment.ColorAttachment0, FinalTexture);
            _frameBuffer.Bind(FramebufferTarget.Framebuffer);
            _frameBuffer.AttachAttachments();
            _frameBuffer.Unbind(FramebufferTarget.Framebuffer);

            _vertexBuffer.Bind();
            _vertexArray.Load();
            _vertexBuffer.Unbind();
        }

        public override void ResizeTextures(Resolution resolution)
        {
            FinalTexture.Resize(resolution.Width, resolution.Height, 0);
            FinalTexture.Bind();
            FinalTexture.ReserveMemory();
        }

        public void SaveFontBitmap(string fontPath, string bitmapPath, int fontSize)
        {
            var fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile(fontPath);
            var font = new Font(fontCollection.Families.First(), fontSize);

            int bitmapWidth = GLYPHS_PER_LINE * GLYPH_WIDTH;
            int bitmapHeight = GLYPH_LINE_COUNT * GLYPH_HEIGHT;
            int maxDimension = Math.Max(bitmapWidth, bitmapHeight);

            using (var bitmap = new Bitmap(maxDimension, maxDimension, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                    for (var i = 0; i < GLYPH_LINE_COUNT; i++)
                    {
                        for (var j = 0; j < GLYPHS_PER_LINE; j++)
                        {
                            var character = (char)(i * GLYPHS_PER_LINE + j);
                            graphics.DrawString(character.ToString(), font, Brushes.White, j * GLYPH_WIDTH, i * GLYPH_HEIGHT);
                        }
                    }
                }

                bitmap.Save(bitmapPath);
            }
        }

        /// <summary>
        /// Renders text to the screen.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="fontScale"></param>
        /// <returns>Height of the rendered text.</returns>
        public int RenderText(string text, int x, int y, float fontScale, bool wordWrap = false)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.Disable(EnableCap.DepthTest);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

            var uStep = (float)GLYPH_WIDTH / FontTexture.Width;
            var vStep = (float)GLYPH_HEIGHT / FontTexture.Height;

            var width = (int)(GLYPH_WIDTH * fontScale);
            var height = (int)(GLYPH_HEIGHT * fontScale);

            var initialX = x;

            _vertexBuffer.Clear();
            for (var i = 0; i < text.Length; i++)
            {
                char character = text[i];

                var u = (character % GLYPHS_PER_LINE) * uStep;
                var v = (character / GLYPHS_PER_LINE) * vStep;

                if (wordWrap && x + width > FinalTexture.Width)
                {
                    x = initialX;
                    y += height + Y_SPACING;
                }

                _vertexBuffer.AddVertex(new TextureVertex2D(new Vector2(x + width, y + height), new Vector2(u + uStep, v)));
                _vertexBuffer.AddVertex(new TextureVertex2D(new Vector2(x, y + height), new Vector2(u, v)));
                _vertexBuffer.AddVertex(new TextureVertex2D(new Vector2(x, y), new Vector2(u, v + vStep)));
                _vertexBuffer.AddVertex(new TextureVertex2D(new Vector2(x + width, y), new Vector2(u + uStep, v + vStep)));

                x += X_SPACING + 20;
            }

            _textProgram.Use();
            _textProgram.BindTexture(FontTexture, "textureSampler", 0);
            _textProgram.SetUniform("halfResolution", new Vector2(FinalTexture.Width / 2, FinalTexture.Height / 2));

            _vertexArray.Bind();
            _vertexBuffer.Bind();
            _vertexBuffer.Buffer();

            _vertexBuffer.DrawQuads();

            _vertexArray.Unbind();
            _vertexBuffer.Unbind();

            GL.Disable(EnableCap.Blend);

            return y;
        }
    }
}
