using TakoEngine.Materials;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TakoEngine.Rendering.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ColorVertex// : IVertex
    {
        public Vector3 Position;// { get; set; }
        public Vector4 Color;// { get; set; }

        public ColorVertex(Vector3 position, Vector4 color)
        {
            Position = position;
            Color = color;
        }
    }
}
