using SpiceEngineCore.Rendering.Textures;
using System;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;

namespace StarchUICore.Text
{
    public class Font : IFont
    {
        public Font(string filePath, int size)
        {
            using (var fontCollection = new PrivateFontCollection())
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

        public ITexture Texture { get; internal set; }

        public Bitmap ToBitmap()
        {
            var bitmapWidth = GlyphsPerLine * GlyphWidth;
            var bitmapHeight = GlyphLineCount * GlyphHeight;
            var maxDimension = Math.Max(bitmapWidth, bitmapHeight);

            using (var fontCollection = new PrivateFontCollection())
            {
                fontCollection.AddFontFile(Path);

                using (var font = new System.Drawing.Font(fontCollection.Families.First(), Size))
                {
                    using (var bitmap = new Bitmap(maxDimension, maxDimension, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                    {
                        using (var graphics = Graphics.FromImage(bitmap))
                        {
                            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                            for (var i = 0; i < GlyphLineCount; i++)
                            {
                                for (var j = 0; j < GlyphsPerLine; j++)
                                {
                                    var character = (char)(i * GlyphsPerLine + j);
                                    graphics.DrawString(character.ToString(), font, Brushes.White, j * GlyphWidth, i * GlyphHeight);
                                }
                            }
                        }

                        return bitmap;
                    }
                }
            }
        }
    }
}
