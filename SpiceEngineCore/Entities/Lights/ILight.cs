using SpiceEngineCore.Geometry.Vectors;

namespace SpiceEngineCore.Entities
{
    public interface ILight : IEntity
    {
        Vector4 Color { get; set; }
        float Intensity { get; set; }

        /*void SetUniforms(ShaderProgram program);

        void DrawForStencilPass(ShaderProgram program);
        void DrawForLightPass(ShaderProgram program);*/
    }
}
