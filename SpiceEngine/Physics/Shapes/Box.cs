using OpenTK;
using SpiceEngine.Physics.Collisions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Physics.Shapes
{
    public class Box : Shape3D
    {
        public float Width { get; private set; }
        public float Height { get; private set; }
        public float Depth { get; private set; }

        public Box(IEnumerable<Vector3> vertices)
        {
            var minX = vertices.Select(v => v.X).Min();
            var maxX = vertices.Select(v => v.X).Max();
            Width = maxX - minX;

            var minY = vertices.Select(v => v.Y).Min();
            var maxY = vertices.Select(v => v.Y).Max();
            Height = maxY - minY;

            var minZ = vertices.Select(v => v.Z).Min();
            var maxZ = vertices.Select(v => v.Z).Max();
            Depth = maxZ - minZ;
        }

        public Box(float width, float height, float depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
        }

        public override Shape3D Duplicate() => new Box(Width, Height, Depth);

        public override IPartition ToPartition(Vector3 position)
        {
            var min = new Vector3(position.X - Width / 2.0f, position.Y - Height / 2.0f, position.Z - Depth / 2.0f);
            var max = new Vector3(position.X + Width / 2.0f, position.Y + Height / 2.0f, position.Z + Depth / 2.0f);

            return new Oct(min, max);
        }

        public override Vector3 GetFurthestPointInDirection(Vector3 direction)
        {
            var xRatio = (Width / 2.0f) / direction.X;
            var yRatio = (Height / 2.0f) / direction.Y;
            var zRatio = (Depth / 2.0f) / direction.Z;

            var newX = direction.X * yRatio;
            var newY = direction.Y * xRatio;
            var newZ = direction.Z * zRatio;

            return new Vector3();
            /*return (Math.Abs(newX) < Width / 2.0f)
                ? new Vector2(newX, direction.Y * yRatio)
                : new Vector2(direction.X * xRatio, newY);*/
        }

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
        }*/

        /*public override bool CollidesWith(Vector3 position, Vector3 point) => point.X > position.X - Center.X - Width / 2.0f
            && point.X < position.X - Center.X + Width / 2.0f
            && point.Y > position.Y - Center.Y - Height / 2.0f
            && point.Y < position.Y - Center.Y + Height / 2.0f
            && point.Z > position.Z - Center.Z - Depth / 2.0f
            && point.Z < position.Z - Center.Z + Depth / 2.0f;*/

        public override float CalculateInertia(float mass) => mass * (Width * Width + Height * Height + Depth * Depth) / 12.0f;
    }
}
