using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace SpiceEngine.Rendering.Buffers
{
    public class MatrixBuffer : UniformBuffer<Matrix4>
    {
        public List<Matrix4> Matrices { get; } = new List<Matrix4>();

        public MatrixBuffer(string name, int binding) : base(name, binding) { }

        public void AddMatrix(Matrix4 matrix) => Matrices.Add(matrix);
        public void AddMatrices(IEnumerable<Matrix4> matrices) => Matrices.AddRange(matrices);

        public void Clear() => Matrices.Clear();

        public override void Bind()
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, _handle);
            GL.BufferData(BufferTarget.UniformBuffer, _size * Matrices.Count, Matrices.ToArray(), BufferUsageHint.DynamicDraw);
        }
    }
}
