using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using System;

namespace SweetGraphicsCore.Vertices
{
    public class VertexAttribute
    {
        public string Name { get; private set; }

        private readonly int _size;
        private readonly VertexAttribPointerType _type;
        private readonly bool _normalize;
        private readonly int _stride;
        private readonly IntPtr _offset;

        public VertexAttribute(string name, int size, VertexAttribPointerType type, int stride, int offset, bool normalize = false)
        {
            Name = name;
            _size = size;
            _type = type;
            _stride = stride;
            _offset = (IntPtr)offset;
            _normalize = normalize;
        }

        public void Set(int index)
        {
            GL.EnableVertexAttribArray(index);
            GL.VertexAttribPointer(index, _size, _type, _normalize, _stride, _offset);
            /*if (_type == VertexAttribPointerType.Int)
            {
                GL.VertexAttribIPointer(index, _size, VertexAttribIntegerType.Int, _normalize, _stride, _offset);
            }*/
        }
    }
}
