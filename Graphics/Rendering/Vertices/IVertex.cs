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
    public interface IVertex
    {
        Vector3 Position { get; }
        Vector3 Normal { get; }
        Vector3 Tangent { get; }
        Color4 Color { get; }
        Vector2 TextureCoords { get; }
        int MaterialIndex { get; }
    }
}
