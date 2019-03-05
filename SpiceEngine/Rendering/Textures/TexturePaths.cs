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
            var textureMapping = new TextureMapping();

            if (!string.IsNullOrEmpty(DiffuseMapFilePath))
            {
                textureMapping.DiffuseMapID = textureManager.AddTexture(DiffuseMapFilePath);
            }

            if (!string.IsNullOrEmpty(NormalMapFilePath))
            {
                textureMapping.NormalMapID = textureManager.AddTexture(NormalMapFilePath);
            }

            if (!string.IsNullOrEmpty(SpecularMapFilePath))
            {
                textureMapping.SpecularMapID = textureManager.AddTexture(SpecularMapFilePath);
            }

            if (!string.IsNullOrEmpty(ParallaxMapFilePath))
            {
                textureMapping.ParallaxMapID = textureManager.AddTexture(ParallaxMapFilePath);
            }

            return textureMapping;
        }
    }
}
