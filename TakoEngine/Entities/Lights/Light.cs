using OpenTK;
using System;
using System.Runtime.Serialization;
using TakoEngine.Outputs;
using TakoEngine.Rendering.Shaders;

namespace TakoEngine.Entities.Lights
{
    public abstract class Light : IEntity
    {
        public int ID { get; set; }

        public Vector3 Position { get; set; }

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
    }
}
