using OpenTK;

namespace SpiceEngine.Rendering.Meshes
{
    public class MeshTriangle
    {
        public Vector3 VertexA { get; set; }
        public Vector3 VertexB { get; set; }
        public Vector3 VertexC { get; set; }

        public Vector3 Normal { get; set; }
        public Vector3 Tangent { get; set; }

        public MeshTriangle() { }
        public MeshTriangle(Vector3 vertexA, Vector3 vertexB, Vector3 vertexC)
        {
            VertexA = vertexA;
            VertexB = vertexB;
            VertexC = vertexC;
        }
    }
}
