namespace StarchUICore.Text
{
    public interface IFontProvider
    {
        IFont AddFontFile(string filePath, int fontSize);
        IFont GetFont(string filePath);
    }
}
