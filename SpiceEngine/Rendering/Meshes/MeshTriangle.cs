using OpenTK;

namespace SpiceEngine.Rendering.Meshes
{
    public class MeshTriangle : IMeshShape
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

        public Vector3 GetAveragePosition() => new Vector3()
        {
            X = (VertexA.X + VertexB.X + VertexC.X) / 3.0f,
            Y = (VertexA.Y + VertexB.Y + VertexC.Y) / 3.0f,
            Z = (VertexA.Z + VertexB.Z + VertexC.Z) / 3.0f
        };

        public void CenterAround(Vector3 position)
        {
            VertexA = VertexA - position;
            VertexB = VertexB - position;
            VertexC = VertexC - position;
        }
    }
}
