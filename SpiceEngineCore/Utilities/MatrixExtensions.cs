using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SpiceEngineCore.Utilities
{
    public static class MatrixExtensions
    {
        public static Vector3 GetTranslation(this Matrix4 transform) =>
            new Vector3(transform.M14, transform.M24, transform.M34);
            //new Vector3(transform.M03, transform.M13, transform.M23);

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

            /*var rotationMatrix = new Matrix3()
            {
                M00 = transform.M00 / scale.X,
                M01 = transform.M01 / scale.Y,
                M02 = transform.M02 / scale.Z,
                M10 = transform.M10 / scale.X,
                M11 = transform.M11 / scale.Y,
                M12 = transform.M12 / scale.Z,
                M20 = transform.M20 / scale.X,
                M21 = transform.M21 / scale.Y,
                M22 = transform.M22 / scale.Z
            };*/

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
            /*return new Matrix3()
            {
                M00 = transform.M00 / scale.X,
                M01 = transform.M01 / scale.Y,
                M02 = transform.M02 / scale.Z,
                M10 = transform.M10 / scale.X,
                M11 = transform.M11 / scale.Y,
                M12 = transform.M12 / scale.Z,
                M20 = transform.M20 / scale.X,
                M21 = transform.M21 / scale.Y,
                M22 = transform.M22 / scale.Z
            };*/
        }
    }
}
