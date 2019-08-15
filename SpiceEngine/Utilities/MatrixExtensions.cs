using OpenTK;
using System.Drawing;

namespace SpiceEngine.Utilities
{
    public static class MatrixExtensions
    {
        public static Vector3 GetTranslation(this Matrix4 transform) => new Vector3(transform.M14, transform.M24, transform.M34);

        public static Vector3 GetScale(this Matrix4 transform) => new Vector3(transform.Column0.Length, transform.Column1.Length, transform.Column2.Length);

        public static Quaternion GetRotation(this Matrix4 transform)
        {
            var scale = transform.GetScale();
            var rotationMatrix = new Matrix3()
            {
                M11 = transform.M11 / scale.X,
                M12 = transform.M12 / scale.Y,
                M13 = transform.M13 / scale.Z,
                M21 = transform.M21 / scale.X,
                M22 = transform.M22 / scale.Y,
                M23 = transform.M23 / scale.Z,
                M31 = transform.M31 / scale.X,
                M32 = transform.M32 / scale.Y,
                M33 = transform.M33 / scale.Z
            };

            return Quaternion.FromMatrix(rotationMatrix);
        }

        public static Matrix3 GetRotationMatrix(this Matrix4 transform)
        {
            var scale = transform.GetScale();
            return new Matrix3()
            {
                M11 = transform.M11 / scale.X,
                M12 = transform.M12 / scale.Y,
                M13 = transform.M13 / scale.Z,
                M21 = transform.M21 / scale.X,
                M22 = transform.M22 / scale.Y,
                M23 = transform.M23 / scale.Z,
                M31 = transform.M31 / scale.X,
                M32 = transform.M32 / scale.Y,
                M33 = transform.M33 / scale.Z
            };
        }
    }
}
