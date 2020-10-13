using SpiceEngineCore.Rendering.Textures;

namespace StarchUICore.Text
{
    public interface IFontGenerator
    {
        IFont CreateFont(string filePath, int fontSize);
    }
}
