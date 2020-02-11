using OpenTK;
using System;
using UmamiPhysicsCore.Shapes;

namespace UmamiPhysicsCore.Raycasting
{
    public struct RayCircle
    {
        public Vector3 Origin { get; set; }
        public float Radius { get; set; }

        public RayCircle(Vector3 origin, float radius)
        {
            Origin = origin;
            Radius = radius;
        }

        public bool TryGetBoxIntersection(Box box, Vector3 position, out Vector3 intersection)
        {
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

                if (boxLeft.GetDistanceFromPoint(Origin) <= Radius)
                {
                    if (Origin.Y >= position.Y + box.Height / 2.0f)
                    {
                        horizontalIntersection = new Vector3(position.X - box.Width / 2.0f, position.Y + box.Height / 2.0f, position.Z);
                    }
                    else if (Origin.Y <= position.Y - box.Height / 2.0f)
                    {
                        horizontalIntersection = new Vector3(position.X - box.Width / 2.0f, position.Y - box.Height / 2.0f, position.Z);
                    }
                    else
                    {
                        horizontalIntersection = new Vector3(position.X - box.Width / 2.0f, Origin.Y, position.Z);
                    }
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

                if (boxRight.GetDistanceFromPoint(Origin) <= Radius)
                {
                    if (Origin.Y >= position.Y + box.Height / 2.0f)
                    {
                        horizontalIntersection = new Vector3(position.X + box.Width / 2.0f, position.Y + box.Height / 2.0f, position.Z);
                    }
                    else if (Origin.Y <= position.Y - box.Height / 2.0f)
                    {
                        horizontalIntersection = new Vector3(position.X + box.Width / 2.0f, position.Y - box.Height / 2.0f, position.Z);
                    }
                    else
                    {
                        horizontalIntersection = new Vector3(position.X + box.Width / 2.0f, Origin.Y, position.Z);
                    }
                }
            }

            if (Origin.Y < position.Y - box.Height / 2.0f)
            {
                // Consider bottom side
                var boxBottom = new LineSegment()
                {
                    PointA = new Vector3(position.X - box.Width / 2.0f, position.Y - box.Height / 2.0f, position.Z),
                    PointB = new Vector3(position.X + box.Width / 2.0f, position.Y - box.Height / 2.0f, position.Z)
                };

                if (boxBottom.GetDistanceFromPoint(Origin) <= Radius)
                {
                    if (Origin.X >= position.X + box.Width / 2.0f)
                    {
                        verticalIntersection = new Vector3(position.X + box.Width / 2.0f, position.Y - box.Height / 2.0f, position.Z);
                    }
                    else if (Origin.X <= position.X - box.Width / 2.0f)
                    {
                        verticalIntersection = new Vector3(position.X - box.Width / 2.0f, position.Y - box.Height / 2.0f, position.Z);
                    }
                    else
                    {
                        verticalIntersection = new Vector3(Origin.X, position.Y - box.Height / 2.0f, position.Z);
                    }
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

                if (boxTop.GetDistanceFromPoint(Origin) <= Radius)
                {
                    if (Origin.X >= position.X + box.Width / 2.0f)
                    {
                        verticalIntersection = new Vector3(position.X + box.Width / 2.0f, position.Y + box.Height / 2.0f, position.Z);
                    }
                    else if (Origin.X <= position.X - box.Width / 2.0f)
                    {
                        verticalIntersection = new Vector3(position.X - box.Width / 2.0f, position.Y + box.Height / 2.0f, position.Z);
                    }
                    else
                    {
                        verticalIntersection = new Vector3(Origin.X, position.Y + box.Height / 2.0f, position.Z);
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
        public bool TryGetCircleIntersection(Circle circle, Vector3 position, out Vector3 intersection)
        {
            var distanceSquared = (position.X - Origin.X) * (position.X - Origin.X) + (position.Y - Origin.Y) * (position.Y - Origin.Y);
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
                        X = 0.5f * (Origin.X + position.X) + b * (position.X - Origin.X) + c * (position.Y - Origin.Y),
                        Y = 0.5f * (Origin.X + position.X) + b * (position.X - Origin.X) + c * (position.Y - Origin.Y),
                        Z = position.Z
                    };

                    return true;
                }
                else
                {
                    var intersectionA = new Vector3()
                    {
                        X = 0.5f * (Origin.X + position.X) + b * (position.X - Origin.X) + c * (position.Y - Origin.Y),
                        Y = 0.5f * (Origin.X + position.X) + b * (position.X - Origin.X) + c * (position.Y - Origin.Y),
                        Z = position.Z
                    };

                    var intersectionB = new Vector3()
                    {
                        X = 0.5f * (Origin.X + position.X) + b * (position.X - Origin.X) - c * (position.Y - Origin.Y),
                        Y = 0.5f * (Origin.X + position.X) + b * (position.X - Origin.X) - c * (position.Y - Origin.Y),
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
    }
}
