using Graphics.Meshes;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Physics.Collision
{
    public class BoundingSphere : ICollider
    {
        public Vector3 Center { get; set; }
        public float Radius { get; set; }

        public BoundingSphere(Vector3 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public BoundingSphere(IEnumerable<MeshVertex> vertices)
        {
            var maxDistanceSquared = vertices.Select(v => v.Position.LengthSquared).Max();
            Radius = (float)Math.Sqrt(maxDistanceSquared);
        }

        public bool CollidesWith(Vector3 point)
        {
            var distanceSquared = Math.Pow(point.X - Center.X, 2.0f) + Math.Pow(point.Y - Center.Y, 2.0f) + Math.Pow(point.Z - Center.Z, 2.0f);
            return distanceSquared < Math.Pow(Radius, 2.0f);
        }

        public bool CollidesWith(ICollider collider)
        {
            return false;
        }

        public bool CollidesWith(BoundingSphere boundingSphere)
        {
            var distanceSquared = Math.Pow(Center.X - boundingSphere.Center.X, 2.0f) + Math.Pow(Center.Y - boundingSphere.Center.Y, 2.0f) + Math.Pow(Center.Z - boundingSphere.Center.Z, 2.0f);
            return distanceSquared < Math.Pow(Radius + boundingSphere.Radius, 2.0f);
        }

        public bool CollidesWith(BoundingBox boundingBox)
        {
            throw new NotImplementedException();

            var distanceSquared = Math.Pow(Math.Max(boundingBox.MinXMinY.X, Math.Min(Center.X, boundingBox.MaxXMaxY.X)), 2.0f)
                + Math.Pow(Math.Max(boundingBox.MinXMinY.Y, Math.Min(Center.Y, boundingBox.MaxXMaxY.Y)), 2.0f)
                /*+ Math.Pow(Math.Max(boundingBox.MinXMinY.Z, Math.Min(Center.Z, boundingBox.MaxXMaxY.Z)), 2.0f)*/;

            return distanceSquared < Math.Pow(Radius, 2.0f);
        }
    }
}
