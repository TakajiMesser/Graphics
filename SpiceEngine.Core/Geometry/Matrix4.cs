using System;
using System.Runtime.InteropServices;

namespace SpiceEngineCore.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix4 : IEquatable<Matrix4>
    {
        public Matrix4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21, float m22, float m23, float m30, float m31, float m32, float m33)
        {
            M00 = m00;
            M01 = m01;
            M02 = m02;
            M03 = m03;
            M10 = m10;
            M11 = m11;
            M12 = m12;
            M13 = m13;
            M20 = m20;
            M21 = m21;
            M22 = m22;
            M23 = m23;
            M30 = m30;
            M31 = m31;
            M32 = m32;
            M33 = m33;
        }

        public float M00 { get; set; }
        public float M01 { get; set; }
        public float M02 { get; set; }
        public float M03 { get; set; }
        public float M10 { get; set; }
        public float M11 { get; set; }
        public float M12 { get; set; }
        public float M13 { get; set; }
        public float M20 { get; set; }
        public float M21 { get; set; }
        public float M22 { get; set; }
        public float M23 { get; set; }
        public float M30 { get; set; }
        public float M31 { get; set; }
        public float M32 { get; set; }
        public float M33 { get; set; }

        public Vector4 Row0 => new Vector4(M00, M01, M02, M03);
        public Vector4 Row1 => new Vector4(M10, M11, M12, M13);
        public Vector4 Row2 => new Vector4(M20, M21, M22, M23);
        public Vector4 Row3 => new Vector4(M30, M31, M32, M33);

        public Vector4 Column0 => new Vector4(M00, M10, M20, M30);
        public Vector4 Column1 => new Vector4(M01, M11, M21, M31);
        public Vector4 Column2 => new Vector4(M02, M12, M22, M32);
        public Vector4 Column3 => new Vector4(M03, M13, M23, M33);

        public float this[int rowIndex, int columnIndex]
        {
            get => (rowIndex, columnIndex) switch
            {
                (0, 0) => M00,
                (0, 1) => M01,
                (0, 2) => M02,
                (0, 3) => M03,
                (1, 0) => M10,
                (1, 1) => M11,
                (1, 2) => M12,
                (1, 3) => M13,
                (2, 0) => M20,
                (2, 1) => M21,
                (2, 2) => M22,
                (2, 3) => M23,
                (3, 0) => M30,
                (3, 1) => M31,
                (3, 2) => M32,
                (3, 3) => M33,
                _ => throw new ArgumentOutOfRangeException(),
            };
            set
            {
                _ = (rowIndex, columnIndex) switch
                {
                    (0, 0) => M00 = value,
                    (0, 1) => M01 = value,
                    (0, 2) => M02 = value,
                    (0, 3) => M03 = value,
                    (1, 0) => M10 = value,
                    (1, 1) => M11 = value,
                    (1, 2) => M12 = value,
                    (1, 3) => M13 = value,
                    (2, 0) => M20 = value,
                    (2, 1) => M21 = value,
                    (2, 2) => M22 = value,
                    (2, 3) => M23 = value,
                    (3, 0) => M30 = value,
                    (3, 1) => M31 = value,
                    (3, 2) => M32 = value,
                    (3, 3) => M33 = value,
                    _ => throw new ArgumentOutOfRangeException(),
                };
            }
        }

        // TODO - Consider actually storing these matrices as arrays, and having properties mask index access
        public float[] Values => new[] { M00, M01, M02, M03, M10, M11, M12, M13, M20, M21, M22, M23, M30, M31, M32, M33 };

        public float Determinant => M00 * M11 * M22 * M33 - M00 * M11 * M23 * M32 + M00 * M12 * M23 * M31 - M00 * M12 * M21 * M33
            + M00 * M13 * M21 * M32 - M00 * M13 * M22 * M31 - M01 * M12 * M23 * M30 + M01 * M12 * M20 * M33
            - M01 * M13 * M20 * M32 + M01 * M13 * M22 * M30 - M01 * M10 * M22 * M33 + M01 * M10 * M23 * M32
            + M02 * M13 * M20 * M31 - M02 * M13 * M21 * M30 + M02 * M10 * M21 * M33 - M02 * M10 * M23 * M31
            + M02 * M11 * M23 * M30 - M02 * M11 * M20 * M33 - M03 * M10 * M21 * M32 + M03 * M10 * M22 * M31
            - M03 * M11 * M22 * M30 + M03 * M11 * M20 * M32 - M03 * M12 * M20 * M31 + M03 * M12 * M21 * M30;

        public float Trace => M00 + M11 + M22 + M33;

        public Matrix4 Transposed() => new Matrix4(M00, M10, M20, M30, M01, M11, M21, M31, M02, M12, M22, M32, M03, M13, M23, M33);

        public Matrix4 Inverted()
        {
            /*  M00    M01     M02     M03
                M10    M11     M12     M13
                M20    M21     M22     M23
                M30    M31     M32     M33 */

            /*  [0]    [1]     [2]     [3] 
                [4]    [5]     [6]     [7]
                [8]    [9]    [10]    [11]
               [12]   [13]    [14]    [15] */

            var inverseM00 = M11 * M22 * M33
                - M11 * M23 * M32
                - M21 * M12 * M33
                + M21 * M13 * M32
                + M31 * M12 * M23
                - M31 * M13 * M22;

            var inverseM10 = -M10 * M22 * M33
                + M10 * M23 * M32
                + M20 * M12 * M33
                - M20 * M13 * M32
                - M30 * M12 * M23
                + M30 * M13 * M22;

            var inverseM20 = M10 * M21 * M33
                - M10 * M23 * M31
                - M20 * M11 * M33
                + M20 * M13 * M31
                + M30 * M11 * M23
                - M30 * M13 * M21;

            var inverseM30 = -M10 * M21 * M32
                + M10 * M22 * M31
                + M20 * M11 * M32
                - M20 * M12 * M31
                - M30 * M11 * M22
                + M30 * M12 * M21;

            var inverseM01 = -M01 * M22 * M33
                + M01 * M23 * M32
                + M21 * M02 * M33
                - M21 * M03 * M32
                - M31 * M02 * M23
                + M31 * M03 * M22;

            var inverseM11 = M00 * M22 * M33
                - M00 * M23 * M32
                - M20 * M02 * M33
                + M20 * M03 * M32
                + M30 * M02 * M23
                - M30 * M03 * M22;

            var inverseM21 = -M00 * M21 * M33
                + M00 * M23 * M31
                + M20 * M01 * M33
                - M20 * M03 * M31
                - M30 * M01 * M23
                + M30 * M03 * M21;

            var inverseM31 = M00 * M21 * M32
                - M00 * M22 * M31
                - M20 * M01 * M32
                + M20 * M02 * M31
                + M30 * M01 * M22
                - M30 * M02 * M21;

            var inverseM02 = M01 * M12 * M33
                - M01 * M13 * M32
                - M11 * M02 * M33
                + M11 * M03 * M32
                + M31 * M02 * M13
                - M31 * M03 * M12;

            var inverseM12 = -M00 * M12 * M33
                + M00 * M13 * M32
                + M10 * M02 * M33
                - M10 * M03 * M32
                - M30 * M02 * M13
                + M30 * M03 * M12;

            var inverseM22 = M00 * M11 * M33
                - M00 * M13 * M31
                - M10 * M01 * M33
                + M10 * M03 * M31
                + M30 * M01 * M13
                - M30 * M03 * M11;

            var inverseM32 = -M00 * M11 * M32
                + M00 * M12 * M31
                + M10 * M01 * M32
                - M10 * M02 * M31
                - M30 * M01 * M12
                + M30 * M02 * M11;

            var inverseM03 = -M01 * M12 * M23
                + M01 * M13 * M22
                + M11 * M02 * M23
                - M11 * M03 * M22
                - M21 * M02 * M13
                + M21 * M03 * M12;

            var inverseM13 = M00 * M12 * M23
                - M00 * M13 * M22
                - M10 * M02 * M23
                + M10 * M03 * M22
                + M20 * M02 * M13
                - M20 * M03 * M12;

            var inverseM23 = -M00 * M11 * M23
                + M00 * M13 * M21
                + M10 * M01 * M23
                - M10 * M03 * M21
                - M20 * M01 * M13
                + M20 * M03 * M11;

            var inverseM33 = M00 * M11 * M22
                - M00 * M12 * M21
                - M10 * M01 * M22
                + M10 * M02 * M21
                + M20 * M01 * M12
                - M20 * M02 * M11;

            var determinant = M00 * inverseM00 + M01 * inverseM10 + M02 * inverseM20 + M03 * inverseM30;
            if (determinant == 0.0f) throw new InvalidOperationException("Matrix is singular");

            determinant = 1.0f / determinant;

            return new Matrix4(inverseM00 * determinant, inverseM01 * determinant, inverseM02 * determinant, inverseM03 * determinant,
                               inverseM10 * determinant, inverseM11 * determinant, inverseM12 * determinant, inverseM13 * determinant,
                               inverseM20 * determinant, inverseM21 * determinant, inverseM22 * determinant, inverseM23 * determinant,
                               inverseM30 * determinant, inverseM31 * determinant, inverseM32 * determinant, inverseM33 * determinant);
        }

        public override string ToString() => "|" + M00 + "," + M01 + "," + M02 + "," + M03 + "|"
                     + Environment.NewLine + "|" + M10 + "," + M11 + "," + M12 + "," + M13 + "|"
                     + Environment.NewLine + "|" + M20 + "," + M21 + "," + M22 + "," + M23 + "|"
                     + Environment.NewLine + "|" + M30 + "," + M31 + "," + M32 + "," + M33 + "|"; 

        public static Matrix4 operator +(Matrix4 left, Matrix4 right) => new Matrix4(left.M00 + right.M00, left.M01 + right.M01, left.M02 + right.M02, left.M03 + right.M03, left.M10 + right.M10, left.M11 + right.M11, left.M12 + right.M12, left.M13 + right.M13, left.M20 + right.M20, left.M21 + right.M21, left.M22 + right.M22, left.M23 + right.M23, left.M30 + right.M30, left.M31 + right.M31, left.M32 + right.M32, left.M33 + right.M33);

        public static Matrix4 operator -(Matrix4 left, Matrix4 right) => new Matrix4(left.M00 - right.M00, left.M01 - right.M01, left.M02 - right.M02, left.M03 - right.M03, left.M10 - right.M10, left.M11 - right.M11, left.M12 - right.M12, left.M13 - right.M13, left.M20 - right.M20, left.M21 - right.M21, left.M22 - right.M22, left.M23 - right.M23, left.M30 - right.M30, left.M31 - right.M31, left.M32 - right.M32, left.M33 - right.M33);

        public static Matrix4 operator *(Matrix4 left, Matrix4 right) => new Matrix4(left.M00 * right.M00 + left.M01 * right.M10 + left.M02 * right.M20 + left.M03 * right.M30, left.M00 * right.M01 + left.M01 * right.M11 + left.M02 * right.M21 + left.M03 * right.M31, left.M00 * right.M02 + left.M01 * right.M12 + left.M02 * right.M22 + left.M03 * right.M32, left.M00 * right.M03 + left.M01 * right.M13 + left.M02 * right.M23 + left.M03 * right.M33, left.M10 * right.M00 + left.M11 * right.M10 + left.M12 * right.M20 + left.M13 * right.M30, left.M10 * right.M01 + left.M11 * right.M11 + left.M12 * right.M21 + left.M13 * right.M31, left.M10 * right.M02 + left.M11 * right.M12 + left.M12 * right.M22 + left.M13 * right.M32, left.M10 * right.M03 + left.M11 * right.M13 + left.M12 * right.M23 + left.M13 * right.M33, left.M20 * right.M00 + left.M21 * right.M10 + left.M22 * right.M20 + left.M23 * right.M30, left.M20 * right.M01 + left.M21 * right.M11 + left.M22 * right.M21 + left.M23 * right.M31, left.M20 * right.M02 + left.M21 * right.M12 + left.M22 * right.M22 + left.M23 * right.M32, left.M20 * right.M03 + left.M21 * right.M13 + left.M22 * right.M23 + left.M23 * right.M33, left.M30 * right.M00 + left.M31 * right.M10 + left.M32 * right.M20 + left.M33 * right.M30, left.M30 * right.M01 + left.M31 * right.M11 + left.M32 * right.M21 + left.M33 * right.M31, left.M30 * right.M02 + left.M31 * right.M12 + left.M32 * right.M22 + left.M33 * right.M32, left.M30 * right.M03 + left.M31 * right.M13 + left.M32 * right.M23 + left.M33 * right.M33);

        public static bool operator ==(Matrix4 left, Matrix4 right) => left.Equals(right);

        public static bool operator !=(Matrix4 left, Matrix4 right) => !(left == right);

        public static Matrix4 CreateTranslation(Vector3 vector) => new Matrix4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, vector.X, vector.Y, vector.Z, 1);

        public static Matrix4 CreateScale(Vector3 scale) => new Matrix4(scale.X, 0, 0, 0, 0, scale.Y, 0, 0, 0, 0, scale.Z, 0, 0, 0, 0, 1);

        public static Matrix4 CreateScale(float scale) => CreateScale(new Vector3(scale, scale, scale));

        public static Matrix4 CreateFromQuaternion(Quaternion quaternion)
        {
            var axisAngle = quaternion.ToAxisAngle();

            var axis = axisAngle.Xyz.Normalized();
            var cos = (float)Math.Cos(-axisAngle.W);
            var sin = (float)Math.Sin(-axisAngle.W);
            var t = 1f - cos;

            float tXX = t * axis.X * axis.X;
            float tXY = t * axis.X * axis.Y;
            float tXZ = t * axis.X * axis.Z;
            float tYY = t * axis.Y * axis.Y;
            float tYZ = t * axis.Y * axis.Z;
            float tZZ = t * axis.Z * axis.Z;

            float sinX = sin * axis.X;
            float sinY = sin * axis.Y;
            float sinZ = sin * axis.Z;

            return new Matrix4(
                tXX + cos, tXY - sinZ, tXZ + sinY, 0,
                tXY + sinZ, tYY + cos, tYZ - sinX, 0,
                tXZ - sinY, tYZ + sinX, tZZ + cos, 0,
                0, 0, 0, 1);
        }

        public static Matrix4 CreateOrthographic(float width, float height, float depthNear, float depthFar)
        {
            var left = -width / 2;
            var right = width / 2;
            var bottom = -height / 2;
            var top = height / 2;

            var invRL = 1.0f / (right - left);
            var invTB = 1.0f / (top - bottom);
            var invFN = 1.0f / (depthFar - depthNear);

            return new Matrix4(
                2 * invRL, 0, 0, 0,
                0, 2 * invTB, 0, 0,
                0, 0, -2 * invFN, 0,
                -(right + left) * invRL, -(top + bottom) * invTB, -(depthFar + depthNear) * invFN, 1
            );
        }

        public static Matrix4 CreatePerspectiveFieldOfView(float fovy, float aspect, float depthNear, float depthFar)
        {
            if (fovy <= 0 || fovy > Math.PI) throw new ArgumentOutOfRangeException(nameof(fovy));
            if (aspect <= 0) throw new ArgumentOutOfRangeException(nameof(aspect));
            if (depthNear <= 0 || depthNear >= depthFar) throw new ArgumentOutOfRangeException(nameof(depthNear));
            if (depthFar <= 0) throw new ArgumentOutOfRangeException(nameof(depthFar));

            var top = depthNear * (float)Math.Tan(0.5f * fovy);
            var bottom = -top;
            var left = bottom * aspect;
            var right = top * aspect;

            var x = 2.0f * depthNear / (right - left);
            var y = 2.0f * depthNear / (top - bottom);
            var a = (right + left) / (right - left);
            var b = (top + bottom) / (top - bottom);
            var c = -(depthFar + depthNear) / (depthFar - depthNear);
            var d = -(2.0f * depthFar * depthNear) / (depthFar - depthNear);

            return new Matrix4(
                x, 0, 0, 0,
                0, y, 0, 0,
                a, b, c, -1,
                0, 0, d, 0);
        }

        public static Matrix4 LookAt(Vector3 eye, Vector3 target, Vector3 up)
        {
            var z = (eye - target).Normalized();
            var x = Vector3.Cross(up, z).Normalized();
            var y = Vector3.Cross(z, x).Normalized();

            return new Matrix4(
                x.X, y.X, z.X, 0,
                x.Y, y.Y, z.Y, 0,
                x.Z, y.Z, z.Z, 0,
                -((x.X * eye.X) + (x.Y * eye.Y) + (x.Z * eye.Z)), -((y.X * eye.X) + (y.Y * eye.Y) + (y.Z * eye.Z)), -((z.X * eye.X) + (z.Y * eye.Y) + (z.Z * eye.Z)), 1);
        }

        public static Matrix4 Identity => new Matrix4(1.0f, 0.0f, 0.0f, 0.0f,
                                                      0.0f, 1.0f, 0.0f, 0.0f,
                                                      0.0f, 0.0f, 1.0f, 0.0f,
                                                      0.0f, 0.0f, 0.0f, 1.0f);

        public static Matrix4 Zero => new Matrix4(0.0f, 0.0f, 0.0f, 0.0f,
                                                  0.0f, 0.0f, 0.0f, 0.0f,
                                                  0.0f, 0.0f, 0.0f, 0.0f,
                                                  0.0f, 0.0f, 0.0f, 0.0f);

        public override bool Equals(object obj) => obj is Matrix4 matrix && Equals(matrix);

        public bool Equals(Matrix4 other) => M00 == other.M00
            && M01 == other.M01
            && M02 == other.M02
            && M03 == other.M03
            && M10 == other.M10
            && M11 == other.M11
            && M12 == other.M12
            && M13 == other.M13
            && M20 == other.M20
            && M21 == other.M21
            && M22 == other.M22
            && M23 == other.M23
            && M30 == other.M30
            && M31 == other.M31
            && M32 == other.M32
            && M33 == other.M33;

        public override int GetHashCode()
        {
            var hashCode = 1197848312;
            hashCode = hashCode * -1521134295 + M00.GetHashCode();
            hashCode = hashCode * -1521134295 + M01.GetHashCode();
            hashCode = hashCode * -1521134295 + M02.GetHashCode();
            hashCode = hashCode * -1521134295 + M03.GetHashCode();
            hashCode = hashCode * -1521134295 + M10.GetHashCode();
            hashCode = hashCode * -1521134295 + M11.GetHashCode();
            hashCode = hashCode * -1521134295 + M12.GetHashCode();
            hashCode = hashCode * -1521134295 + M13.GetHashCode();
            hashCode = hashCode * -1521134295 + M20.GetHashCode();
            hashCode = hashCode * -1521134295 + M21.GetHashCode();
            hashCode = hashCode * -1521134295 + M22.GetHashCode();
            hashCode = hashCode * -1521134295 + M23.GetHashCode();
            hashCode = hashCode * -1521134295 + M30.GetHashCode();
            hashCode = hashCode * -1521134295 + M31.GetHashCode();
            hashCode = hashCode * -1521134295 + M32.GetHashCode();
            hashCode = hashCode * -1521134295 + M33.GetHashCode();
            return hashCode;
        }
    }
}
