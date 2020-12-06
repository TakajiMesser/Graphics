namespace SpiceEngineCore.Rendering.Textures
{
    public struct TextureBinding
    {
        public TextureBinding(string name, ITexture texture)
        {
            Name = name;
            Texture = texture;
        }

        public string Name { get; }
        public ITexture Texture { get; }
    }
}
