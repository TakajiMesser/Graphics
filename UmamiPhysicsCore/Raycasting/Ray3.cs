using OpenTK;
using SavoryPhysicsCore.Shapes;
using System;

namespace SavoryPhysicsCore.Raycasting
{
    public struct Ray3
    {
        public Vector3 Origin { get; set; }
        public Vector3 Direction { get; set; }
        public float Distance { get; set; }

        public Ray3(Vector3 origin, Vector3 direction, float distance)
        {
            Origin = origin;
            Direction = direction;
            Distance = distance;
        }

        // From http://mathworld.wolfram.com/Circle-LineIntersection.html
        public bool TryGetCircleIntersection(Circle circle, Vector3 position, out Vector3 intersection)
        {
            var line = new LineSegment(Origin - position, Origin + Direction * Distance - position);

            var dx = line.PointB.X - line.PointA.X;
            var dy = line.PointB.Y - line.PointA.Y;
            var dr2 = dx * dx + dy * dy;
            var d = line.PointA.X * line.PointB.Y - line.PointB.X * line.PointA.Y;

            var discriminant = circle.Radius * circle.Radius * dr2 - d * d;

            if (discriminant >= 0 && (Origin - position).Length - circle.Radius < Distance)
            {
                if (discriminant == 0)
                {
                    intersection = new Vector3()
                    {
                        X = position.X + (float)((d * dy + (dy < 0 ? -1 : 1) * dx * Math.Sqrt(discriminant)) / dr2),
                        Y = position.Y + (float)((-d * dx + Math.Abs(dy) * Math.Sqrt(discriminant)) / dr2),
                        Z = position.Z
                    };

                    return true;
                }
                else
                {
                    var intersectionA = new Vector3()
                    {
                        X = position.X + (float)((d * dy + (dy < 0 ? -1 : 1) * dx * Math.Sqrt(discriminant)) / dr2),
                        Y = position.Y + (float)((-d * dx + Math.Abs(dy) * Math.Sqrt(discriminant)) / dr2),
                        Z = position.Z
                    };

                    var intersectionB = new Vector3()
                    {
                        X = position.X + (float)((d * dy - (dy < 0 ? -1 : 1) * dx * Math.Sqrt(discriminant)) / dr2),
                        Y = position.Y + (float)((-d * dx - Math.Abs(dy) * Math.Sqrt(discriminant)) / dr2),
                        Z = position.Z
                    };

                    intersection = (intersectionA - Origin).Length < (intersectionB - Origin).Length
                        ? intersectionA
                        : intersectionB;

                    return true;
                }
            }
            else
            {
                intersection = Vector3.Zero;
                return false;
            }
        }

        public bool TryGetBoxIntersection(Box box, Vector3 position, out Vector3 intersection)
        {
            var line = new LineSegment(Origin, Origin + Direction * Distance);

            Vector3? horizontalIntersection = null;
            Vector3? verticalIntersection = null;

            if (Origin.X < position.X - box.Width / 2.0f)
            {
                // Consider left side
                var boxLeft = new LineSegment()
                {
                    PointA = new Vector3(position.X - box.Width / 2.0f, position.Y - box.Height / 2.0f, position.Z),
                    PointB = new Vector3(position.X - box.Width / 2.0f, position.Y + box.Height / 2.0f, position.Z)
                };

                if (line.TryGetLineSegmentIntersection(boxLeft, out Vector3 leftIntersection))
                {
                    horizontalIntersection = leftIntersection;
                }
            }
            else if (Origin.X > position.X + box.Width / 2.0f)
            {
                // Consider right side
                var boxRight = new LineSegment()
                {
                    PointA = new Vector3(position.X + box.Width / 2.0f, position.Y - box.Height / 2.0f, position.Z),
                    PointB = new Vector3(position.X + box.Width / 2.0f, position.Y + box.Height / 2.0f, position.Z)
                };

                if (line.TryGetLineSegmentIntersection(boxRight, out Vector3 rightIntersection))
                {
                    horizontalIntersection = rightIntersection;
                }
            }

            /*public float MinX => Center.X - Width / 2.0f;
            public float MaxX => Center.X + Width / 2.0f;
            public float MinY => Center.Y - Height / 2.0f;
            public float MaxY => Center.Y + Height / 2.0f;*/

            if (Origin.Y < position.Y - box.Height / 2.0f)
            {
                // Consider bottom side
                var boxBottom = new LineSegment()
                {
                    PointA = new Vector3(position.X - box.Width / 2.0f, position.Y - box.Height / 2.0f, position.Z),
                    PointB = new Vector3(position.X + box.Width / 2.0f, position.Y - box.Height / 2.0f, position.Z)
                };

                if (line.TryGetLineSegmentIntersection(boxBottom, out Vector3 bottomIntersection))
                {
                    verticalIntersection = bottomIntersection;
                }
            }
            else if (Origin.Y > position.Y + box.Height / 2.0f)
            {
                // Consider top side
                var boxTop = new LineSegment()
                {
                    PointA = new Vector3(position.X - box.Width / 2.0f, position.Y + box.Height / 2.0f, position.Z),
                    PointB = new Vector3(position.X + box.Width / 2.0f, position.Y + box.Height / 2.0f, position.Z)
                };

                if (line.TryGetLineSegmentIntersection(boxTop, out Vector3 topIntersection))
                {
                    verticalIntersection = topIntersection;
                }
            }

            if (horizontalIntersection.HasValue && verticalIntersection.HasValue)
            {
                // TODO - Determine if LengthSquared is okay here... might cause problems at very close distances
                var distanceA = (horizontalIntersection.Value - Origin).Length;
                var distanceB = (verticalIntersection.Value - Origin).Length;

                if (distanceA < distanceB)
                {
                    intersection = horizontalIntersection.Value;
                }
                else
                {
                    intersection = verticalIntersection.Value;
                }

                return true;
            }
            else if (horizontalIntersection.HasValue && !verticalIntersection.HasValue)
            {
                intersection = horizontalIntersection.Value;
                return true;
            }
            else if (!horizontalIntersection.HasValue && verticalIntersection.HasValue)
            {
                intersection = verticalIntersection.Value;
                return true;
            }
            else
            {
                intersection = Vector3.Zero;
                return false;
            }
        }
    }
}
