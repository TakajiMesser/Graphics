using SpiceEngine.Rendering.Materials;
using SpiceEngine.Rendering.Shaders;

namespace SpiceEngine.Rendering.Textures
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
