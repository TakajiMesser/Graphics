using SpiceEngineCore.Rendering.Textures;
//using System.Drawing;

namespace StarchUICore
{
    public interface IFont
    {
        string Name { get; }
        string Path { get; }
        int Size { get; }

        int GlyphsPerLine { get; }
        int GlyphLineCount { get; }
        int GlyphWidth { get; }
        int GlyphHeight { get; }

        int XSpacing { get; }
        int YSpacing { get; }

        ITexture Texture { get; }

        void LoadTexture();

        //System.Drawing.Bitmap ToBitmap();
    }
}
