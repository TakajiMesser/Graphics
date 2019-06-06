using OpenTK;

namespace SpiceEngine.Rendering.Meshes
{
    public class MeshTriangle
    {
        public MeshVertex VertexA { get; set; }
        public MeshVertex VertexB { get; set; }
        public MeshVertex VertexC { get; set; }

        public Vector3 Normal { get; set; }
        public Vector3 Tangent { get; set; }
        public Vector3 Bitangent => -Vector3.Cross(Normal, Tangent);

        public MeshTriangle() { }
        public MeshTriangle(MeshVertex vertexA, MeshVertex vertexB, MeshVertex vertexC)
        {
            VertexA = vertexA;
            VertexB = vertexB;
            VertexC = vertexC;
        }
    }
}
