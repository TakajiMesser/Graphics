using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Rendering.Materials;
using SpiceEngineCore.Rendering.Textures;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SpiceEngineCore.Rendering.Shaders
{
    public interface IShader
    {
        void Use();

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
