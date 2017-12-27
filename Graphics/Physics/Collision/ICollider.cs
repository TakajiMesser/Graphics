using Graphics.GameObjects;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Physics.Collision
{
    public interface ICollider
    {
        Vector3 Center { get; set; }
        Dictionary<string, GameProperty> Properties { get; set; }

        bool CollidesWith(Vector3 point);
        bool CollidesWith(BoundingSphere boundingSphere);
        bool CollidesWith(BoundingBox boundingBox);
        //bool CollidesWith(ICollider collider);
    }
}
