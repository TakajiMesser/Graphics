using OpenTK;
using SpiceEngine.Physics.Shapes;

namespace SpiceEngine.Physics.Bodies
{
    public abstract class Body2D : IBody
    {
        public int EntityID { get; }
        public IShape Shape { get; }
        public Vector2 Position { get; }

        public Body2D(IEntity entity, IShape shape)
        {
            EntityID = entity.ID;
            Shape = shape;
            Position = entity.Position;
        }

        public Collision GetCollision(Body2D body)
        {
            if (Shape is Circle && body.Shape is Circle)
            {
                return GetCircleCircleCollision(body);
            }

            return new Collision(this, body);
        }

        private Collision GetCircleCircleCollision(Body2D body)
        {
            var collision = new Collision(this, body);

            var normal = body.Position - Position;
            var distanceSquared = normal.LengthSquared;

            var circleA = (Circle)Shape;
            var circleB = (Circle)body.Shape;

            var radius = circleA.Radius + circleB.Radius;

            if (distanceSquared < radius * radius)
            {
                var distance = Math.Sqrt(distanceSquared);

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


        public static Collision GetCollision(Vector2 positionA, Shape2D shapeA, Vector2 positionB, Shape2D shapeB)
        {
            switch (shapeA)
            {
                case Rectangle rectangleA when shapeB is Rectangle rectangleB:
                    return Collides(positionA - rectangleA.Center, rectangleA, positionB - rectangleB.Center, rectangleB);
                case Rectangle rectangleA when shapeB is Circle circleB:
                    return Collides(positionA - rectangleA.Center, rectangleA, positionB - circleB.Center, circleB);
                case Circle circleA when shapeB is Rectangle rectangleB:
                    return Collides(positionB - circleA.Center, rectangleB, positionA - rectangleB.Center, circleA);
                case Circle circleA when shapeB is Circle circleB:
                    return Collides(positionA - circleA.Center, circleA, positionB - circleB.Center, circleB);
            }

            throw new NotImplementedException();
        }

        private static Collision GetCollision(Vector2 positionA, Rectangle rectangleA, Vector2 positionB, Rectangle rectangleB) =>
            positionA.X - rectangleA.Width / 2.0f < positionB.X + rectangleB.Width / 2.0f
            && positionA.X + rectangleA.Width / 2.0f > positionB.X - rectangleB.Width / 2.0f
            && positionA.Y - rectangleA.Height / 2.0f < positionB.Y + rectangleB.Height / 2.0f
            && positionA.Y + rectangleA.Height / 2.0f > positionB.Y - rectangleB.Height / 2.0f;

        private static Collision GetCollision(Vector2 positionA, Circle circleA, Vector2 positionB, Circle circleB)
        {
            var normal = positionB - positionA;
            var distanceSquared = normal.LengthSquared;
            var radius = circleA.Radius + circleB.Radius;

            if (distanceSquared >= radius * radius)
            {

            }

            var distanceSquared = Math.Pow(positionA.X - positionB.X, 2.0f) + Math.Pow(positionA.Y - positionB.Y, 2.0f);
            return distanceSquared < Math.Pow(circleA.Radius + circleB.Radius, 2.0f);
        }
    }
}
