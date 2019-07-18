using OpenTK;

namespace SpiceEngine.Rendering.Meshes
{
    public class ModelTriangle : IModelShape
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

        public void Translate(float x, float y, float z)
        {
            VertexA.Translate(x, y, z);
            VertexB.Translate(x, y, z);
            VertexC.Translate(x, y, z);
        }

        public Vector3 GetAveragePosition() => new Vector3()
        {
            X = (VertexA.Position.X + VertexA.Origin.X + VertexB.Position.X + VertexB.Origin.X + VertexC.Position.X + VertexC.Origin.X) / 3.0f,
            Y = (VertexA.Position.Y + VertexA.Origin.Y + VertexB.Position.Y + VertexB.Origin.Y + VertexC.Position.Y + VertexC.Origin.Y) / 3.0f,
            Z = (VertexA.Position.Z + VertexA.Origin.Z + VertexB.Position.Z + VertexB.Origin.Z + VertexC.Position.Z + VertexC.Origin.Z) / 3.0f
        };

        public void CenterAround(Vector3 position)
        {
            VertexA.CenterAround(position);
            VertexB.CenterAround(position);
            VertexC.CenterAround(position);
        }
    }
}
