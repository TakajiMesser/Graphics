namespace SpiceEngineCore.Rendering.Textures
{
    public struct TextureMapping
    {
        public const string DIFFUSE_NAME = "diffuseMap";
        public const string NORMAL_NAME = "normalMap";
        public const string SPECULAR_NAME = "specularMap";
        public const string PARALLAX_NAME = "parallaxMap";

        public int DiffuseIndex { get; }
        public int NormalIndex { get; }
        public int SpecularIndex { get; }
        public int ParallaxIndex { get; }

        public TextureMapping(int diffuseIndex, int normalIndex, int specularIndex, int parallaxIndex)
        {
            DiffuseIndex = diffuseIndex;
            NormalIndex = normalIndex;
            SpecularIndex = specularIndex;
            ParallaxIndex = parallaxIndex;
        }
    }
}
