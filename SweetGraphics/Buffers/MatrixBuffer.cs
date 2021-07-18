using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering;
using System.Collections.Generic;

namespace SweetGraphicsCore.Buffers
{
    public class MatrixBuffer : UniformBuffer<Matrix4>
    {
        public MatrixBuffer(IRenderContext renderContext, string name, int binding) : base(renderContext, name, binding) { }

        public List<Matrix4> Matrices { get; } = new List<Matrix4>();

        public void AddMatrix(Matrix4 matrix) => Matrices.Add(matrix);
        public void AddMatrices(IEnumerable<Matrix4> matrices) => Matrices.AddRange(matrices);

        public void Clear() => Matrices.Clear();

        public override void Buffer()
        {
            Bind();
            GL.BufferData(BufferTargetARB.UniformBuffer, _size * Matrices.Count, Matrices.ToArray(), BufferUsageARB.DynamicDraw);
        }
    }
}
