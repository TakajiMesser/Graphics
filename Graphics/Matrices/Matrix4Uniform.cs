using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES20;
using Graphics.Geometry.TwoDimensional;
using Graphics.Rendering.Shaders;

namespace Graphics
{
    public class Matrix4Uniform
    {
        private readonly string _name;

        private Matrix4 _matrix;
        public Matrix4 Matrix
        {
            get => _matrix;
            set => _matrix = value;
        }

        public Matrix4Uniform(string name)
        {
            _name = name;
        }

        public void Set(ShaderProgram program)
        {
            int location = program.GetUniformLocation(_name);
            GL.UniformMatrix4(location, false, ref _matrix);
        }
    }
}
