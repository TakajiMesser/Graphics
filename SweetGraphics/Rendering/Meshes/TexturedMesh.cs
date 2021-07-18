using SpiceEngineCore.Rendering.Materials;
using SpiceEngineCore.Rendering.Textures;
using SweetGraphicsCore.Vertices;

namespace SweetGraphicsCore.Rendering.Meshes
{
    public class TexturedMesh<T> : Mesh<T>, ITexturedMesh where T : struct, IVertex3D
    {
        public TexturedMesh(Vertex3DSet<T> vertexSet) : base(vertexSet) { }

        public Material Material { get; set; }
        public TextureMapping? TextureMapping { get; set; }

        /*public virtual void SaveToFile()
        {
            throw new NotImplementedException();
        }*/
    }
}
