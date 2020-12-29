using SpiceEngineCore.Utilities;
using System;
using System.Runtime.InteropServices;

namespace SpiceEngineCore.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2 : IEquatable<Vector2>
    {
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; set; }
        public float Y { get; set; }

        public float LengthSquared => X * X + Y * Y;

        public float Length => (float)Math.Sqrt(LengthSquared);

        public float LengthFast => 1.0f / MathExtensions.InverseSqrtFast(LengthSquared);

        public Vector2 Normalized()
        {
            var scale = 1.0f / Length;
            return new Vector2(X * scale, Y * scale);
        }

        public Vector2 NormalizedFast()
        {
            var scale = MathExtensions.InverseSqrtFast(LengthSquared);
            return new Vector2(X * scale, Y * scale);
        }

        public override string ToString() => "<" + X + "," + Y + ">";

        public override bool Equals(object obj) => obj is Vector2 vector && Equals(vector);

        public bool Equals(Vector2 other) => X == other.X && Y == other.Y;

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        public static readonly Vector2 UnitX = new Vector2(1f, 0f);

        public static readonly Vector2 UnitY = new Vector2(0f, 1f);

        public static readonly Vector2 Zero = new Vector2(0f, 0f);

        public static readonly Vector2 One = new Vector2(1f, 1f);

        public static bool operator ==(Vector2 left, Vector2 right) => left.Equals(right);

        public static bool operator !=(Vector2 left, Vector2 right) => !(left == right);

        public static Vector2 operator +(Vector2 left, Vector2 right) => new Vector2(left.X + right.X, left.Y + right.Y);

        public static Vector2 operator -(Vector2 left, Vector2 right) => new Vector2(left.X - right.X, left.Y - right.Y);

        public static Vector2 operator -(Vector2 vector) => new Vector2(-vector.X, -vector.Y);

        public static Vector2 operator *(Vector2 vector, float scale) => new Vector2(vector.X * scale, vector.Y * scale);

        public static Vector2 operator *(Vector2 left, Vector2 right) => new Vector2(left.X * right.X, left.Y * right.Y);

        public static Vector2 operator /(Vector2 left, Vector2 right) => new Vector2(left.X / right.X, left.Y / right.Y);

        public static float Dot(Vector2 left, Vector2 right) => left.X * right.X + left.Y * right.Y;
    }
}
