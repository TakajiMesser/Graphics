using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Physics.Collision
{
    public struct Quad
    {
        public Vector3 Min { get; set; }
        public Vector3 Max { get; set; }

        public Quad(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        public bool CanContain(Bounds collider)
        {
            if (collider.GetType() == typeof(BoundingBox))
            {
                var boundingBox = (BoundingBox)collider;
                return (Min.X < boundingBox.MaxX && Max.X > boundingBox.MinX) && (Min.Y < boundingBox.MaxY && Max.Y > boundingBox.MinY);
            }
            else if (collider.GetType() == typeof(BoundingCircle))
            {
                var boundingCircle = (BoundingCircle)collider;

                var closestX = (boundingCircle.Center.X > Max.X)
                ? Max.X
                : (boundingCircle.Center.X < Min.X)
                    ? Min.X
                    : boundingCircle.Center.X;

                var closestY = (boundingCircle.Center.Y > Max.Y)
                    ? Max.Y
                    : (boundingCircle.Center.Y < Min.Y)
                        ? Min.Y
                        : boundingCircle.Center.Y;

                var distanceSquared = Math.Pow(boundingCircle.Center.X - closestX, 2) + Math.Pow(boundingCircle.Center.Y - closestY, 2);
                return distanceSquared < Math.Pow(boundingCircle.Radius, 2);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
