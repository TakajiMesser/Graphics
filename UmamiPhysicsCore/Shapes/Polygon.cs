﻿using OpenTK;
using SavoryPhysicsCore.Collisions;
using SpiceEngineCore.Physics;
using System.Collections.Generic;
using System.Linq;

namespace SavoryPhysicsCore.Shapes
{
    public class Polygon : Shape2D
    {
        public List<Vector2> Vertices { get; } = new List<Vector2>();

        public Polygon(int entityID, IEnumerable<Vector2> vertices) : base(entityID) => Vertices.AddRange(vertices);

        public override Shape2D Duplicate(int entityID) => new Polygon(entityID, Vertices);

        public override IPartition ToPartition(Vector3 position)
        {
            Vertices.Select(v => v.X).Min();

            var min = new Vector2()
            {
                X = Vertices.Min(v => v.X),
                Y = Vertices.Min(v => v.Y)
            };

            var max = new Vector2()
            {
                X = Vertices.Max(v => v.X),
                Y = Vertices.Max(v => v.Y)
            };

            return new Quad(min, max);
        }

        public override Vector2 GetFurthestPointInDirection(Vector2 direction) => direction.Normalized();

        // TODO - Correct this (right now it calculates the same as 2D, but just tags on the Z position)
        /*public override Vector3 GetFurthestPoint(Vector3 position, Vector3 direction)
        {
            var xRatio = (Width / 2.0f) / direction.X;
            var yRatio = (Height / 2.0f) / direction.Y;

            var newX = direction.X * yRatio;
            var newY = direction.Y * xRatio;

            return (Math.Abs(newX) < Width / 2.0f)
                ? new Vector3(position.X - Center.X + newX, position.Y - Center.Y + direction.Y * yRatio, position.Z - Center.Z)
                : new Vector3(position.X - Center.X + direction.X * xRatio, position.Y - Center.Y + newY, position.Z - Center.Z);
        }

        public override bool CollidesWith(Vector3 position, Vector3 point) => point.X > position.X - Center.X - Width / 2.0f
            && point.X < position.X - Center.X + Width / 2.0f
            && point.Y > position.Y - Center.Y - Height / 2.0f
            && point.Y < position.Y - Center.Y + Height / 2.0f
            && point.Z > position.Z - Center.Z - Depth / 2.0f
            && point.Z < position.Z - Center.Z + Depth / 2.0f;*/

        public override float CalculateInertia(float mass)
        {
            return 0.0f;
        }
    }
}
