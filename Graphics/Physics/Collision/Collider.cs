using Graphics.GameObjects;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Physics.Collision
{
    public abstract class Collider
    {
        public object AttachedObject { get; set; }
        public Vector3 Center { get; set; }

        public Collider(GameObject gameObject)
        {
            AttachedObject = gameObject;
            Center = gameObject.Position;
        }

        public Collider(Brush brush)
        {
            AttachedObject = brush;
        }

        public abstract bool CollidesWith(Vector3 point);
        public abstract bool CollidesWith(Collider collider);
        public abstract bool CollidesWith(BoundingSphere boundingSphere);
        public abstract bool CollidesWith(BoundingBox boundingBox);

        public static bool HasCollision(BoundingSphere sphere, BoundingBox box)
        {
            var closestX = (sphere.Center.X > box.MaxX)
                ? box.MaxX
                : (sphere.Center.X < box.MinX)
                    ? box.MinX
                    : sphere.Center.X;

            var closestY = (sphere.Center.Y > box.MaxY)
                ? box.MaxY
                : (sphere.Center.Y < box.MinY)
                    ? box.MinY
                    : sphere.Center.Y;

            var distanceSquared = Math.Pow(sphere.Center.X - closestX, 2) + Math.Pow(sphere.Center.Y - closestY, 2);
            return distanceSquared < Math.Pow(sphere.Radius, 2);
        }
    }
}
