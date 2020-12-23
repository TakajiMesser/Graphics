using SpiceEngineCore.Utilities;
using System;
using System.Runtime.InteropServices;

namespace SpiceEngineCore.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3 : IEquatable<Vector3>
    {
        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector2 Xy
        {
            get => new Vector2(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public float LengthSquared => X * X + Y * Y + Z * Z;

        public float Length => (float)Math.Sqrt(LengthSquared);

        public float LengthFast => 1.0f / MathExtensions.InverseSqrtFast(LengthSquared);

        public Vector3 Normalized()
        {
            var scale = 1.0f / Length;
            return new Vector3(X * scale, Y * scale, Z * scale);
        }

        public Vector3 NormalizedFast()
        {
            var scale = MathExtensions.InverseSqrtFast(LengthSquared);
            return new Vector3(X * scale, Y * scale, Z * scale);
        }

        public override string ToString() => "<" + X + "," + Y + "," + Z + ">";

        public override bool Equals(object obj) => obj is Vector3 vector && Equals(vector);

        public bool Equals(Vector3 other) => X == other.X && Y == other.Y && Z == other.Z;

        public override int GetHashCode()
        {
            var hashCode = -307843816;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            return hashCode;
        }

        public static readonly Vector3 UnitX = new Vector3(1f, 0f, 0f);

        public static readonly Vector3 UnitY = new Vector3(0f, 1f, 0f);

        public static readonly Vector3 UnitZ = new Vector3(0f, 0f, 1f);

        public static readonly Vector3 Zero = new Vector3(0f, 0f, 0f);

        public static readonly Vector3 One = new Vector3(1f, 1f, 1f);

        public static bool operator ==(Vector3 left, Vector3 right) => left.Equals(right);

        public static bool operator !=(Vector3 left, Vector3 right) => !(left == right);

        public static Vector3 operator +(Vector3 left, Vector3 right) => new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

        public static Vector3 operator -(Vector3 left, Vector3 right) => new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

        public static Vector3 operator -(Vector3 vector) => new Vector3(-vector.X, -vector.Y, -vector.Z);

        public static Vector3 operator *(Vector3 vector, float scale) => new Vector3(vector.X * scale, vector.Y * scale, vector.Z * scale);

        public static Vector3 operator *(Vector3 left, Vector3 right) => new Vector3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);

        public static Vector3 operator /(Vector3 vector, float scale) => new Vector3(vector.X / scale, vector.Y / scale, vector.Z / scale);

        public static Vector3 operator /(Vector3 left, Vector3 right) => new Vector3(left.X / right.X, left.Y / right.Y, left.Z / right.Z);

        public static Vector3 operator *(float scale, Vector3 vector) => new Vector3(scale * vector.X, scale * vector.Y, scale * vector.Z);

        public static Vector3 operator *(CQuaternion quaternion, Vector3 vector)
        {
            var vectorQuat = new CQuaternion(vector.X, vector.Y, vector.Z, 1.0f);
            var invertedQuat = quaternion.Inverted();
            var rotatedQuat = (quaternion * vectorQuat) * invertedQuat;

            return new Vector3(rotatedQuat.X, rotatedQuat.Y, rotatedQuat.Z);
        }

        public static float Dot(Vector3 left, Vector3 right) => left.X * right.X + left.Y * right.Y + left.Z * right.Z;

        public static Vector3 Cross(Vector3 left, Vector3 right) => new Vector3(left.Y * right.Z - left.Z * right.Y, left.Z * right.X - left.X * right.Z, left.X * right.Y - left.Y * right.X);
    }
}
