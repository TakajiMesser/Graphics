using System;
using System.Runtime.InteropServices;

namespace SpiceEngineCore.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CMatrix2 : IEquatable<CMatrix2>
    {
        public CMatrix2(float m00, float m01, float m10, float m11)
        {
            M00 = m00;
            M01 = m01;
            M10 = m10;
            M11 = m11;
        }

        public float M00 { get; private set; }
        public float M01 { get; private set; }
        public float M10 { get; private set; }
        public float M11 { get; private set; }

        public float Determinant => M00 * M11 - M01 * M10;

        public float Trace => M00 + M11;

        public CMatrix2 Transposed() => new CMatrix2(M00, M10, M01, M11);

        public CMatrix2 Inverted()
        {
            var determinant = Determinant;
            if (determinant == 0f) throw new InvalidOperationException("Cannot invert a singular matrix");

            var inverseDeterminant = 1f / determinant;

            return new CMatrix2(M11 * inverseDeterminant, M01 * inverseDeterminant, M10 * inverseDeterminant, M00 * inverseDeterminant);
        }

        public override string ToString() => "|" + M00 + "," + M01 + "|"
            + Environment.NewLine + "|" + M10 + "," + M11 + "|";

        public override bool Equals(object obj) => obj is CMatrix2 matrix && Equals(matrix);

        public bool Equals(CMatrix2 other) => M00 == other.M00 && M01 == other.M01 && M10 == other.M10 && M11 == other.M11;

        public override int GetHashCode()
        {
            var hashCode = 1944975882;
            hashCode = hashCode * -1521134295 + M00.GetHashCode();
            hashCode = hashCode * -1521134295 + M01.GetHashCode();
            hashCode = hashCode * -1521134295 + M10.GetHashCode();
            hashCode = hashCode * -1521134295 + M11.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(CMatrix2 left, CMatrix2 right) => left.Equals(right);

        public static bool operator !=(CMatrix2 left, CMatrix2 right) => !(left == right);

        public static CMatrix2 operator +(CMatrix2 left, CMatrix2 right) => new CMatrix2(left.M00 + right.M00, left.M01 + right.M01, left.M10 + right.M10, left.M11 + right.M11);

        public static CMatrix2 operator -(CMatrix2 left, CMatrix2 right) => new CMatrix2(left.M00 - right.M00, left.M01 - right.M01, left.M10 - right.M10, left.M11 - right.M11);

        public static CMatrix2 operator *(CMatrix2 left, CMatrix2 right) => new CMatrix2(left.M00 * right.M00 + left.M01 * right.M10, left.M00 * right.M01 + left.M01 * right.M11, left.M10 * right.M00 + left.M11 * right.M10, left.M10 * right.M01 + left.M11 * right.M11);
    }
}
