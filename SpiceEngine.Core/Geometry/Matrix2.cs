using System;
using System.Runtime.InteropServices;

namespace SpiceEngineCore.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix2 : IEquatable<Matrix2>
    {
        public Matrix2(float m00, float m01, float m10, float m11)
        {
            M00 = m00;
            M01 = m01;
            M10 = m10;
            M11 = m11;
        }

        public float M00 { get; set; }
        public float M01 { get; set; }
        public float M10 { get; set; }
        public float M11 { get; set; }

        public Vector2 Row0 => new Vector2(M00, M01);
        public Vector2 Row1 => new Vector2(M10, M11);

        public Vector2 Column0 => new Vector2(M00, M10);
        public Vector2 Column1 => new Vector2(M01, M11);

        public float Determinant => M00 * M11 - M01 * M10;

        public float Trace => M00 + M11;

        public Matrix2 Transposed() => new Matrix2(M00, M10, M01, M11);

        public Matrix2 Inverted()
        {
            var determinant = Determinant;
            if (determinant == 0f) throw new InvalidOperationException("Cannot invert a singular matrix");

            var inverseDeterminant = 1f / determinant;

            return new Matrix2(M11 * inverseDeterminant, M01 * inverseDeterminant, M10 * inverseDeterminant, M00 * inverseDeterminant);
        }

        public override string ToString() => "|" + M00 + "," + M01 + "|"
            + Environment.NewLine + "|" + M10 + "," + M11 + "|";

        public override bool Equals(object obj) => obj is Matrix2 matrix && Equals(matrix);

        public bool Equals(Matrix2 other) => M00 == other.M00 && M01 == other.M01 && M10 == other.M10 && M11 == other.M11;

        public override int GetHashCode()
        {
            var hashCode = 1944975882;
            hashCode = hashCode * -1521134295 + M00.GetHashCode();
            hashCode = hashCode * -1521134295 + M01.GetHashCode();
            hashCode = hashCode * -1521134295 + M10.GetHashCode();
            hashCode = hashCode * -1521134295 + M11.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Matrix2 left, Matrix2 right) => left.Equals(right);

        public static bool operator !=(Matrix2 left, Matrix2 right) => !(left == right);

        public static Matrix2 operator +(Matrix2 left, Matrix2 right) => new Matrix2(left.M00 + right.M00, left.M01 + right.M01, left.M10 + right.M10, left.M11 + right.M11);

        public static Matrix2 operator -(Matrix2 left, Matrix2 right) => new Matrix2(left.M00 - right.M00, left.M01 - right.M01, left.M10 - right.M10, left.M11 - right.M11);

        public static Matrix2 operator *(Matrix2 left, Matrix2 right) => new Matrix2(left.M00 * right.M00 + left.M01 * right.M10, left.M00 * right.M01 + left.M01 * right.M11, left.M10 * right.M00 + left.M11 * right.M10, left.M10 * right.M01 + left.M11 * right.M11);
    }
}
