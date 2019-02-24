using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Physics.Collisions;
using SpiceEngine.Physics.Shapes;
using System;

namespace SpiceEngine.Physics.Bodies
{
    public abstract class Body2D : IBody
    {
        public int EntityID { get; }
        public IShape Shape { get; }
        public Vector2 Position { get; set; }
        public float Restitution { get; set; }

        public Body2D(IEntity entity, IShape shape)
        {
            EntityID = entity.ID;
            Shape = shape;
            Position = entity.Position.Xy;
        }

        public Collision2D GetCollision(Body2D body)
        {
            if (Shape is Circle && body.Shape is Circle)
            {
                return GetCircleCircleCollision(body);
            }
            else if (Shape is Rectangle && body.Shape is Rectangle)
            {
                return GetRectangleRectangleCollision(body);
            }
            else if (Shape is Polygon2D && body.Shape is Polygon2D)
            {
                return GetPolygonPolygonCollision(body);
            }
            else if (Shape is Circle && body.Shape is Rectangle)
            {
                return GetCircleRectangleCollision(body);
            }
            else if (Shape is Rectangle && body.Shape is Circle)
            {
                return body.GetCircleRectangleCollision(this);
            }
            else if (Shape is Circle && body.Shape is Polygon2D)
            {
                return GetCirclePolygonCollision(body);
            }
            else if (Shape is Polygon2D && body.Shape is Circle)
            {
                return body.GetCirclePolygonCollision(this);
            }
            else if (Shape is Rectangle && body.Shape is Polygon2D)
            {
                return GetRectanglePolygonCollision(body);
            }
            else if (Shape is Polygon2D && body.Shape is Rectangle)
            {
                return body.GetRectanglePolygonCollision(this);
            }

            return new Collision2D(this, body);
        }

        private Collision2D GetCircleCircleCollision(Body2D body)
        {
            var collision = new Collision2D(this, body);

            var circleA = (Circle)Shape;
            var circleB = (Circle)body.Shape;

            var radius = circleA.Radius + circleB.Radius;
            var normal = body.Position - Position;
            var distanceSquared = normal.LengthSquared;

            if (distanceSquared < radius * radius)
            {
                var distance = (float)Math.Sqrt(distanceSquared);

                if (distance == 0.0f)
                {
                    collision.PenetrationDepth = circleA.Radius;
                    collision.ContactNormal = new Vector2(1, 0);
                    collision.ContactPoints.Add(Position);
                }
                else
                {
                    collision.PenetrationDepth = radius - distance;
                    collision.ContactNormal = normal / distance;
                    collision.ContactPoints.Add(normal * circleA.Radius + Position);
                }
            }

            return collision;
        }

        private Collision2D GetRectangleRectangleCollision(Body2D body)
        {
            var collision = new Collision2D(this, body);

            var rectangleA = (Rectangle)Shape;
            var rectangleB = (Rectangle)body.Shape;

            /*var doesCollide = positionA.X - rectangleA.Width / 2.0f < positionB.X + rectangleB.Width / 2.0f
                && positionA.X + rectangleA.Width / 2.0f > positionB.X - rectangleB.Width / 2.0f
                && positionA.Y - rectangleA.Height / 2.0f < positionB.Y + rectangleB.Height / 2.0f
                && positionA.Y + rectangleA.Height / 2.0f > positionB.Y - rectangleB.Height / 2.0f;*/

            return collision;
        }

        private Collision2D GetPolygonPolygonCollision(Body2D body)
        {
            var collision = new Collision2D(this, body);

            var polygonA = (Polygon2D)Shape;
            var polygonB = (Polygon2D)body.Shape;

            return collision;
        }

        private Collision2D GetCircleRectangleCollision(Body2D body)
        {
            var collision = new Collision2D(this, body);

            var circle = (Circle)Shape;
            var rectangle = (Rectangle)body.Shape;

            /*var closestX = (positionB.X > positionA.X + rectangleA.Width / 2.0f)
                ? positionA.X + rectangleA.Width / 2.0f
                : (positionB.X < positionA.X - rectangleA.Width / 2.0f)
                    ? positionA.X - rectangleA.Width / 2.0f
                    : positionB.X;

            var closestY = (positionB.Y > positionA.Y + rectangleA.Height / 2.0f)
                ? positionA.Y + rectangleA.Height / 2.0f
                : (positionB.Y < positionA.Y - rectangleA.Height / 2.0f)
                    ? positionA.Y - rectangleA.Height / 2.0f
                    : positionB.Y;

            var distanceSquared = Math.Pow(positionB.X - closestX, 2) + Math.Pow(positionB.Y - closestY, 2);
            var doesCollide = distanceSquared < Math.Pow(circleB.Radius, 2);*/

            return collision;
        }

        private Collision2D GetCirclePolygonCollision(Body2D body)
        {
            var collision = new Collision2D(this, body);

            var circle = (Circle)Shape;
            var polygon = (Polygon2D)body.Shape;

            return collision;
        }

        private Collision2D GetRectanglePolygonCollision(Body2D body)
        {
            var collision = new Collision2D(this, body);

            var rectangle = (Rectangle)Shape;
            var polygon = (Polygon2D)body.Shape;

            return collision;
        }
    }
}
