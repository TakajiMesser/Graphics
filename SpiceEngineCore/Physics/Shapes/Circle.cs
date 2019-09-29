using OpenTK;
using SpiceEngineCore.Physics.Collisions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngineCore.Physics.Shapes
{
    public class Circle : Shape2D
    {
        public float Radius { get; }

        public Circle(IEnumerable<Vector2> vertices)
        {
            var maxDistanceSquared = vertices.Select(v => v.LengthSquared).Max();
            Radius = (float)Math.Sqrt(maxDistanceSquared);
        }

        public Circle(float radius)
        {
            Radius = radius;
        }

        public override Shape2D Duplicate() => new Circle(Radius);

        public override IPartition ToPartition(Vector3 position)
        {
            var min = new Vector2(position.X - Radius, position.Y - Radius);
            var max = new Vector2(position.X + Radius, position.Y + Radius);

            return new Quad(min, max);
        }

        public override Vector2 GetFurthestPointInDirection(Vector2 direction) => direction.Normalized() * Radius;

        /*public override Vector2 GetFurthestPoint(Vector2 position, Vector2 direction)
        {
            var extended = position + (direction.Normalized() * Radius);
            return new Vector2(extended.X, extended.Y);
        }

        public override bool CollidesWith(Vector2 position, Vector2 point)
        {
            var distanceSquared = Math.Pow(point.X - position.X, 2.0f) + Math.Pow(point.Y - position.Y, 2.0f);
            return distanceSquared < Math.Pow(Radius, 2.0f);
        }*/

        public override float CalculateInertia(float mass) => mass * Radius * Radius;
    }
}
