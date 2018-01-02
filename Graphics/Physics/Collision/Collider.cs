using Graphics.GameObjects;
using Graphics.Lighting;
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

        public Collider(Light light)
        {
            AttachedObject = light;
            Center = light.Position;
        }

        public abstract bool CollidesWith(Vector3 point);
        public abstract bool CollidesWith(Collider collider);
        public abstract bool CollidesWith(BoundingCircle boundingCircle);
        public abstract bool CollidesWith(BoundingBox boundingBox);

        public static bool HasCollision(BoundingCircle circle, BoundingBox box)
        {
            var closestX = (circle.Center.X > box.MaxX)
                ? box.MaxX
                : (circle.Center.X < box.MinX)
                    ? box.MinX
                    : circle.Center.X;

            var closestY = (circle.Center.Y > box.MaxY)
                ? box.MaxY
                : (circle.Center.Y < box.MinY)
                    ? box.MinY
                    : circle.Center.Y;

            var distanceSquared = Math.Pow(circle.Center.X - closestX, 2) + Math.Pow(circle.Center.Y - closestY, 2);
            return distanceSquared < Math.Pow(circle.Radius, 2);
        }
    }
}
