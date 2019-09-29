using OpenTK;
using SpiceEngineCore.Physics.Collisions;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngineCore.Physics.Shapes
{
    public class Polyhedron : Shape3D
    {
        public List<Vector3> Vertices { get; } = new List<Vector3>();

        public Polyhedron(IEnumerable<Vector3> vertices) => Vertices.AddRange(vertices);

        public override Shape3D Duplicate() => new Polyhedron(Vertices);

        public override IPartition ToPartition(Vector3 position)
        {
            var min = new Vector3()
            {
                X = Vertices.Min(v => v.X),
                Y = Vertices.Min(v => v.Y),
                Z = Vertices.Min(v => v.Z)
            };

            var max = new Vector3()
            {
                X = Vertices.Max(v => v.X),
                Y = Vertices.Max(v => v.Y),
                Z = Vertices.Max(v => v.Z)
            };

            return new Oct(min, max);
        }

        public override Vector3 GetFurthestPointInDirection(Vector3 direction) => direction.Normalized();

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
