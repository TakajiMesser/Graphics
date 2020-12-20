﻿using SavoryPhysicsCore.Helpers;
using SavoryPhysicsCore.Partitioning;
using System;
using System.Collections.Generic;
using System.Linq;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

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
