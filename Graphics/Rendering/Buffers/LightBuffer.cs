using Graphics.Lighting;
using Graphics.Rendering.Shaders;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Rendering.Buffers
{
    public class LightBuffer : UniformBuffer<Light>
    {
        public const string NAME = "LightBlock";
        public const int BINDING = 0;

        public List<Light> Lights { get; } = new List<Light>();

        public LightBuffer(ShaderProgram program) : base(NAME, BINDING, program) { }

        public void AddLight(Light light) => Lights.Add(light);

        public void AddLights(IEnumerable<Light> lights) => Lights.AddRange(lights);

        public void Clear() => Lights.Clear();

        public override void Bind()
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, _handle);
            GL.BufferData(BufferTarget.UniformBuffer, _size * Lights.Count, Lights.ToArray(), BufferUsageHint.DynamicDraw);
        }
    }
}
