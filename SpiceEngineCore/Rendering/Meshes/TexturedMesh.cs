using SpiceEngineCore.Rendering.Materials;
using SpiceEngineCore.Rendering.Textures;
using SpiceEngineCore.Rendering.Vertices;
using System;
using System.Collections.Generic;

namespace SpiceEngineCore.Rendering.Meshes
{
    public class TexturedMesh<T> : Mesh<T>, ITexturedMesh, IDisposable where T : IVertex3D
    {
        public Material Material { get; set; }
        public TextureMapping? TextureMapping { get; set; }

        public TexturedMesh(List<T> vertices, List<int> triangleIndices) : base(vertices, triangleIndices) { }

        /*public virtual void SaveToFile()
        {
            throw new NotImplementedException();
        }*/
    }
}
