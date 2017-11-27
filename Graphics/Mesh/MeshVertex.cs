using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Mesh
{
    public struct MeshVertex
    {
        public Vector3 Position { get; private set; }
        public Vector3 Normal { get; private set; }
        public Color4 Color { get; private set; }
        public Vector2 TextureCoords { get; private set; }

        public MeshVertex(Vector3 position, Vector3 normal, Color4 color, Vector2 textureCoords)
        {
            Position = position;
            Normal = normal;
            Color = color;
            TextureCoords = textureCoords;
        }
    }
}
