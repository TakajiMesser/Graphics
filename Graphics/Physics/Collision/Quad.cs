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

        public bool CanContain(Collider collider)
        {
            if (collider.GetType() == typeof(BoundingBox))
            {
                var boundingBox = (BoundingBox)collider;
                return (Min.X < boundingBox.MaxX && Max.X > boundingBox.MinX) && (Min.Y < boundingBox.MaxY && Max.Y > boundingBox.MinY);
            }
            else if (collider.GetType() == typeof(BoundingSphere))
            {
                var boundingSphere = (BoundingSphere)collider;

                var closestX = (boundingSphere.Center.X > Max.X)
                ? Max.X
                : (boundingSphere.Center.X < Min.X)
                    ? Min.X
                    : boundingSphere.Center.X;

                var closestY = (boundingSphere.Center.Y > Max.Y)
                    ? Max.Y
                    : (boundingSphere.Center.Y < Min.Y)
                        ? Min.Y
                        : boundingSphere.Center.Y;

                var distanceSquared = Math.Pow(boundingSphere.Center.X - closestX, 2) + Math.Pow(boundingSphere.Center.Y - closestY, 2);
                return distanceSquared < Math.Pow(boundingSphere.Radius, 2);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
