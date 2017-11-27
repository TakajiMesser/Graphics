using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public struct VertexBase
    {
        static int Size { get; }
        IEnumerable<VertexAttribute> Attributes { get; }
    }
}
