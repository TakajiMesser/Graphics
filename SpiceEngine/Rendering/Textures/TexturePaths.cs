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

        public TextureMapping ToTextureMapping(TextureManager textureManager)
        {
            var diffuseMapID = !string.IsNullOrEmpty(DiffuseMapFilePath) ? textureManager.AddTexture(DiffuseMapFilePath) : 0;
            var normalMapID = !string.IsNullOrEmpty(NormalMapFilePath) ? textureManager.AddTexture(NormalMapFilePath) : 0;
            var specularMapID = !string.IsNullOrEmpty(SpecularMapFilePath) ? textureManager.AddTexture(SpecularMapFilePath) : 0;
            var parallaxMapID = !string.IsNullOrEmpty(ParallaxMapFilePath) ? textureManager.AddTexture(ParallaxMapFilePath) : 0;

            return new TextureMapping(diffuseMapID, normalMapID, specularMapID, parallaxMapID);
        }
    }
}
