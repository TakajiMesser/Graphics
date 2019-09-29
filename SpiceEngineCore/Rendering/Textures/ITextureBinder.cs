using SpiceEngineCore.Rendering.Materials;
using SpiceEngineCore.Rendering.Shaders;

namespace SpiceEngineCore.Rendering.Textures
{
    public interface ITextureBinder
    {
        Material Material { get; }
        TextureMapping? TextureMapping { get; }

        void AddMaterial(Material material);
        void AddTextureMapping(TextureMapping? textureMapping);
        void BindTextures(ShaderProgram program, ITextureProvider textureProvider);
    }
}
