using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SweetGraphicsCore.Buffers
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
