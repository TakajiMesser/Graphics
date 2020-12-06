using SpiceEngineCore.Rendering.Materials;
using System.Collections.Generic;

namespace SpiceEngineCore.Rendering.Textures
{
    public interface ITextureBinder
    {
        IEnumerable<Material> Materials { get; }
        IEnumerable<TextureMapping?> TextureMappings { get; }

        Material CurrentMaterial { get; }
        TextureMapping? CurrentTextureMapping { get; }

        void AddMaterial(Material material);
        void AddTextureMapping(TextureMapping? textureMapping);
    }
}
