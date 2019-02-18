using OpenTK;
using SpiceEngine.Physics.Collisions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Physics.Shapes
{
    public class Circle : Shape2D
    {
        public float Radius { get; }

        public override Vector2 Center { get; }
        public override float Mass { get; set; }
        public override float MomentOfInertia { get; }

        public Circle(IEnumerable<Vector2> vertices)
        {
            var maxDistanceSquared = vertices.Select(v => v.LengthSquared).Max();
            Radius = (float)Math.Sqrt(maxDistanceSquared);
        }

        public Circle(float radius)
        {
            Radius = radius;
        }

        public override IShape Duplicate() => new Circle(Radius);

        public override IPartition ToCollider(Vector3 position)
        {
            var min = new Vector2(position.X - Center.X - Radius, position.Y - Center.Y - Radius);
            var max = new Vector2(position.X - Center.X + Radius, position.Y - Center.Y + Radius);

            return new Quad(min, max);
        }

        public override Vector2 GetFurthestPoint(Vector2 position, Vector2 direction)
        {
            var extended = position - Center + (direction.Normalized() * Radius);
            return new Vector2(extended.X, extended.Y);
        }

        public override bool CollidesWith(Vector2 position, Vector2 point)
        {
            var distanceSquared = Math.Pow(point.X - position.X - Center.X, 2.0f) + Math.Pow(point.Y - position.Y - Center.Y, 2.0f);
            return distanceSquared < Math.Pow(Radius, 2.0f);
        }
    }
}
