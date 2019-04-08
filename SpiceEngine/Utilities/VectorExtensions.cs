using OpenTK;
using OpenTK.Graphics;
using System;

namespace SpiceEngine.Utilities
{
    public static class VectorExtensions
    {
        public static float AngleBetween(this Vector2 vectorA, Vector2 vectorB)
        {
            var cosAngle = Vector2.Dot(vectorA, vectorB) / (vectorA.Length * vectorB.Length);
            return (float)Math.Acos(cosAngle);
        }

        public static float AngleBetween(this Vector3 vectorA, Vector3 vectorB)
        {
            var cosAngle = Vector3.Dot(vectorA, vectorB) / (vectorA.Length * vectorB.Length);
            return (float)Math.Acos(cosAngle);
        }

        public static bool IsSignificant(this Vector2 vector) => vector.X >= MathExtensions.EPSILON || vector.X <= -MathExtensions.EPSILON
            || vector.Y >= MathExtensions.EPSILON || vector.Y <= -MathExtensions.EPSILON;

        public static bool IsSignificant(this Vector3 vector) => vector.X >= MathExtensions.EPSILON || vector.X <= -MathExtensions.EPSILON
            || vector.Y >= MathExtensions.EPSILON || vector.Y <= -MathExtensions.EPSILON
            || vector.Z >= MathExtensions.EPSILON || vector.Z <= -MathExtensions.EPSILON;

        public static Color4 ToColor4(this Vector4 vector) => new Color4(vector.X, vector.Y, vector.Z, vector.W);

        public static Vector3 ToRadians(this Vector3 vector) => new Vector3(UnitConversions.ToRadians(vector.X), UnitConversions.ToRadians(vector.Y), UnitConversions.ToRadians(vector.Z));

        public static Vector3 ToDegrees(this Vector3 vector) => new Vector3(UnitConversions.ToDegrees(vector.X), UnitConversions.ToDegrees(vector.Y), UnitConversions.ToDegrees(vector.Z));
    }
}
