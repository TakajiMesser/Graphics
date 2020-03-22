using OpenTK.Graphics.OpenGL;
using SpiceEngineCore.Rendering.Textures;
using StarchUICore.Text;
using SweetGraphicsCore.Rendering.Textures;
using System;
//using System.Drawing;
//using System.Drawing.Text;
using System.Linq;

namespace SpiceEngine.Rendering
{
    public class Font : IFont
    {
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

        public int GlyphsPerLine { get; set; } = FontManager.GLYPHS_PER_LINE;
        public int GlyphLineCount { get; set; } = FontManager.GLYPH_LINE_COUNT;
        public int GlyphWidth { get; set; } = FontManager.GLYPH_WIDTH;
        public int GlyphHeight { get; set; } = FontManager.GLYPH_HEIGHT;

        public int XSpacing { get; set; } = FontManager.X_SPACING;
        public int YSpacing { get; set; } = FontManager.Y_SPACING;

        public ITexture Texture { get; private set; }

        public void LoadTexture()
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

                        Texture = LoadFromBitmap(bitmap, false, false);
                    }
                }
            }
        }

        private Texture LoadFromBitmap(System.Drawing.Bitmap bitmap, bool enableMipMap, bool enableAnisotrophy, TextureMinFilter minFilter = TextureMinFilter.Linear, TextureMagFilter magFilter = TextureMagFilter.Linear)
        {
            var data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var texture = new Texture(bitmap.Width, bitmap.Height, 1)
            {
                Target = TextureTarget.Texture2D,
                MinFilter = minFilter,
                MagFilter = magFilter,
                WrapMode = TextureWrapMode.Repeat,
                EnableMipMap = enableMipMap,
                EnableAnisotropy = enableAnisotrophy
            };

            switch (data.PixelFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
                    texture.PixelInternalFormat = PixelInternalFormat.Rgb8;
                    texture.PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.ColorIndex;
                    texture.PixelType = PixelType.Bitmap;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    texture.PixelInternalFormat = PixelInternalFormat.Rgb8;
                    texture.PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Bgr;
                    texture.PixelType = PixelType.UnsignedByte;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                    texture.PixelInternalFormat = PixelInternalFormat.Rgba;
                    texture.PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Bgra;
                    texture.PixelType = PixelType.UnsignedByte;
                    break;
            }

            texture.Bind();
            texture.Load(data.Scan0);

            bitmap.UnlockBits(data);
            bitmap.Dispose();

            return texture;
        }
    }
}
