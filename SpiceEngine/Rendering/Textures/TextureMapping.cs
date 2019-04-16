namespace SpiceEngine.Rendering.Textures
{
    public struct TextureMapping
    {
        public int DiffuseMapID { get; }
        public int NormalMapID { get; }
        public int SpecularMapID { get; }
        public int ParallaxMapID { get; }

        public TextureMapping(int diffuseMapID, int normalMapID, int specularMapID, int parallaxMapID)
        {
            DiffuseMapID = diffuseMapID;
            NormalMapID = normalMapID;
            SpecularMapID = specularMapID;
            ParallaxMapID = parallaxMapID;
        }
    }
}
