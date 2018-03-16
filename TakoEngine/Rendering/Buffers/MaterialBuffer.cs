using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using TakoEngine.Materials;

namespace TakoEngine.Rendering.Buffers
{
    public class MaterialBuffer : UniformBuffer<Material>
    {
        public const string NAME = "MaterialBlock";
        public const int BINDING = 0;

        public List<Material> Materials { get; } = new List<Material>();

        public MaterialBuffer() : base(NAME, BINDING) { }

        public void AddMaterial(Material material) => Materials.Add(material);

        public void AddMaterials(IEnumerable<Material> materials) => Materials.AddRange(materials);

        public void Clear() => Materials.Clear();

        public override void Bind()
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, _handle);
            GL.BufferData(BufferTarget.UniformBuffer, _size * Materials.Count, Materials.ToArray(), BufferUsageHint.DynamicDraw);
        }
    }
}
