using OpenTK;
using SpiceEngine.Physics.Collision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceEngine.Physics.Shapes
{
    public class Box : Shape3D
    {
        public float Width { get; }
        public float Height { get; }
        public float Depth { get; }

        public override Vector3 Center { get; }
        public override float Mass { get; set; }
        public override float MomentOfInertia { get; }

        /*public float MinX => Center.X - Width / 2.0f;
        public float MaxX => Center.X + Width / 2.0f;
        public float MinY => Center.Y - Height / 2.0f;
        public float MaxY => Center.Y + Height / 2.0f;*/

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

            Center = new Vector3()
            {
                X = (maxX + minX) / 2.0f,
                Y = (maxY + minY) / 2.0f,
                Z = (maxZ + minZ) / 2.0f
            };
        }

        public override ICollider ToCollider(Vector3 position)
        {
            var min = new Vector3(position.X - Center.X - Width / 2.0f, position.Y - Center.Y - Height / 2.0f, position.Z - Center.Z - Depth / 2.0f);
            var max = new Vector3(position.X - Center.X + Width / 2.0f, position.Y - Center.Y + Height / 2.0f, position.Z - Center.Z + Depth / 2.0f);

            return new Oct(min, max);
        }

        // TODO - Correct this (right now it calculates the same as 2D, but just tags on the Z position)
        public override Vector3 GetFurthestPoint(Vector3 position, Vector3 direction)
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
            && point.Z < position.Z - Center.Z + Depth / 2.0f;
    }
}
