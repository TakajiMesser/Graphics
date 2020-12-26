using SpiceEngineCore.Utilities;
using System;
using System.Runtime.InteropServices;

namespace SpiceEngineCore.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CVector4 : IEquatable<CVector4>
    {
        public CVector4(Vector3 xyz, float w) : this(xyz.X, xyz.Y, xyz.Z, w) { }
        public CVector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public Vector2 Xy
        {
            get => new Vector2(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public Vector3 Xyz
        {
            get => new Vector3(X, Y, Z);
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        public float LengthSquared => X * X + Y * Y + Z * Z + W * W;

        public float Length => (float)Math.Sqrt(LengthSquared);

        public float LengthFast => 1.0f / MathExtensions.InverseSqrtFast(LengthSquared);

        public CVector4 Normalized()
        {
            var scale = 1.0f / Length;
            return new CVector4(X * scale, Y * scale, Z * scale, W * scale);
        }

        public CVector4 NormalizedFast()
        {
            var scale = MathExtensions.InverseSqrtFast(LengthSquared);
            return new CVector4(X * scale, Y * scale, Z * scale, W * scale);
        }

        public override string ToString() => "<" + X + "," + Y + "," + Z + "," + W + ">";

        public override bool Equals(object obj) => obj is CVector4 vector && Equals(vector);

        public bool Equals(CVector4 other) => X == other.X && Y == other.Y && Z == other.Z && W == other.W;

        public override int GetHashCode()
        {
            var hashCode = 707706286;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            hashCode = hashCode * -1521134295 + W.GetHashCode();
            return hashCode;
        }

        public static readonly CVector4 UnitX = new CVector4(1f, 0f, 0f, 0f);

        public static readonly CVector4 UnitY = new CVector4(0f, 1f, 0f, 0f);

        public static readonly CVector4 UnitZ = new CVector4(0f, 0f, 1f, 0f);

        public static readonly CVector4 UnitW = new CVector4(0f, 0f, 0f, 1f);

        public static readonly CVector4 Zero = new CVector4(0f, 0f, 0f, 0f);

        public static readonly CVector4 One = new CVector4(1f, 1f, 1f, 1f);

        public static bool operator ==(CVector4 left, CVector4 right) => left.Equals(right);

        public static bool operator !=(CVector4 left, CVector4 right) => !(left == right);

        public static CVector4 operator +(CVector4 left, CVector4 right) => new CVector4(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);

        public static CVector4 operator -(CVector4 left, CVector4 right) => new CVector4(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);

        public static CVector4 operator -(CVector4 vector) => new CVector4(-vector.X, -vector.Y, -vector.Z, -vector.W);

        public static CVector4 operator *(CVector4 vector, float scale) => new CVector4(vector.X * scale, vector.Y * scale, vector.Z * scale, vector.W * scale);

        public static CVector4 operator *(CVector4 left, CVector4 right) => new CVector4(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);

        public static CVector4 operator *(Quaternion quaternion, CVector4 vector) => Transform(vector, quaternion);

        public static CVector4 operator *(CVector4 vector, Matrix4 matrix) => TransformRow(vector, matrix);

        public static CVector4 operator *(Matrix4 matrix, CVector4 vector) => TransformColumn(vector, matrix);

        public static CVector4 operator *(CVector4 vector, CMatrix4 matrix) => new CVector4(
            (vector.X * matrix.M00) + (vector.Y * matrix.M10) + (vector.Z * matrix.M20) + (vector.W * matrix.M30),
            (vector.X * matrix.M01) + (vector.Y * matrix.M11) + (vector.Z * matrix.M21) + (vector.W * matrix.M31),
            (vector.X * matrix.M02) + (vector.Y * matrix.M12) + (vector.Z * matrix.M22) + (vector.W * matrix.M32),
            (vector.X * matrix.M03) + (vector.Y * matrix.M13) + (vector.Z * matrix.M23) + (vector.W * matrix.M33));

        public static CVector4 operator *(CMatrix4 matrix, CVector4 vector) => new CVector4(
            (matrix.M00 * vector.X) + (matrix.M01 * vector.Y) + (matrix.M02 * vector.Z) + (matrix.M03 * vector.W),
            (matrix.M10 * vector.X) + (matrix.M11 * vector.Y) + (matrix.M12 * vector.Z) + (matrix.M13 * vector.W),
            (matrix.M20 * vector.X) + (matrix.M21 * vector.Y) + (matrix.M22 * vector.Z) + (matrix.M23 * vector.W),
            (matrix.M30 * vector.X) + (matrix.M31 * vector.Y) + (matrix.M32 * vector.Z) + (matrix.M33 * vector.W));

        public static CVector4 operator /(CVector4 left, CVector4 right) => new CVector4(left.X / right.X, left.Y / right.Y, left.Z / right.Z, left.W / right.W);

        public static float Dot(CVector4 left, CVector4 right) => left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.W * right.W;

        public static CVector4 Transform(Quaternion quaternion, CVector4 vector)
        {
            var vectorQuat = new Quaternion(vector.X, vector.Y, vector.Z, vector.W);
            var invertedQuat = quaternion.Inverted();
            var rotatedQuat = (quaternion * vectorQuat) * invertedQuat;

            return new CVector4(rotatedQuat.X, rotatedQuat.Y, rotatedQuat.Z, rotatedQuat.W);
        }

        public static CVector4 Transform(CVector4 vector, Quaternion quaternion)
        {
            Quaternion v = new Quaternion(vector.X, vector.Y, vector.Z, vector.W);
            Quaternion.Invert(in quaternion, out Quaternion i);
            Quaternion.Multiply(in quaternion, in v, out Quaternion t);
            Quaternion.Multiply(in t, in i, out v);

            return new CVector4(v.X, v.Y, v.Z, v.W);
        }

        public static CVector4 TransformRow(CVector4 vector, Matrix4 matrix) => new CVector4(
            (vector.X * matrix.Row0.X) + (vector.Y * matrix.Row1.X) + (vector.Z * matrix.Row2.X) + (vector.W * matrix.Row3.X),
            (vector.X * matrix.Row0.Y) + (vector.Y * matrix.Row1.Y) + (vector.Z * matrix.Row2.Y) + (vector.W * matrix.Row3.Y),
            (vector.X * matrix.Row0.Z) + (vector.Y * matrix.Row1.Z) + (vector.Z * matrix.Row2.Z) + (vector.W * matrix.Row3.Z),
            (vector.X * matrix.Row0.W) + (vector.Y * matrix.Row1.W) + (vector.Z * matrix.Row2.W) + (vector.W * matrix.Row3.W));

        public static CVector4 TransformColumn(CVector4 vector, Matrix4 matrix) => new CVector4(
            (matrix.Row0.X * vector.X) + (matrix.Row0.Y * vector.Y) + (matrix.Row0.Z * vector.Z) + (matrix.Row0.W * vector.W),
            (matrix.Row1.X * vector.X) + (matrix.Row1.Y * vector.Y) + (matrix.Row1.Z * vector.Z) + (matrix.Row1.W * vector.W),
            (matrix.Row2.X * vector.X) + (matrix.Row2.Y * vector.Y) + (matrix.Row2.Z * vector.Z) + (matrix.Row2.W * vector.W),
            (matrix.Row3.X * vector.X) + (matrix.Row3.Y * vector.Y) + (matrix.Row3.Z * vector.Z) + (matrix.Row3.W * vector.W));
    }
}
