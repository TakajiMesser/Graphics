using SavoryPhysicsCore.Partitioning;
using SpiceEngineCore.Geometry.Vectors;
using System.Collections.Generic;
using System.Linq;

namespace SavoryPhysicsCore.Shapes.TwoDimensional
{
    public class Polygon : IShape
    {
        public List<Vector2> Vertices { get; } = new List<Vector2>();

        public Polygon(IEnumerable<Vector2> vertices) => Vertices.AddRange(vertices);

        public IShape Duplicate() => new Polygon(Vertices);

        public IPartition ToPartition(Vector3 position)
        {
            Vertices.Select(v => v.X).Min();

            var min = new Vector2(
                Vertices.Min(v => v.X),
                Vertices.Min(v => v.Y)
            );

            var max = new Vector2(
                Vertices.Max(v => v.X),
                Vertices.Max(v => v.Y)
            );

            return new Quad(min, max);
        }

        public Vector3 GetFurthestPointInDirection(Vector3 direction) => direction.Normalized();

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

        public float CalculateInertia(float mass)
        {
            return 0.0f;
        }
    }
}
