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
    public class LightBuffer : UniformBuffer<PLight>
    {
        public const string NAME = "PointLightBlock";
        public const int BINDING = 1;

        public List<PLight> PointLights { get; } = new List<PLight>();

        public LightBuffer(ShaderProgram program) : base(NAME, BINDING, program) { }

        public void AddPointLight(PointLight light) => PointLights.Add(light.ToStruct());
        public void AddPointLights(IEnumerable<PointLight> lights) => PointLights.AddRange(lights.Select(l => l.ToStruct()));

        public void Clear() => PointLights.Clear();

        public override void Bind()
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, _handle);
            GL.BufferData(BufferTarget.UniformBuffer, _size * PointLights.Count, PointLights.ToArray(), BufferUsageHint.DynamicDraw);
        }
    }
}
