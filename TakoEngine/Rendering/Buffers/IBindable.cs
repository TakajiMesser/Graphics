using TakoEngine.Lighting;
using TakoEngine.Rendering.Shaders;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TakoEngine.Rendering.Buffers
{
    public interface IBindable
    {
        void Bind();
        void Unbind();
    }
}
