using SpiceEngineCore.Utilities;
using System;
using System.Runtime.InteropServices;

namespace SpiceEngineCore.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2i : IEquatable<Vector2i>
    {
        public Vector2i(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public float LengthSquared => X * X + Y * Y;

        public float Length => (float)Math.Sqrt(LengthSquared);

        public float LengthFast => 1.0f / MathExtensions.InverseSqrtFast(LengthSquared);

        public override string ToString() => "<" + X + "," + Y + ">";

        public override bool Equals(object obj) => obj is Vector2i vector && Equals(vector);

        public bool Equals(Vector2i other) => X == other.X && Y == other.Y;

        public override int GetHashCode()
        {
            int hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        public static readonly Vector2i UnitX = new Vector2i(1, 0);

        public static readonly Vector2i UnitY = new Vector2i(0, 1);

        public static readonly Vector2i Zero = new Vector2i(0, 0);

        public static readonly Vector2i One = new Vector2i(1, 1);

        public static bool operator ==(Vector2i left, Vector2i right) => left.Equals(right);

        public static bool operator !=(Vector2i left, Vector2i right) => !(left == right);

        public static Vector2i operator +(Vector2i left, Vector2i right) => new Vector2i(left.X + right.X, left.Y + right.Y);

        public static Vector2i operator -(Vector2i left, Vector2i right) => new Vector2i(left.X - right.X, left.Y - right.Y);

        public static Vector2i operator -(Vector2i vector) => new Vector2i(-vector.X, -vector.Y);

        public static Vector2i operator *(Vector2i vector, int scale) => new Vector2i(vector.X * scale, vector.Y * scale);

        public static Vector2i operator *(Vector2i left, Vector2i right) => new Vector2i(left.X * right.X, left.Y * right.Y);

        public static Vector2i operator /(Vector2i left, Vector2i right) => new Vector2i(left.X / right.X, left.Y / right.Y);

        public static float Dot(Vector2i left, Vector2i right) => left.X * right.X + left.Y * right.Y;
    }
}
