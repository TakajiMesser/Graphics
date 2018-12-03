using OpenTK;
using SpiceEngine.Physics.Collision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceEngine.Physics.Shapes
{
    public class Rectangle : Shape2D
    {
        public float Width { get; }
        public float Height { get; }

        public override float Mass { get; set; }
        public override float MomentOfInertia { get; }

        public Rectangle(IEnumerable<Vector2> vertices)
        {
            var minX = vertices.Select(v => v.X).Min();
            var maxX = vertices.Select(v => v.X).Max();
            Width = maxX - minX;

            var minY = vertices.Select(v => v.Y).Min();
            var maxY = vertices.Select(v => v.Y).Max();
            Height = maxY - minY;
        }

        public override ICollider ToCollider(Vector3 position)
        {
            var min = new Vector2(position.X - Width / 2.0f, position.Y - Height / 2.0f);
            var max = new Vector2(position.X + Width / 2.0f, position.Y + Height / 2.0f);

            return new Quad(min, max);
        }

        public override Vector2 GetFurthestPoint(Vector2 position, Vector2 direction)
        {
            var xRatio = (Width / 2.0f) / direction.X;
            var yRatio = (Height / 2.0f) / direction.Y;

            var newX = direction.X * yRatio;
            var newY = direction.Y * xRatio;

            return (Math.Abs(newX) < Width / 2.0f)
                ? new Vector2(position.X + newX, position.Y + direction.Y * yRatio)
                : new Vector2(position.X + direction.X * xRatio, position.Y + newY);
        }

        public override bool CollidesWith(Vector2 position, Vector2 point) => point.X > position.X - Width / 2.0f
            && point.X < position.X + Width / 2.0f
            && point.Y > position.Y - Height / 2.0f
            && point.Y < position.Y + Height / 2.0f;
    }
}
