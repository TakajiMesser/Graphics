using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Rendering.Materials;
using SpiceEngineCore.Rendering.Textures;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SpiceEngineCore.Game
{
    public interface IRender
    {
        void Use();

        void SetUniform<T>(string name, T value) where T : struct;
        void SetUniform(string name, Color4 color);
        void SetUniform(string name, Matrix4 matrix);
        void SetMaterial(Material material);
        
        void SetCamera(ICamera camera);
        void RenderLightFromCameraPerspective(ILight light, ICamera camera);

        void BindTexture(ITexture texture, string name, int index);
        void BindTextures(ITextureProvider textureProvider, TextureMapping textureMapping);
        void UnbindTextures();
    }
}
