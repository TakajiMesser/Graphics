using TakoEngine.GameObjects;
using TakoEngine.Meshes;
using TakoEngine.Rendering.Vertices;
using TakoEngine.Utilities;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakoEngine.Physics.Collision
{
    public class BoundingBox : Bounds
    {
        public float Width { get; set; }
        public float Height { get; set; }

        public float MinX => Center.X - Width / 2.0f;
        public float MaxX => Center.X + Width / 2.0f;
        public float MinY => Center.Y - Height / 2.0f;
        public float MaxY => Center.Y + Height / 2.0f;

        public BoundingBox(GameObject gameObject) : base(gameObject)
        {
            var vertices = gameObject.Model.Vertices.Select(v => Matrix4.CreateScale(gameObject.Model.Scale) * Matrix4.CreateFromQuaternion(gameObject.Model.Rotation) * new Vector4(v, 1.0f));

            var minX = vertices.Select(v => v.X).Min();
            var maxX = vertices.Select(v => v.X).Max();
            Width = maxX - minX;

            var minY = vertices.Select(v => v.Y).Min();
            var maxY = vertices.Select(v => v.Y).Max();
            Height = maxY - minY;
        }

        public BoundingBox(Brush brush) : base(brush)
        {
            var minX = brush.Vertices.Select(v => v.X).Min();
            var maxX = brush.Vertices.Select(v => v.X).Max();
            Width = maxX - minX;

            var minY = brush.Vertices.Select(v => v.Y).Min();
            var maxY = brush.Vertices.Select(v => v.Y).Max();
            Height = maxY - minY;

            Center = new Vector3()
            {
                X = (maxX + minX) / 2.0f,
                Y = (maxY + minY) / 2.0f,
                Z = brush.Vertices.Select(v => v.Z).Average()
            };
        }

        public override bool CollidesWith(Vector3 point) => (point.X > MinX && point.X < MaxX) && (point.Y > MinY && point.Y < MaxY);

        public override bool CollidesWith(Bounds collider) => throw new NotImplementedException();

        public override bool CollidesWith(BoundingCircle boundingCircle) => HasCollision(boundingCircle, this);

        public override bool CollidesWith(BoundingBox boundingBox) =>
            (MinX < boundingBox.MaxX && MaxX > boundingBox.MinX) && (MinY < boundingBox.MaxY && MaxY > boundingBox.MinY);

        public override Vector3 GetBorder(Vector3 direction)
        {
            var xRatio = (Width / 2.0f) / direction.X;
            var yRatio = (Height / 2.0f) / direction.Y;

            var newX = direction.X * yRatio;
            var newY = direction.Y * xRatio;

            return (Math.Abs(newX) < Width / 2.0f)
                ? new Vector3(Center.X + newX, Center.Y + direction.Y * yRatio, Center.Z)
                : new Vector3(Center.X + direction.X * xRatio, Center.Y + newY, Center.Z);
        }
    }
}
