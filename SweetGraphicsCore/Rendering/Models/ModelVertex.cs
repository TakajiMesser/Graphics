using SpiceEngineCore.Rendering.Matrices;
using SweetGraphicsCore.Vertices;
using System;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SweetGraphicsCore.Rendering.Models
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

        public void Transform(Transform transform)
        {
            _origin += transform.Translation;
            Position = (new Vector4(Position, 1.0f) * Matrix4.CreateFromQuaternion(transform.Rotation)).Xyz;
        }

        public void Translate(Vector3 translation)
        {
            _origin += translation;
            //Position -= translation;
            //Position += translation;//new Vector3(x * 100.0f, y * 100.0f, z * 100.0f);
            //Origin += translation;
            //Position += translation;
        }

        public void Rotate(Quaternion rotation) => Position = (new Vector4(Position, 1.0f) * Matrix4.CreateFromQuaternion(rotation)).Xyz;

        public void TranslateTexture(Vector2 translation) => UV += translation;

        public void RotateTexture(Vector3 center, float angle)
        {
            // Let's say our point is at (1, 0), then the angle is 0...
            // For this vertex, get the physical distance from the center point
            // This is now our radius... now we need to rotate by this amount around the unit circle
            // For a point (x, y) on the unit circle, x = cos(angle) and y = sin(angle)
            var radius = (Position - center).Length;

            // We need to somehow determine the current angle that this point is at...
            //var currentAngle = ;
            // Position.X = radius * Math.Cos(currentAngle)
            // Position.Y = radius * Math.Sin(currentAngle)
            // Solve for currentAngle...........
            // Math.Acos(x / radius) = angle
            // Math.Asin(y / radius) = angle

            var x = radius * (float)Math.Cos(angle);
            var y = radius * (float)Math.Sin(angle);
        }

        public void ScaleTexture(float x, float y)
        {

        }

        public void TransformTexture(Matrix4 matrix)
        {

        }

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
