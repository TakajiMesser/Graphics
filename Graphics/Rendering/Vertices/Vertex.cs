using Graphics.Materials;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Rendering.Vertices
{
    public struct Vertex
    {
        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }
        public Vector3 Tangent { get; set; }
        public Color4 Color { get; set; }
        public Vector2 TextureCoords { get; set; }
        public int MaterialIndex { get; set; }

        public Vertex(Vector3 position, Vector3 normal, Vector3 tangent, Vector2 textureCoords, int materialIndex)
        {
            Position = position;
            Normal = normal;
            Tangent = tangent;
            Color = new Color4();
            TextureCoords = textureCoords;
            MaterialIndex = materialIndex;
        }

        public Vertex(Vector3 position, Vector3 normal, Vector3 tangent, Color4 color, Vector2 textureCoords, int materialIndex)
        {
            Position = position;
            Normal = normal;
            Tangent = tangent;
            Color = color;
            TextureCoords = textureCoords;
            MaterialIndex = materialIndex;
        }
    }
}
