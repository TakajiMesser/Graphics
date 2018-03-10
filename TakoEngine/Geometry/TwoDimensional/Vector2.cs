using Graphics.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Geometry.TwoDimensional
{
    public struct Vector2
    {
        public static Vector2 Zero => new Vector2(0, 0);
        public static Vector2 Left => new Vector2(-1, 0);
        public static Vector2 Up => new Vector2(0, 1);
        public static Vector2 Right => new Vector2(1, 0);
        public static Vector2 Down => new Vector2(0, -1);

        public double X { get; set; }
        public double Y { get; set; }

        public double Magnitude => Math.Sqrt(Math.Pow(X, 2.0) + Math.Pow(Y, 2.0));
        public double Angle => (UnitConversions.RadiansToDegrees(Math.Atan2(Y, X)) + 360) % 360;
        public Vector2 Normal => this / Magnitude;

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static double DotProduct(Vector2 vectorA, Vector2 vectorB) => vectorA.X * vectorB.X + vectorA.Y * vectorB.Y;

        /// <summary>
        /// Determines the angle between two vectors.
        /// </summary>
        /// <param name="vectorA"></param>
        /// <param name="vectorB"></param>
        /// <returns>The measure of the angle, in radians.</returns>
        public static double AngleBetween(Vector2 vectorA, Vector2 vectorB) => Math.Acos(DotProduct(vectorA, vectorB) / (vectorA.Magnitude * vectorB.Magnitude));

        public static Vector2 operator +(Vector2 vectorA, Vector2 vectorB) => new Vector2(vectorA.X + vectorB.X, vectorA.Y + vectorB.Y);

        public static Vector2 operator -(Vector2 vectorA, Vector2 vectorB) => vectorA + (-vectorB);

        public static Vector2 operator -(Vector2 vector) => new Vector2(-vector.X, -vector.Y);

        public static Vector2 operator *(Vector2 vector, double scalar) => new Vector2(vector.X * scalar, vector.Y * scalar);

        public static Vector2 operator /(Vector2 vector, double scalar) => new Vector2(vector.X / scalar, vector.Y / scalar);
    }
}
