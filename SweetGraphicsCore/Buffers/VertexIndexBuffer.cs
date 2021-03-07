using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Rendering;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SweetGraphicsCore.Buffers
{
    public class VertexIndexBuffer : OpenGLObject
    {
        private List<ushort> _indices = new List<ushort>();

        public VertexIndexBuffer(IRenderContextProvider contextProvider) : base(contextProvider) { }

        protected override int Create() => GL.GenBuffer();
        protected override void Delete() => GL.DeleteBuffer(Handle);

        public override void Bind() => GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, Handle);
        public override void Unbind() => GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);

        public void AddIndex(ushort index) => _indices.Add(index);

        public void AddIndices(IEnumerable<ushort> indices) => _indices.AddRange(indices);

        public void Clear() => _indices.Clear();

        public void Buffer() => GL.BufferData(BufferTargetARB.ElementArrayBuffer, Marshal.SizeOf<ushort>() * _indices.Count, _indices.ToArray(), BufferUsageARB.StreamDraw);

        public void Draw() => GL.DrawElements(PrimitiveType.Triangles, _indices.Count, DrawElementsType.UnsignedShort, IntPtr.Zero);
    }
}
