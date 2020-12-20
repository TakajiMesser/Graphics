using SavoryPhysicsCore.Helpers;
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
    public class Rectangle : IShape
    {
        public float Width { get; }
        public float Height { get; }

        public Rectangle(IEnumerable<Vector2> vertices)
        {
            var minX = vertices.Select(v => v.X).Min();
            var maxX = vertices.Select(v => v.X).Max();
            Width = maxX - minX;

            var minY = vertices.Select(v => v.Y).Min();
            var maxY = vertices.Select(v => v.Y).Max();
            Height = maxY - minY;
        }

        public Rectangle(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public IShape Duplicate() => new Rectangle(Width, Height);

        public IPartition ToPartition(Vector3 position)
        {
            var min = new Vector2(position.X - Width / 2.0f, position.Y - Height / 2.0f);
            var max = new Vector2(position.X + Width / 2.0f, position.Y + Height / 2.0f);

            return new Quad(min, max);
        }

        public Vector3 GetFurthestPointInDirection(Vector3 direction)
        {
            var xRatio = (Width / 2.0f) / direction.X;
            var yRatio = (Height / 2.0f) / direction.Y;

            var newX = direction.X * yRatio;
            var newY = direction.Y * xRatio;

            return (Math.Abs(newX) < Width / 2.0f)
                ? new Vector2(newX, direction.Y * yRatio).ToVector3()
                : new Vector2(direction.X * xRatio, newY).ToVector3();
        }

        /*public override Vector2 GetFurthestPoint(Vector2 position, Vector2 direction)
        {
            var xRatio = (Width / 2.0f) / direction.X;
            var yRatio = (Height / 2.0f) / direction.Y;

            var newX = direction.X * yRatio;
            var newY = direction.Y * xRatio;

            return (Math.Abs(newX) < Width / 2.0f)
                ? new Vector2(position.X + newX, position.Y+ direction.Y * yRatio)
                : new Vector2(position.X+ direction.X * xRatio, position.Y + newY);
        }*/

        /*public override bool CollidesWith(Vector2 position, Vector2 point) => point.X > position.X - Center.X - Width / 2.0f
            && point.X < position.X - Center.X + Width / 2.0f
            && point.Y > position.Y - Center.Y - Height / 2.0f
            && point.Y < position.Y - Center.Y + Height / 2.0f;*/

        public float CalculateInertia(float mass) => mass * (Width * Width + Height * Height) / 12.0f;
    }
}
