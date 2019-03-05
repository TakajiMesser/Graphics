using OpenTK;
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
    }
}
