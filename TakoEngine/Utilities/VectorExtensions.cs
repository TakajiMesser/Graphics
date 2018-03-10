using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TakoEngine.Utilities
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
