using OpenTK;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Physics;
using System;
using UmamiPhysicsCore.Collisions;
using UmamiPhysicsCore.Shapes;

namespace UmamiPhysicsCore.Bodies
{
    public abstract class Body2D : IBody
    {
        public int EntityID { get; }
        public BodyStates State { get; set; }
        public Shape2D Shape { get; }

        public Vector3 Position { get; set; }
        public float Restitution { get; set; }

        public bool IsMovable => false;
        public bool IsPhysical { get; set; } = false;

        public Body2D(IEntity entity, Shape2D shape)
        {
            EntityID = entity.ID;
            Shape = shape;
            Position = entity.Position;
        }

        public ICollision GetCollision(IBody body)
        {
            if (body is Body2D body2D)
            {
                if (Shape is Circle && body2D.Shape is Circle)
                {
                    return GetCircleCircleCollision(body2D);
                }
                else if (Shape is Rectangle && body2D.Shape is Rectangle)
                {
                    return GetRectangleRectangleCollision(body2D);
                }
                else if (Shape is Polygon && body2D.Shape is Polygon)
                {
                    return GetPolygonPolygonCollision(body2D);
                }
                else if (Shape is Circle && body2D.Shape is Rectangle)
                {
                    return GetCircleRectangleCollision(body2D);
                }
                else if (Shape is Rectangle && body2D.Shape is Circle)
                {
                    return body2D.GetCircleRectangleCollision(this);
                }
                else if (Shape is Circle && body2D.Shape is Polygon)
                {
                    return GetCirclePolygonCollision(body2D);
                }
                else if (Shape is Polygon && body2D.Shape is Circle)
                {
                    return body2D.GetCirclePolygonCollision(this);
                }
                else if (Shape is Rectangle && body2D.Shape is Polygon)
                {
                    return GetRectanglePolygonCollision(body2D);
                }
                else if (Shape is Polygon && body2D.Shape is Rectangle)
                {
                    return body2D.GetRectanglePolygonCollision(this);
                }

                return new Collision2D(this, body2D);
            }
            else
            {
                return null;
            }
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
                    collision.ContactPoints.Add(Position.Xy);
                }
                else
                {
                    collision.PenetrationDepth = radius - distance;
                    collision.ContactNormal = normal.Xy / distance;
                    collision.ContactPoints.Add((normal * circleA.Radius + Position).Xy);
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

            var offset = Position.Xy - closestPoint;
            if (offset != Vector2.Zero)
            {
                var offsetLengthSquared = offset.LengthSquared;

                if (offsetLengthSquared < circle.Radius * circle.Radius)
                {
                    var offsetLength = (float)Math.Sqrt(offsetLengthSquared);

                    collision.PenetrationDepth = offsetLength;
                    collision.ContactNormal = offset / offsetLength;
                    collision.ContactPoints.Add(closestPoint);

                    var circlePositionToContactPoint = closestPoint - Position.Xy;
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
