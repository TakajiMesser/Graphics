using TakoEngine.Physics.Collision;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakoEngine.Physics.Raycasting
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
            }
            else if (horizontalIntersection.HasValue && !verticalIntersection.HasValue)
            {
                intersection = horizontalIntersection.Value;
            }
            else if (!horizontalIntersection.HasValue && verticalIntersection.HasValue)
            {
                intersection = verticalIntersection.Value;
            }
            else
            {
                intersection = Vector3.Zero;
                return false;
            }

            // We now have a valid intersection, so find the distance from it and see if it is within our radius
            if ((intersection - Origin).Length <= Radius)
            {
                return true;
            }
            else
            {
                intersection = Vector3.Zero;
                return false;
            }
        }

        // From https://math.stackexchange.com/questions/256100/how-can-i-find-the-points-at-which-two-circles-intersect
        public bool TryGetCircleIntersection(BoundingCircle circle, out Vector3 intersection)
        {
            var distanceSquared = (circle.Center.X - Origin.X) * (circle.Center.X - Origin.X) + (circle.Center.Y - Origin.Y) * (circle.Center.Y - Origin.Y);
            var r1Squared = Radius * Radius;
            var r2Squared = circle.Radius * circle.Radius;

            var discriminant = (2 * (r1Squared + r2Squared)) / distanceSquared
                - (r1Squared - r2Squared) * (r1Squared - r2Squared) / (distanceSquared * distanceSquared)
                - 1;

            if (discriminant >= 0)
            {
                // Calculate coefficients in advance, since they are used multiple times
                var b = (r1Squared - r2Squared) / (2 * distanceSquared);
                var c = 0.5f * (float)Math.Sqrt(discriminant);

                if (discriminant == 0)
                {
                    intersection = new Vector3()
                    {
                        X = 0.5f * (Origin.X + circle.Center.X) + b * (circle.Center.X - Origin.X) + c * (circle.Center.Y - Origin.Y),
                        Y = 0.5f * (Origin.X + circle.Center.X) + b * (circle.Center.X - Origin.X) + c * (circle.Center.Y - Origin.Y),
                        Z = circle.Center.Z
                    };

                    return true;
                }
                else
                {
                    var intersectionA = new Vector3()
                    {
                        X = 0.5f * (Origin.X + circle.Center.X) + b * (circle.Center.X - Origin.X) + c * (circle.Center.Y - Origin.Y),
                        Y = 0.5f * (Origin.X + circle.Center.X) + b * (circle.Center.X - Origin.X) + c * (circle.Center.Y - Origin.Y),
                        Z = circle.Center.Z
                    };

                    var intersectionB = new Vector3()
                    {
                        X = 0.5f * (Origin.X + circle.Center.X) + b * (circle.Center.X - Origin.X) - c * (circle.Center.Y - Origin.Y),
                        Y = 0.5f * (Origin.X + circle.Center.X) + b * (circle.Center.X - Origin.X) - c * (circle.Center.Y - Origin.Y),
                        Z = circle.Center.Z
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
    }
}
