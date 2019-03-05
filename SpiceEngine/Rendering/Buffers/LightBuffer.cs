using OpenTK.Graphics.OpenGL;
using SpiceEngine.Entities.Lights;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Rendering.Buffers
{
    public class LightBuffer : UniformBuffer<PLight>
    {
        public const string NAME = "PointLightBlock";
        public const int BINDING = 1;

        public List<PLight> PointLights { get; } = new List<PLight>();

        public LightBuffer() : base(NAME, BINDING) { }

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
