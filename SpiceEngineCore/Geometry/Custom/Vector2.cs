using SpiceEngineCore.Utilities;
using System;
using System.Runtime.InteropServices;

namespace SpiceEngineCore.Geometry.Vectors
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CVector2 : IEquatable<CVector2>
    {
        public CVector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; private set; }
        public float Y { get; private set; }

        public float LengthSquared => X * X + Y * Y;

        public float Length => (float)Math.Sqrt(LengthSquared);

        public float LengthFast => 1.0f / MathExtensions.InverseSqrtFast(LengthSquared);

        public CVector2 Normalized()
        {
            var scale = 1.0f / Length;
            return new CVector2(X * scale, Y * scale);
        }

        public CVector2 NormalizedFast()
        {
            var scale = MathExtensions.InverseSqrtFast(LengthSquared);
            return new CVector2(X * scale, Y * scale);
        }

        public override string ToString() => "<" + X + "," + Y + ">";

        public override bool Equals(object obj) => obj is CVector2 vector && Equals(vector);

        public bool Equals(CVector2 other) => X == other.X && Y == other.Y;

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        public static readonly CVector2 UnitX = new CVector2(1f, 0f);

        public static readonly CVector2 UnitY = new CVector2(0f, 1f);

        public static readonly CVector2 Zero = new CVector2(0f, 0f);

        public static readonly CVector2 One = new CVector2(1f, 1f);

        public static bool operator ==(CVector2 left, CVector2 right) => left.Equals(right);

        public static bool operator !=(CVector2 left, CVector2 right) => !(left == right);

        public static CVector2 operator +(CVector2 left, CVector2 right) => new CVector2(left.X + right.X, left.Y + right.Y);

        public static CVector2 operator -(CVector2 left, CVector2 right) => new CVector2(left.X - right.X, left.Y - right.Y);

        public static CVector2 operator -(CVector2 vector) => new CVector2(-vector.X, -vector.Y);

        public static CVector2 operator *(CVector2 vector, float scale) => new CVector2(vector.X * scale, vector.Y * scale);

        public static CVector2 operator *(CVector2 left, CVector2 right) => new CVector2(left.X * right.X, left.Y * right.Y);

        public static CVector2 operator /(CVector2 left, CVector2 right) => new CVector2(left.X / right.X, left.Y / right.Y);

        public static float Dot(CVector2 left, CVector2 right) => left.X * right.X + left.Y * right.Y;
    }
}
