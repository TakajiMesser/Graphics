using SpiceEngineCore.UserInterfaces;

namespace SpiceEngineCore.Rendering.Textures
{
    public interface ITextureProvider
    {
        bool EnableMipMapping { get; set; }
        bool EnableAnisotropy { get; set; }

        int AddTexture(ITexture texture);
        int AddTexture(IFont font);
        int AddTexture(string texturePath);

        ITexture RetrieveTexture(int index);
    }
}
