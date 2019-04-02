using OpenTK;
using SpiceEngine.Rendering.Shaders;

namespace SpiceEngine.Entities.Lights
{
    public interface ILight : IEntity
    {
        Vector4 Color { get; set; }
        float Intensity { get; set; }

        void DrawForStencilPass(ShaderProgram program);
        void DrawForLightPass(ShaderProgram program);
    }
}
