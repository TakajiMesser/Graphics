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
        public Quaternion Rotation { get; set; }

        [IgnoreDataMember]
        public Vector3 Scale
        {
            get => throw new NotImplementedException(); //_viewMatrix.Scale;
            set => throw new NotImplementedException(); //_viewMatrix.Scale = value;
        }

        private float _intensity;

        public Vector3 Color { get; set; }
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
        public abstract void DrawForLightPass(Resolution resolution, ShaderProgram program);
    }
}
