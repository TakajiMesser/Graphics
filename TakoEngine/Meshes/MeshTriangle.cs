using TakoEngine.Materials;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakoEngine.Meshes
{
    public struct MeshTriangle
    {
        public int VertexIndexA { get; set; }
        public int VertexIndexB { get; set; }
        public int VertexIndexC { get; set; }

        public MeshTriangle(int vertexIndexA, int vertexIndexB, int vertexIndexC)
        {
            VertexIndexA = vertexIndexA;
            VertexIndexB = vertexIndexB;
            VertexIndexC = vertexIndexC;
        }
    }
}
