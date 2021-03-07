using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Entities.Lights;
using SpiceEngineCore.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace SweetGraphicsCore.Buffers
{
    public class LightBuffer : UniformBuffer<PLight>
    {
        public const string NAME = "PointLightBlock";
        public const int BINDING = 1;

        public LightBuffer(IRenderContextProvider contextProvider) : base(contextProvider, NAME, BINDING) { }

        public List<PLight> PointLights { get; } = new List<PLight>();

        public void AddPointLight(PointLight light) => PointLights.Add(light.ToStruct());
        public void AddPointLights(IEnumerable<PointLight> lights) => PointLights.AddRange(lights.Select(l => l.ToStruct()));

        public void Clear() => PointLights.Clear();

        public override void Buffer()
        {
            Bind();
            GL.BufferData(BufferTargetARB.UniformBuffer, _size * PointLights.Count, PointLights.ToArray(), BufferUsageARB.DynamicDraw);
        }
    }
}
