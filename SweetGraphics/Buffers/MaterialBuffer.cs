using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Materials;
using System.Collections.Generic;

namespace SweetGraphicsCore.Buffers
{
    public class MaterialBuffer : UniformBuffer<Material>
    {
        public const string NAME = "MaterialBlock";
        public const int BINDING = 0;

        public MaterialBuffer(IRenderContext renderContext) : base(renderContext, NAME, BINDING) { }

        public List<Material> Materials { get; } = new List<Material>();

        public void AddMatrix(Material material) => Materials.Add(material);
        public void AddMatrices(IEnumerable<Material> materials) => Materials.AddRange(materials);

        public void Clear() => Materials.Clear();

        public override void Buffer()
        {
            Bind();
            GL.BufferData(BufferTargetARB.UniformBuffer, _size * Materials.Count, Materials.ToArray(), BufferUsageARB.DynamicDraw);
        }
    }
}
