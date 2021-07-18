using System;
using System.Runtime.InteropServices;

namespace SpiceEngineCore.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix3 : IEquatable<Matrix3>
    {
        public Matrix3(float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22)
        {
            M00 = m00;
            M01 = m01;
            M02 = m02;
            M10 = m10;
            M11 = m11;
            M12 = m12;
            M20 = m20;
            M21 = m21;
            M22 = m22;
        }

        public float M00 { get; set; }
        public float M01 { get; set; }
        public float M02 { get; set; }
        public float M10 { get; set; }
        public float M11 { get; set; }
        public float M12 { get; set; }
        public float M20 { get; set; }
        public float M21 { get; set; }
        public float M22 { get; set; }

        public Vector3 Row0 => new Vector3(M00, M01, M02);
        public Vector3 Row1 => new Vector3(M10, M11, M12);
        public Vector3 Row2 => new Vector3(M20, M21, M22);

        public Vector3 Column0 => new Vector3(M00, M10, M20);
        public Vector3 Column1 => new Vector3(M01, M11, M21);
        public Vector3 Column2 => new Vector3(M02, M12, M22);

        public float Determinant => M00 * M11 * M22 + M01 * M12 * M20 + M02 * M10 * M21 - M02 * M11 * M20 - M00 * M12 * M21 - M01 * M10 * M22;

        public float Trace => M00 + M11 + M22;

        public Matrix3 Transposed() => new Matrix3(M00, M10, M20, M01, M11, M21, M02, M12, M22);

        public override string ToString() => "|" + M00 + "," + M01 + "," + M02 + "|"
            + Environment.NewLine + "|" + M10 + "," + M11 + "," + M12 + "|"
            + Environment.NewLine + "|" + M20 + "," + M21 + "," + M22 + "|";

        public override bool Equals(object obj) => obj is Matrix3 matrix && Equals(matrix);

        public bool Equals(Matrix3 other) => M00 == other.M00
            && M01 == other.M01
            && M02 == other.M02
            && M10 == other.M10
            && M11 == other.M11
            && M12 == other.M12
            && M20 == other.M20
            && M21 == other.M21
            && M22 == other.M22;

        public override int GetHashCode()
        {
            var hashCode = -378756394;
            hashCode = hashCode * -1521134295 + M00.GetHashCode();
            hashCode = hashCode * -1521134295 + M01.GetHashCode();
            hashCode = hashCode * -1521134295 + M02.GetHashCode();
            hashCode = hashCode * -1521134295 + M10.GetHashCode();
            hashCode = hashCode * -1521134295 + M11.GetHashCode();
            hashCode = hashCode * -1521134295 + M12.GetHashCode();
            hashCode = hashCode * -1521134295 + M20.GetHashCode();
            hashCode = hashCode * -1521134295 + M21.GetHashCode();
            hashCode = hashCode * -1521134295 + M22.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Matrix3 left, Matrix3 right) => left.Equals(right);

        public static bool operator !=(Matrix3 left, Matrix3 right) => !(left == right);

        public static Matrix3 operator +(Matrix3 left, Matrix3 right) => new Matrix3(left.M00 + right.M00, left.M01 + right.M01, left.M02 + right.M02, left.M10 + right.M10, left.M11 + right.M11, left.M12 + right.M12, left.M20 + right.M20, left.M21 + right.M21, left.M22 + right.M22);

        public static Matrix3 operator -(Matrix3 left, Matrix3 right) => new Matrix3(left.M00 - right.M00, left.M01 - right.M01, left.M02 - right.M02, left.M10 - right.M10, left.M11 - right.M11, left.M12 - right.M12, left.M20 - right.M20, left.M21 - right.M21, left.M22 - right.M22);

        public static Matrix3 operator *(Matrix3 left, Matrix3 right) => new Matrix3(left.M00 * right.M00 + left.M01 * right.M10 + left.M02 * right.M20, left.M00 * right.M01 + left.M01 * right.M11 + left.M02 * right.M21, left.M00 * right.M02 + left.M01 * right.M12 + left.M02 * right.M22, left.M10 * right.M00 + left.M11 * right.M10 + left.M12 * right.M20, left.M10 * right.M01 + left.M11 * right.M11 + left.M12 * right.M21, left.M10 * right.M02 + left.M11 * right.M12 + left.M12 * right.M22, left.M20 * right.M00 + left.M21 * right.M10 + left.M22 * right.M20, left.M20 * right.M01 + left.M21 * right.M11 + left.M22 * right.M21, left.M20 * right.M02 + left.M21 * right.M12 + left.M22 * right.M22);
    }
}
