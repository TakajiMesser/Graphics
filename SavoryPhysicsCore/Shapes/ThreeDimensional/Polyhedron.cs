using SavoryPhysicsCore.Partitioning;
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
    public class Polyhedron : IShape
    {
        public List<Vector3> Vertices { get; } = new List<Vector3>();

        public Polyhedron(IEnumerable<Vector3> vertices) => Vertices.AddRange(vertices);

        public IShape Duplicate() => new Polyhedron(Vertices);

        public IPartition ToPartition(Vector3 position)
        {
            var min = new Vector3(
                Vertices.Min(v => v.X),
                Vertices.Min(v => v.Y),
                Vertices.Min(v => v.Z)
            );

            var max = new Vector3(
                Vertices.Max(v => v.X),
                Vertices.Max(v => v.Y),
                Vertices.Max(v => v.Z)
            );

            return new Oct(min, max);
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
