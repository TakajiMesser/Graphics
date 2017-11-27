using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.ThreeDimensional
{
    public struct Vector3
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double Magnitude => Math.Sqrt(Math.Pow(X, 2.0) + Math.Pow(Y, 2.0) + Math.Pow(Z, 2.0));

        public static double DotProduct(Vector3 vectorA, Vector3 vectorB)
        {
            return vectorA.X * vectorB.X + vectorA.Y * vectorB.Y + vectorA.Z * vectorB.Z;
        }

        public static Vector3 CrossProduct(Vector3 vectorA, Vector3 vectorB)
        {
            return new Vector3()
            {
                X = vectorA.Y * vectorB.Z - vectorA.Z * vectorB.Y,
                Y = vectorA.Z * vectorB.X - vectorA.X * vectorB.Z,
                Z = vectorA.X * vectorB.Y - vectorA.Y * vectorB.X
            };
        }

        /// <summary>
        /// Determines the angle between two vectors.
        /// </summary>
        /// <param name="vectorA"></param>
        /// <param name="vectorB"></param>
        /// <returns>The measure of the angle, in radians.</returns>
        public static double AngleBetween(Vector3 vectorA, Vector3 vectorB)
        {
            return Math.Acos(DotProduct(vectorA, vectorB) / (vectorA.Magnitude * vectorB.Magnitude));
        }

        public static Vector3 operator +(Vector3 vectorA, Vector3 vectorB)
        {
            return new Vector3(vectorA.X + vectorB.X, vectorA.Y + vectorB.Y, vectorA.Z + vectorB.Z);
        }
    }
}
