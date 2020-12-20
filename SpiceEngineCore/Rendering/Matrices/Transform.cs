using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SpiceEngineCore.Rendering.Matrices
{
    public class Transform
    {
        public Vector3 Translation { get; set; } = Vector3.Zero;
        public Quaternion Rotation { get; set; } = Quaternion.Identity;
        public Vector3 Scale { get; set; } = Vector3.One;

        public Transform() { }
        public Transform(Vector3 translation, Quaternion rotation, Vector3 scale)
        {
            Translation = translation;
            Rotation = rotation;
            Scale = scale;
        }

        public void Combine(Transform transform)
        {
            Translation += transform.Translation;
            Rotation = transform.Rotation * Rotation;
            Scale *= transform.Scale;
        }

        public Matrix4 ToMatrix() => Matrix4.CreateScale(Scale) * Matrix4.CreateFromQuaternion(Rotation) * Matrix4.CreateTranslation(Translation);

        public static Transform FromTranslation(Vector3 translation) => new Transform()
        {
            Translation = translation
        };

        public static Transform FromRotation(Quaternion rotation) => new Transform()
        {
            Rotation = rotation
        };

        public static Transform FromScale(Vector3 scale) => new Transform()
        {
            Scale = scale
        };
    }
}
