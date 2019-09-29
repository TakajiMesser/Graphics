using OpenTK;
using System;

namespace SpiceEngineCore.Physics.Raycasting
{
    public struct LineSegment
    {
        public Vector3 PointA { get; set; }
        public Vector3 PointB { get; set; }

        public LineSegment(Vector3 pointA, Vector3 pointB)
        {
            PointA = pointA;
            PointB = pointB;
        }

        public float GetDistanceFromPoint(Vector3 point)
        {
            var vector = PointB - PointA;

            var numerator = Math.Abs(vector.Y * point.X - vector.X * point.Y + PointB.X * PointA.Y - PointB.Y * PointA.X);
            var denominator = (float)Math.Sqrt(vector.Y * vector.Y + vector.X * vector.X);

            return numerator / denominator;
        }

        public bool TryGetLineSegmentIntersection(LineSegment line, out Vector3 intersection)
        {
            var vectorA = PointB - PointA;
            var vectorB = line.PointB - line.PointA;

            var s = (-vectorA.Y * (PointA.X - line.PointA.X) + vectorA.X * (PointA.Y - line.PointA.Y))
                / (-vectorB.X * vectorA.Y + vectorA.X * vectorB.Y);
            var t = (vectorB.X * (PointA.Y - line.PointA.Y) - vectorB.Y * (PointA.X - line.PointA.X))
                / (-vectorB.X * vectorA.Y + vectorA.X * vectorB.Y); ;

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            {
                intersection = new Vector3()
                {
                    X = PointA.X + (t * vectorA.X),
                    Y = PointA.Y + (t * vectorA.Y)
                };

                return true;
            }
            else
            {
                intersection = Vector3.Zero;
                return false;
            }

            //var p = new Vector3(-e.Y, e.X, e.Z);
            //var h = Vector3.Dot(lineA.PointA - lineB.PointA, p) / Vector3.Dot(f, p);
        }
    }
}
