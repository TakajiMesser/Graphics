using Graphics.Materials;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Rendering.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TextureVertex// : IVertex
    {
        public Vector2 Position;// { get; set; }
        public Vector2 TextureCoords;// { get; set; }

        public TextureVertex(Vector2 position, Vector2 textureCoords)
        {
            Position = position;
            TextureCoords = textureCoords;
        }
    }
}
