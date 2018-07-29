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
    }
}
