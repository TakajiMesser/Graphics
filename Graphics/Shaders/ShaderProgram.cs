using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES20;
using Graphics.TwoDimensional;

namespace Graphics
{
    public class ShaderProgram
    {
        private readonly int _handle;

        public ShaderProgram(params Shader[] shaders)
        {
            _handle = GL.CreateProgram();

            foreach (var shader in shaders)
            {
                GL.AttachShader(_handle, shader.Handle);
            }

            GL.LinkProgram(_handle);

            foreach (var shader in shaders)
            {
                GL.DetachShader(_handle, shader.Handle);
            }
        }

        public void Use()
        {
            GL.UseProgram(_handle);
        }

        public int GetAttributeLocation(string name)
        {
            return GL.GetAttribLocation(_handle, name);
        }

        public int GetUniformLocation(string name)
        {
            return GL.GetUniformLocation(_handle, name);
        }
    }
}
