namespace SpiceEngine.Rendering.Textures
{
    public interface ITextureProvider
    {
        bool EnableMipMapping { get; set; }
        bool EnableAnisotropy { get; set; }

        int AddTexture(Texture texture);
        int AddTexture(string texturePath);

        Texture RetrieveTexture(int id);
    }
}
