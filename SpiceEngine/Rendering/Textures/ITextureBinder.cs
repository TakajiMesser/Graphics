using SpiceEngine.Rendering.Shaders;

namespace SpiceEngine.Rendering.Textures
{
    public interface ITextureBinder
    {
        void BindTextures(ShaderProgram program, ITextureProvider textureProvider);
    }
}
