using Graphics.Lighting;
using Graphics.Rendering.Shaders;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Rendering.Buffers
{
    public interface IBindable
    {
        void Bind();
        void Unbind();
    }
}
