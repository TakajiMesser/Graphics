using OpenTK;
using SpiceEngineCore.Rendering.Shaders;

namespace SpiceEngineCore.Entities
{
    public interface ILight : IEntity
    {
        Vector4 Color { get; set; }
        float Intensity { get; set; }

        void DrawForStencilPass(ShaderProgram program);
        void DrawForLightPass(ShaderProgram program);
    }
}
