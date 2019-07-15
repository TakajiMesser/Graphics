using OpenTK;

namespace SpiceEngine.Rendering.Meshes
{
    public class ModelTriangle : IMeshShape
    {
        public ModelVertex VertexA { get; set; }
        public ModelVertex VertexB { get; set; }
        public ModelVertex VertexC { get; set; }

        public Vector3 Normal { get; set; }
        public Vector3 Tangent { get; set; }
        public Vector3 Bitangent => -Vector3.Cross(Normal, Tangent);

        public ModelTriangle() { }
        public ModelTriangle(ModelVertex vertexA, ModelVertex vertexB, ModelVertex vertexC)
        {
            VertexA = vertexA;
            VertexB = vertexB;
            VertexC = vertexC;
        }

        public ModelTriangle Duplicated() => new ModelTriangle()
        {
            VertexA = VertexA.Duplicated(),
            VertexB = VertexB.Duplicated(),
            VertexC = VertexC.Duplicated(),
            Normal = Normal,
            Tangent = Tangent
        };

        public Vector3 GetAveragePosition() => new Vector3()
        {
            X = (VertexA.Position.X + VertexB.Position.X + VertexC.Position.X) / 3.0f,
            Y = (VertexA.Position.Y + VertexB.Position.Y + VertexC.Position.Y) / 3.0f,
            Z = (VertexA.Position.Z + VertexB.Position.Z + VertexC.Position.Z) / 3.0f
        };

        public void CenterAround(Vector3 position)
        {
            VertexA.Position -= position;
            VertexB.Position -= position;
            VertexC.Position -= position;
        }
    }
}
