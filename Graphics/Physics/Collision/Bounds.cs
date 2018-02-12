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
    public abstract class Bounds
    {
        public object AttachedObject { get; set; }
        public Vector3 Center { get; set; }

        public Bounds(GameObject gameObject)
        {
            AttachedObject = gameObject;
            Center = gameObject.Model.Position;
        }

        public Bounds(Brush brush)
        {
            AttachedObject = brush;
        }

        public Bounds(Light light)
        {
            AttachedObject = light;

            switch (light)
            {
                case PointLight pLight:
                    Center = pLight.Position;
                    break;
                case SpotLight sLight:
                    Center = sLight.Position;
                    break;
            }
        }

        public abstract bool CollidesWith(Vector3 point);
        public abstract bool CollidesWith(Bounds collider);
        public abstract bool CollidesWith(BoundingCircle boundingCircle);
        public abstract bool CollidesWith(BoundingBox boundingBox);

        /// <summary>
        /// Gets the position on the collider's border
        /// </summary>
        /// <param name="direction">The direction from the collider's center</param>
        /// <returns>The border position</returns>
        public abstract Vector3 GetBorder(Vector3 direction);

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
