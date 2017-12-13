using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Graphics.TwoDimensional;

namespace Graphics.Vertices
{
    public class VertexAttribute
    {
        private readonly string _name;
        private readonly int _size;
        private readonly VertexAttribPointerType _type;
        private readonly bool _normalize;
        private readonly int _stride;
        private readonly int _offset;

        public VertexAttribute(string name, int size, VertexAttribPointerType type, int stride, int offset, bool normalize = false)
        {
            _name = name;
            _size = size;
            _type = type;
            _stride = stride;
            _offset = offset;
            _normalize = normalize;
        }

        public void Set(ShaderProgram program)
        {
            int index = program.GetAttributeLocation(_name);
            if (index == -1)
            {
                //throw new ArgumentOutOfRangeException(_name + " not found in program attributes");
            }

            GL.EnableVertexAttribArray(index);
            GL.VertexAttribPointer(index, _size, _type, _normalize, _stride, _offset);
        }
    }
}
