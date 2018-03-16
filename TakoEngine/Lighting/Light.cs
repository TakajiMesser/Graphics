using TakoEngine.GameObjects;
using TakoEngine.Outputs;
using TakoEngine.Rendering.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TakoEngine.Lighting
{
    public abstract class Light : GameEntity
    {
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
