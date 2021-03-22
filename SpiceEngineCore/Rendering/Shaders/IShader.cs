using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering.Materials;
using SpiceEngineCore.Rendering.Textures;

namespace SpiceEngineCore.Rendering.Shaders
{
    public interface IShader
    {
        void Bind();

        void SetUniform<T>(string name, T value) where T : struct;

        void SetUniform(string name, Matrix4 matrix);
        void SetUniform(string name, Matrix4[] matrices);
        void SetUniform(string name, Vector2 vector);
        void SetUniform(string name, Vector3 vector);
        void SetUniform(string name, Vector4 vector);
        void SetUniform(string name, Color4 color);
        void SetUniform(string name, float value);
        void SetUniform(string name, int value);

        void SetMaterial(Material material);
        void SetCamera(ICamera camera);
        void SetLight(ILight light);
        void SetLightView(ILight light);
        void StencilPass(ILight light);
        void LightPass(ILight light);

        void BindTexture(ITexture texture, string name, int index);
        void BindImageTexture(ITexture texture, string name, int index);
        void BindTextures(ITextureProvider textureProvider, TextureMapping textureMapping);
        void UnbindTextures();
    }
}
