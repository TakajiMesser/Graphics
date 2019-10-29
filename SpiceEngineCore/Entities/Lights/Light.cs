using OpenTK;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Rendering.Shaders;
using System;

namespace SpiceEngineCore.Entities.Lights
{
    public abstract class Light<T> : Entity, ILight where T : struct
    {
        private float _intensity;

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

        public abstract void DrawForStencilPass(ShaderProgram program);
        public abstract void DrawForLightPass(ShaderProgram program);
        public abstract T ToStruct();

        public override void SetUniforms(ShaderProgram program) => _modelMatrix.Set(program);
        public bool CompareUniforms(Entity entity) => entity is ILight && base.CompareUniforms(entity);
    }
}
