using OpenTK;

namespace SpiceEngine.Rendering.Meshes
{
    public class ModelTriangle : IModelShape, ITexturedShape
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

        public void Translate(Vector3 translation)
        {
            VertexA.Translate(translation);
            VertexB.Translate(translation);
            VertexC.Translate(translation);
        }

        public void Rotate(Quaternion rotation)
        {

        }

        public void Scale(Vector3 scale)
        {

        }

        public void TranslateTexture(float x, float y)
        {
            var translation = new Vector2(x, y);

            VertexA.TranslateTexture(translation);
            VertexB.TranslateTexture(translation);
            VertexC.TranslateTexture(translation);
        }

        public void RotateTexture(float angle)
        {
            var center = GetAveragePosition();

            VertexA.RotateTexture(center, angle);
            VertexB.RotateTexture(center, angle);
            VertexC.RotateTexture(center, angle);
        }

        public void ScaleTexture(float x, float y)
        {
            VertexA.ScaleTexture(x, y);
            VertexB.ScaleTexture(x, y);
            VertexC.ScaleTexture(x, y);
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
