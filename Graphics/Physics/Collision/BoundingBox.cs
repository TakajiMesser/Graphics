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
        public float Width { get; set; }
        public float Height { get; set; }

        public float MinX => Center.X - Width / 2.0f;
        public float MaxX => Center.X + Width / 2.0f;
        public float MinY => Center.Y - Height / 2.0f;
        public float MaxY => Center.Y + Height / 2.0f;

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
            return false;
        }

        public bool CollidesWith(ICollider collider)
        {
            return false;
        }

        public bool CollidesWith(BoundingSphere boundingSphere)
        {
            return false;
        }

        public bool CollidesWith(BoundingBox boundingBox)
        {
            return false;
        }
    }
}
