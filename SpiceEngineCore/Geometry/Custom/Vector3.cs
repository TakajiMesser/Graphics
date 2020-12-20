using SpiceEngineCore.Utilities;
using System;
using System.Runtime.InteropServices;

namespace SpiceEngineCore.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CVector3 : IEquatable<CVector3>
    {
        public CVector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public CVector2 Xy
        {
            get => new CVector2(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public float LengthSquared => X * X + Y * Y + Z * Z;

        public float Length => (float)Math.Sqrt(LengthSquared);

        public float LengthFast => 1.0f / MathExtensions.InverseSqrtFast(LengthSquared);

        public CVector3 Normalized()
        {
            var scale = 1.0f / Length;
            return new CVector3(X * scale, Y * scale, Z * scale);
        }

        public CVector3 NormalizedFast()
        {
            var scale = MathExtensions.InverseSqrtFast(LengthSquared);
            return new CVector3(X * scale, Y * scale, Z * scale);
        }

        public override string ToString() => "<" + X + "," + Y + "," + Z + ">";

        public override bool Equals(object obj) => obj is CVector3 vector && Equals(vector);

        public bool Equals(CVector3 other) => X == other.X && Y == other.Y && Z == other.Z;

        public override int GetHashCode()
        {
            var hashCode = -307843816;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            return hashCode;
        }

        public static readonly CVector3 UnitX = new CVector3(1f, 0f, 0f);

        public static readonly CVector3 UnitY = new CVector3(0f, 1f, 0f);

        public static readonly CVector3 UnitZ = new CVector3(0f, 0f, 1f);

        public static readonly CVector3 Zero = new CVector3(0f, 0f, 0f);

        public static readonly CVector3 One = new CVector3(1f, 1f, 1f);

        public static bool operator ==(CVector3 left, CVector3 right) => left.Equals(right);

        public static bool operator !=(CVector3 left, CVector3 right) => !(left == right);

        public static CVector3 operator +(CVector3 left, CVector3 right) => new CVector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

        public static CVector3 operator -(CVector3 left, CVector3 right) => new CVector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

        public static CVector3 operator -(CVector3 vector) => new CVector3(-vector.X, -vector.Y, -vector.Z);

        public static CVector3 operator *(CVector3 vector, float scale) => new CVector3(vector.X * scale, vector.Y * scale, vector.Z * scale);

        public static CVector3 operator *(CVector3 left, CVector3 right) => new CVector3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);

        public static CVector3 operator /(CVector3 vector, float scale) => new CVector3(vector.X / scale, vector.Y / scale, vector.Z / scale);

        public static CVector3 operator /(CVector3 left, CVector3 right) => new CVector3(left.X / right.X, left.Y / right.Y, left.Z / right.Z);

        public static CVector3 operator *(float scale, CVector3 vector) => new CVector3(scale * vector.X, scale * vector.Y, scale * vector.Z);

        public static CVector3 operator *(CQuaternion quaternion, CVector3 vector)
        {
            var vectorQuat = new CQuaternion(vector.X, vector.Y, vector.Z, 1.0f);
            var invertedQuat = quaternion.Inverted();
            var rotatedQuat = (quaternion * vectorQuat) * invertedQuat;

            return new CVector3(rotatedQuat.X, rotatedQuat.Y, rotatedQuat.Z);
        }

        public static float Dot(CVector3 left, CVector3 right) => left.X * right.X + left.Y * right.Y + left.Z * right.Z;

        public static CVector3 Cross(CVector3 left, CVector3 right) => new CVector3(left.Y * right.Z - left.Z * right.Y, left.Z * right.X - left.X * right.Z, left.X * right.Y - left.Y * right.X);
    }
}
