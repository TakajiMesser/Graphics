using System.IO;

namespace SpiceEngine.Rendering.Textures
{
    public class TexturePaths
    {
        public string DiffuseMapFilePath { get; set; }
        public string NormalMapFilePath { get; set; }
        public string SpecularMapFilePath { get; set; }
        public string ParallaxMapFilePath { get; set; }

        public bool IsEmpty => string.IsNullOrEmpty(DiffuseMapFilePath)
            && string.IsNullOrEmpty(NormalMapFilePath)
            && string.IsNullOrEmpty(SpecularMapFilePath)
            && string.IsNullOrEmpty(ParallaxMapFilePath);

        public TexturePaths() { }
        public TexturePaths(Assimp.Material material, string directoryPath)
        {
            if (!string.IsNullOrEmpty(material.TextureDiffuse.FilePath))
            {
                DiffuseMapFilePath = Path.Combine(directoryPath, material.TextureDiffuse.FilePath);
            }

            if (!string.IsNullOrEmpty(material.TextureNormal.FilePath))
            {
                NormalMapFilePath = Path.Combine(directoryPath, material.TextureNormal.FilePath);
            }

            if (!string.IsNullOrEmpty(material.TextureSpecular.FilePath))
            {
                SpecularMapFilePath = Path.Combine(directoryPath, material.TextureSpecular.FilePath);
            }
        }

        public TextureMapping ToTextureMapping(ITextureProvider textureProvider)
        {
            var diffuseIndex = !string.IsNullOrEmpty(DiffuseMapFilePath) ? textureProvider.AddTexture(DiffuseMapFilePath) : -1;
            var normalIndex = !string.IsNullOrEmpty(NormalMapFilePath) ? textureProvider.AddTexture(NormalMapFilePath) : -1;
            var specularIndex = !string.IsNullOrEmpty(SpecularMapFilePath) ? textureProvider.AddTexture(SpecularMapFilePath) : -1;
            var parallaxIndex = !string.IsNullOrEmpty(ParallaxMapFilePath) ? textureProvider.AddTexture(ParallaxMapFilePath) : -1;

            return new TextureMapping(diffuseIndex, normalIndex, specularIndex, parallaxIndex);
        }
    }
}
