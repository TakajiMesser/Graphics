using Graphics.Physics.Collision;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Physics.Raycasting
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

        public bool TryGetBoxIntersection(BoundingBox box, out Vector3 intersection)
        {
            var line = new LineSegment(Origin, Origin + Direction * Distance);

            Vector3? horizontalIntersection = null;
            Vector3? verticalIntersection = null;

            if (Origin.X < box.MinX)
            {
                // Consider left side
                var boxLeft = new LineSegment()
                {
                    PointA = new Vector3(box.MinX, box.MinY, box.Center.Z),
                    PointB = new Vector3(box.MinX, box.MaxY, box.Center.Z)
                };

                if (line.TryGetLineSegmentIntersection(boxLeft, out Vector3 leftIntersection))
                {
                    horizontalIntersection = leftIntersection;
                }
            }
            else if (Origin.X > box.MaxX)
            {
                // Consider right side
                var boxRight = new LineSegment()
                {
                    PointA = new Vector3(box.MaxX, box.MinY, box.Center.Z),
                    PointB = new Vector3(box.MaxX, box.MaxY, box.Center.Z)
                };

                if (line.TryGetLineSegmentIntersection(boxRight, out Vector3 rightIntersection))
                {
                    horizontalIntersection = rightIntersection;
                }
            }

            if (Origin.Y < box.MinY)
            {
                // Consider bottom side
                var boxBottom = new LineSegment()
                {
                    PointA = new Vector3(box.MinX, box.MinY, box.Center.Z),
                    PointB = new Vector3(box.MaxX, box.MinY, box.Center.Z)
                };

                if (line.TryGetLineSegmentIntersection(boxBottom, out Vector3 bottomIntersection))
                {
                    verticalIntersection = bottomIntersection;
                }
            }
            else if (Origin.Y > box.MaxY)
            {
                // Consider top side
                var boxTop = new LineSegment()
                {
                    PointA = new Vector3(box.MinX, box.MaxY, box.Center.Z),
                    PointB = new Vector3(box.MaxX, box.MaxY, box.Center.Z)
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
