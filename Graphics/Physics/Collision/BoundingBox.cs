using Graphics.Meshes;
using Graphics.Vertices;
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
        public Vector3 MinXMinY { get; set; }
        public Vector3 MinXMaxY { get; set; }
        public Vector3 MaxXMinY { get; set; }
        public Vector3 MaxXMaxY { get; set; }

        public BoundingBox(Vector3 minXMinY, Vector3 minXMaxY, Vector3 maxXMinY, Vector3 maxXMaxY)
        {
            MinXMinY = minXMinY;
            MinXMaxY = minXMaxY;
            MaxXMinY = maxXMinY;
            MaxXMaxY = maxXMaxY;
        }

        public BoundingBox(IEnumerable<Vertex> vertices)
        {

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
