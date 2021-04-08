using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Textures;
using SweetGraphicsCore.Rendering.Textures;
using System;
using System.Linq;

namespace SweetGraphicsCore.Rendering
{
    public class Font : IFont
    {
        // For now, we are treating all fonts as monospaced
        public const int DEFAULT_GLYPHS_PER_LINE = 16;
        public const int DEFAULT_GLYPH_LINE_COUNT = 16;
        public const int DEFAULT_GLYPH_WIDTH = 24;
        public const int DEFAULT_GLYPH_HEIGHT = 32;
        public const int DEFAULT_X_SPACING = 4;
        public const int DEFAULT_Y_SPACING = 5;

        public Font(string filePath, int size)
        {
            using (var fontCollection = new System.Drawing.Text.PrivateFontCollection())
            {
                fontCollection.AddFontFile(filePath);

                using (var font = new System.Drawing.Font(fontCollection.Families.First(), size))
                {
                    Name = font.Name;
                }
            }

            Path = filePath;
            Size = size;
        }

        public string Name { get; }
        public string Path { get; }
        public int Size { get; }

        public int GlyphsPerLine { get; set; } = DEFAULT_GLYPHS_PER_LINE;
        public int GlyphLineCount { get; set; } = DEFAULT_GLYPH_LINE_COUNT;
        public int GlyphWidth { get; set; } = DEFAULT_GLYPH_WIDTH;
        public int GlyphHeight { get; set; } = DEFAULT_GLYPH_HEIGHT;

        public int XSpacing { get; set; } = DEFAULT_X_SPACING;
        public int YSpacing { get; set; } = DEFAULT_Y_SPACING;

        public ITexture Texture { get; private set; }

        public void LoadTexture(IRenderContext renderContext)
        {
            var bitmapWidth = GlyphsPerLine * GlyphWidth;
            var bitmapHeight = GlyphLineCount * GlyphHeight;
            var maxDimension = Math.Max(bitmapWidth, bitmapHeight);

            using (var fontCollection = new System.Drawing.Text.PrivateFontCollection())
            {
                fontCollection.AddFontFile(Path);

                using (var font = new System.Drawing.Font(fontCollection.Families.First(), Size))
                {
                    using (var bitmap = new System.Drawing.Bitmap(maxDimension, maxDimension, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                    {
                        using (var graphics = System.Drawing.Graphics.FromImage(bitmap))
                        {
                            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                            for (var i = 0; i < GlyphLineCount; i++)
                            {
                                for (var j = 0; j < GlyphsPerLine; j++)
                                {
                                    var character = (char)(i * GlyphsPerLine + j);
                                    graphics.DrawString(character.ToString(), font, System.Drawing.Brushes.White, j * GlyphWidth, i * GlyphHeight);
                                }
                            }
                        }

                        Texture = LoadFromBitmap(renderContext, bitmap, false, false);
                    }
                }
            }
        }

        private Texture LoadFromBitmap(IRenderContext renderContext, System.Drawing.Bitmap bitmap, bool enableMipMap, bool enableAnisotrophy, TextureMinFilter minFilter = TextureMinFilter.Linear, TextureMagFilter magFilter = TextureMagFilter.Linear)
        {
            var data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var texture = new Texture(renderContext, bitmap.Width, bitmap.Height, 0)
            {
                Target = TextureTarget.Texture2d,
                MinFilter = minFilter,
                MagFilter = magFilter,
                WrapMode = TextureWrapMode.Repeat,
                EnableMipMap = enableMipMap,
                EnableAnisotropy = enableAnisotrophy
            };

            switch (data.PixelFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
                    texture.InternalFormat = InternalFormat.Rgb8;
                    texture.PixelFormat = PixelFormat.ColorIndex;
                    texture.PixelType = PixelType.Bitmap;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    texture.InternalFormat = InternalFormat.Rgb8;
                    texture.PixelFormat = PixelFormat.Bgr;
                    texture.PixelType = PixelType.UnsignedByte;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                    texture.InternalFormat = InternalFormat.Rgba;
                    texture.PixelFormat = PixelFormat.Bgra;
                    texture.PixelType = PixelType.UnsignedByte;
                    break;
            }

            texture.Load(data.Scan0);

            bitmap.UnlockBits(data);
            bitmap.Dispose();

            return texture;
        }
    }
}
