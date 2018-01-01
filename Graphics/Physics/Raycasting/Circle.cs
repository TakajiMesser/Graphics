using Graphics.Physics.Collision;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Physics.Raycasting
{
    public struct Circle
    {
        public Vector3 Origin { get; set; }
        public float Radius { get; set; }

        public Circle(Vector3 origin, float radius)
        {
            Origin = origin;
            Radius = radius;
        }

        public bool TryGetBoxIntersection(BoundingBox box, out Vector3 intersection)
        {
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

                if (boxLeft.GetDistanceFromPoint(Origin) <= Radius)
                {
                    if (Origin.Y >= box.MaxY)
                    {
                        horizontalIntersection = new Vector3(box.MinX, box.MaxY, box.Center.Z);
                    }
                    else if (Origin.Y <= box.MinY)
                    {
                        horizontalIntersection = new Vector3(box.MinX, box.MinY, box.Center.Z);
                    }
                    else
                    {
                        horizontalIntersection = new Vector3(box.MinX, Origin.Y, box.Center.Z);
                    }
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

                if (boxRight.GetDistanceFromPoint(Origin) <= Radius)
                {
                    if (Origin.Y >= box.MaxY)
                    {
                        horizontalIntersection = new Vector3(box.MaxX, box.MaxY, box.Center.Z);
                    }
                    else if (Origin.Y <= box.MinY)
                    {
                        horizontalIntersection = new Vector3(box.MaxX, box.MinY, box.Center.Z);
                    }
                    else
                    {
                        horizontalIntersection = new Vector3(box.MaxX, Origin.Y, box.Center.Z);
                    }
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

                if (boxBottom.GetDistanceFromPoint(Origin) <= Radius)
                {
                    if (Origin.X >= box.MaxX)
                    {
                        verticalIntersection = new Vector3(box.MaxX, box.MinY, box.Center.Z);
                    }
                    else if (Origin.X <= box.MinX)
                    {
                        verticalIntersection = new Vector3(box.MinX, box.MinY, box.Center.Z);
                    }
                    else
                    {
                        verticalIntersection = new Vector3(Origin.X, box.MinY, box.Center.Z);
                    }
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

                if (boxTop.GetDistanceFromPoint(Origin) <= Radius)
                {
                    if (Origin.X >= box.MaxX)
                    {
                        verticalIntersection = new Vector3(box.MaxX, box.MaxY, box.Center.Z);
                    }
                    else if (Origin.X <= box.MinX)
                    {
                        verticalIntersection = new Vector3(box.MinX, box.MaxY, box.Center.Z);
                    }
                    else
                    {
                        verticalIntersection = new Vector3(Origin.X, box.MaxY, box.Center.Z);
                    }
                }
            }

            if (horizontalIntersection.HasValue && verticalIntersection.HasValue)
            {
                // TODO - Determine if LengthSquared is okay here... might cause problems at very close distances
                var distanceA = (horizontalIntersection.Value - Origin).LengthSquared;
                var distanceB = (verticalIntersection.Value - Origin).LengthSquared;

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
