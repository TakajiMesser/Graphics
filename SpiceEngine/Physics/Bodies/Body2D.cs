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
        public BodyStates State { get; set; }
        public Shape2D Shape { get; }

        public Vector2 Position { get; set; }
        public float Restitution { get; set; }

        public bool IsMovable => false;
        public bool IsPhysical => false;

        public Body2D(IEntity entity, Shape2D shape)
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
            else if (Shape is Polygon && body.Shape is Polygon)
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
            else if (Shape is Circle && body.Shape is Polygon)
            {
                return GetCirclePolygonCollision(body);
            }
            else if (Shape is Polygon && body.Shape is Circle)
            {
                return body.GetCirclePolygonCollision(this);
            }
            else if (Shape is Rectangle && body.Shape is Polygon)
            {
                return GetRectanglePolygonCollision(body);
            }
            else if (Shape is Polygon && body.Shape is Rectangle)
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

            var polygonA = (Polygon)Shape;
            var polygonB = (Polygon)body.Shape;

            return collision;
        }

        private Collision2D GetCircleRectangleCollision(Body2D body)
        {
            var collision = new Collision2D(this, body);

            var circle = (Circle)Shape;
            var rectangle = (Rectangle)body.Shape;

            var closestPoint = new Vector2()
            {
                X = MathHelper.Clamp(Position.X, body.Position.X - rectangle.Width / 2.0f, body.Position.X + rectangle.Width / 2.0f),
                Y = MathHelper.Clamp(Position.Y, body.Position.Y - rectangle.Height / 2.0f, body.Position.Y + rectangle.Height / 2.0f)
            };

            var offset = Position - closestPoint;
            if (offset != Vector2.Zero)
            {
                var offsetLengthSquared = offset.LengthSquared;

                if (offsetLengthSquared < circle.Radius * circle.Radius)
                {
                    var offsetLength = (float)Math.Sqrt(offsetLengthSquared);

                    collision.PenetrationDepth = offsetLength;
                    collision.ContactNormal = offset / offsetLength;
                    collision.ContactPoints.Add(closestPoint);

                    var circlePositionToContactPoint = closestPoint - Position;
                    collision.PenetrationDepth = circle.Radius + Vector2.Dot(circlePositionToContactPoint, collision.ContactNormal);
                }
            }

            return collision;
        }

        private Collision2D GetCirclePolygonCollision(Body2D body)
        {
            var collision = new Collision2D(this, body);

            var circle = (Circle)Shape;
            var polygon = (Polygon)body.Shape;

            return collision;
        }

        private Collision2D GetRectanglePolygonCollision(Body2D body)
        {
            var collision = new Collision2D(this, body);

            var rectangle = (Rectangle)Shape;
            var polygon = (Polygon)body.Shape;

            return collision;
        }
    }
}
