using Graphics.GameObjects;
using Graphics.Meshes;
using Graphics.Rendering.Vertices;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Physics.Collision
{
    public class BoundingBox : Collider
    {
        public float Width { get; set; }
        public float Height { get; set; }

        public float MinX => Center.X - Width / 2.0f;
        public float MaxX => Center.X + Width / 2.0f;
        public float MinY => Center.Y - Height / 2.0f;
        public float MaxY => Center.Y + Height / 2.0f;

        public BoundingBox(GameObject gameObject) : base(gameObject)
        {
            var minX = gameObject.Mesh.Vertices.Select(v => v.Position.X).Min();
            var maxX = gameObject.Mesh.Vertices.Select(v => v.Position.X).Max();
            Width = maxX - minX;

            var minY = gameObject.Mesh.Vertices.Select(v => v.Position.Y).Min();
            var maxY = gameObject.Mesh.Vertices.Select(v => v.Position.Y).Max();
            Height = maxY - minY;
        }

        public BoundingBox(Brush brush) : base(brush)
        {
            var minX = brush.Vertices.Select(v => v.Position.X).Min();
            var maxX = brush.Vertices.Select(v => v.Position.X).Max();
            Width = maxX - minX;

            var minY = brush.Vertices.Select(v => v.Position.Y).Min();
            var maxY = brush.Vertices.Select(v => v.Position.Y).Max();
            Height = maxY - minY;

            Center = new Vector3()
            {
                X = (maxX + minX) / 2.0f,
                Y = (maxY + minY) / 2.0f,
                Z = brush.Vertices.Select(v => v.Position.Z).Average()
            };
        }

        public override bool CollidesWith(Vector3 point) => (point.X > MinX && point.X < MaxX) && (point.Y > MinY && point.Y < MaxY);

        public override bool CollidesWith(Collider collider) => throw new NotImplementedException();

        public override bool CollidesWith(BoundingCircle boundingCircle) => HasCollision(boundingCircle, this);

        public override bool CollidesWith(BoundingBox boundingBox) =>
            (MinX < boundingBox.MaxX && MaxX > boundingBox.MinX) && (MinY < boundingBox.MaxY && MaxY > boundingBox.MinY);
    }
}
