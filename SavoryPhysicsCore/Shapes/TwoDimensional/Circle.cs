using SavoryPhysicsCore.Helpers;
using SavoryPhysicsCore.Partitioning;
using System;
using System.Collections.Generic;
using System.Linq;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SavoryPhysicsCore.Shapes.TwoDimensional
{
    public class Circle : IShape
    {
        public float Radius { get; }

        public Circle(IEnumerable<Vector2> vertices)
        {
            var maxDistanceSquared = vertices.Select(v => v.LengthSquared).Max();
            Radius = (float)Math.Sqrt(maxDistanceSquared);
        }

        public Circle(float radius) => Radius = radius;

        public IShape Duplicate() => new Circle(Radius);

        public IPartition ToPartition(Vector3 position)
        {
            var min = new Vector2(position.X - Radius, position.Y - Radius);
            var max = new Vector2(position.X + Radius, position.Y + Radius);

            return new Quad(min, max);
        }

        public Vector3 GetFurthestPointInDirection(Vector3 direction) => (direction.Normalized() * Radius).Flattened();

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

        public float CalculateInertia(float mass) => mass * Radius * Radius;
    }
}
