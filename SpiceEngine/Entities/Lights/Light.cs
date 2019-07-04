using OpenTK;
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Rendering.Shaders;
using System;

namespace SpiceEngine.Entities.Lights
{
    public abstract class Light<T> : ILight where T : struct
    {
        protected ModelMatrix _modelMatrix = new ModelMatrix();
        private float _intensity;

        public int ID { get; set; }
        public Vector3 Position
        {
            get => _modelMatrix.Translation;
            set => _modelMatrix.Translation = value;
        }
        public Vector4 Color { get; set; }
        public float Intensity
        {
            get => _intensity;
            set
            {
                if (value > 1.0f) throw new ArgumentOutOfRangeException("Intensity cannot be greater than 1.0");
                if (value < 0.0f) throw new ArgumentOutOfRangeException("Intensity cannot be less than 0.0");

                _intensity = value;
            }
        }

        public void SetUniforms(ShaderProgram program) => _modelMatrix.Set(program);

        public abstract void DrawForStencilPass(ShaderProgram program);
        public abstract void DrawForLightPass(ShaderProgram program);
        public abstract T ToStruct();
    }
}
