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

namespace SavoryPhysicsCore.Shapes.ThreeDimensional
{
    public class Sphere : IShape
    {
        public float Radius { get; }

        public Sphere(IEnumerable<Vector3> vertices)
        {
            var maxDistanceSquared = vertices.Select(v => v.LengthSquared).Max();
            Radius = (float)Math.Sqrt(maxDistanceSquared);
        }

        public Sphere(float radius) => Radius = radius;

        public IShape Duplicate() => new Sphere(Radius);

        public IPartition ToPartition(Vector3 position)
        {
            var min = new Vector3(position.X - Radius, position.Y - Radius, position.Z - Radius);
            var max = new Vector3(position.X + Radius, position.Y + Radius, position.Z + Radius);

            return new Oct(min, max);
        }

        public Vector3 GetFurthestPointInDirection(Vector3 direction) => direction.Normalized() * Radius;

        /*public override Vector3 GetFurthestPoint(Vector3 position, Vector3 direction)
        {
            return position - Center + (direction.Normalized() * Radius);
        }

        public override bool CollidesWith(Vector3 position, Vector3 point)
        {
            var distanceSquared = Math.Pow(point.X - position.X - Center.X, 2.0f) + Math.Pow(point.Y - position.Y - Center.Y, 2.0f) + Math.Pow(point.Z - position.Z - Center.Z, 2.0f);
            return distanceSquared < Math.Pow(Radius, 2.0f);
        }*/

        public float CalculateInertia(float mass) => 0.0f;
    }
}
