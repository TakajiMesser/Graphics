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
    public class BoundingBox : ICollider
    {
        public Vector3 Center { get; set; }
        public Dictionary<string, GameProperty> Properties { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public float MinX => Center.X - Width / 2.0f;
        public float MaxX => Center.X + Width / 2.0f;
        public float MinY => Center.Y - Height / 2.0f;
        public float MaxY => Center.Y + Height / 2.0f;

        public BoundingBox(IEnumerable<Vertex> vertices)
        {
            var minX = vertices.Select(v => v.Position.X).Min();
            var maxX = vertices.Select(v => v.Position.X).Max();
            Width = maxX - minX;

            var minY = vertices.Select(v => v.Position.Y).Min();
            var maxY = vertices.Select(v => v.Position.Y).Max();
            Height = maxY - minY;

            Center = new Vector3()
            {
                X = (maxX + minX) / 2.0f,
                Y = (maxY + minY) / 2.0f,
                Z = vertices.Select(v => v.Position.Z).Average()
            };
        }

        public BoundingBox(Vector3 center, IEnumerable<Vertex> vertices)
        {
            Center = center;

            var minX = vertices.Select(v => v.Position.X).Min();
            var maxX = vertices.Select(v => v.Position.X).Max();
            Width = maxX - minX;

            var minY = vertices.Select(v => v.Position.Y).Min();
            var maxY = vertices.Select(v => v.Position.Y).Max();
            Height = maxY - minY;
        }

        public bool CollidesWith(Vector3 point)
        {
            return (point.X > MinX && point.X < MaxX) && (point.Y > MinY && point.Y < MaxY);
        }

        public bool CollidesWith(ICollider collider)
        {
            return false;
        }

        public bool CollidesWith(BoundingSphere boundingSphere)
        {
            var closestX = (boundingSphere.Center.X > MaxX)
                ? MaxX
                : (boundingSphere.Center.X < MinX)
                    ? MinX
                    : boundingSphere.Center.X;

            var closestY = (boundingSphere.Center.Y > MaxY)
                ? MaxY
                : (boundingSphere.Center.Y < MinY)
                    ? MinY
                    : boundingSphere.Center.Y;

            var distanceSquared = Math.Pow(boundingSphere.Center.X - closestX, 2) + Math.Pow(boundingSphere.Center.Y - closestY, 2);
            return distanceSquared < Math.Pow(boundingSphere.Radius, 2);
        }

        public bool CollidesWith(BoundingBox boundingBox)
        {
            return (MinX < boundingBox.MaxX && MaxX > boundingBox.MinX) && (MinY < boundingBox.MaxY && MaxY > boundingBox.MinY);
        }
    }
}
