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

            return new Collision2D(this, body);
        }

        private Collision2D GetCircleCircleCollision(Body2D body)
        {
            var collision = new Collision2D(this, body);

            var normal = body.Position - Position;
            var distanceSquared = normal.LengthSquared;

            var circleA = (Circle)Shape;
            var circleB = (Circle)body.Shape;

            var radius = circleA.Radius + circleB.Radius;

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

        private Collision2D GetCircleRectangleCollision(Body2D body)
        {
            return new Collision2D(this, body);
        }

        private Collision2D GetRectangleRectangleCollision(Body2D body)
        {
            return new Collision2D(this, body);
        }

        private Collision2D GetPolygonPolygonCollision(Body2D body)
        {
            return new Collision2D(this, body);
        }

        private Collision2D GetCirclePolygonCollision(Body2D body)
        {
            return new Collision2D(this, body);
        }

        private Collision2D GetRectanglePolygonCollision(Body2D body)
        {
            return new Collision2D(this, body);
        }
    }
}
