using OpenTK;

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
