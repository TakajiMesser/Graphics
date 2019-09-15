namespace SpiceEngine.Rendering.Textures
{
    public struct TextureMapping
    {
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
