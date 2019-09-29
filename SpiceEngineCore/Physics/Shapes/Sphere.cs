using OpenTK;
using SpiceEngineCore.Physics.Collisions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngineCore.Physics.Shapes
{
    public class Sphere : Shape3D
    {
        public float Radius { get; }

        public Sphere(IEnumerable<Vector3> vertices)
        {
            var maxDistanceSquared = vertices.Select(v => v.LengthSquared).Max();
            Radius = (float)Math.Sqrt(maxDistanceSquared);
        }

        public Sphere(float radius)
        {
            Radius = radius;
        }

        public override Shape3D Duplicate() => new Sphere(Radius);

        public override IPartition ToPartition(Vector3 position)
        {
            var min = new Vector3(position.X - Radius, position.Y - Radius, position.Z - Radius);
            var max = new Vector3(position.X + Radius, position.Y + Radius, position.Z + Radius);

            return new Oct(min, max);
        }

        public override Vector3 GetFurthestPointInDirection(Vector3 direction) => direction.Normalized() * Radius;

        /*public override Vector3 GetFurthestPoint(Vector3 position, Vector3 direction)
        {
            return position - Center + (direction.Normalized() * Radius);
        }

        public override bool CollidesWith(Vector3 position, Vector3 point)
        {
            var distanceSquared = Math.Pow(point.X - position.X - Center.X, 2.0f) + Math.Pow(point.Y - position.Y - Center.Y, 2.0f) + Math.Pow(point.Z - position.Z - Center.Z, 2.0f);
            return distanceSquared < Math.Pow(Radius, 2.0f);
        }*/

        public override float CalculateInertia(float mass) => 0.0f;
    }
}
