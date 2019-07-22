using OpenTK;
using OpenTK.Graphics;
using SpiceEngine.Rendering.Vertices;
using System.Collections.Generic;

namespace SpiceEngine.Rendering.Meshes
{
    public class ModelVertex : IModelShape
    {
        private Vector3 _origin = Vector3.Zero;

        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }
        public Vector3 Tangent { get; set; }
        public Vector2 UV { get; set; }
        public Color4 Color { get; set; }
        public Vector4? BoneIDs { get; set; }
        public Vector4? BoneWeights { get; set; }

        public Vector3 Origin
        {
            get => _origin;
            set
            {
                // P2 = O1 + P1 - O2
                // P1 = P2 - O1 + O2
                Position = _origin + Position - value;
                _origin = value;
            }
        }

        public void Translate(float x, float y, float z) => Position += new Vector3(x, y, z);//new Vector3(x * 100.0f, y * 100.0f, z * 100.0f);

        public void TranslateTexture(float x, float y)
        {
            UV = new Vector2()
            {
                X = UV.X + x,
                Y = UV.Y + y
            };
        }

        public void RotateTexture(float angle) { }

        public void ScaleTexture(float x, float y) { }

        public Vector3 GetAveragePosition() => Position + Origin;

        public void CenterAround(Vector3 position) => Origin = position;

        public bool IsAnimated => BoneIDs.HasValue && BoneWeights.HasValue;

        public ModelVertex Duplicated() => new ModelVertex()
        {
            Position = Position,
            Normal = Normal,
            Tangent = Tangent,
            UV = UV,
            Color = Color,
            BoneIDs = BoneIDs,
            BoneWeights = BoneWeights
        };

        public Vertex3D ToVertex3D() => new Vertex3D(Position, Normal, Tangent, UV, Color);

        public AnimatedVertex3D ToJointVertex3D() => (AnimatedVertex3D)new AnimatedVertex3D(Position, Normal, Tangent, UV,
            BoneIDs ?? Vector4.Zero, BoneWeights ?? Vector4.Zero)
            .Colored(Color);
    }
}
