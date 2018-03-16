using OpenTK;
using System;
using System.Linq;
using TakoEngine.Entities;

namespace TakoEngine.Physics.Collision
{
    public class BoundingSphere : Bounds
    {
        public float Radius { get; set; }

        public BoundingSphere(Actor actor) : base(actor)
        {
            var maxDistanceSquared = actor.Model.Vertices.Select(v => v.LengthSquared).Max();
            Radius = (float)Math.Sqrt(maxDistanceSquared);
        }

        public BoundingSphere(Brush brush) : base(brush)
        {
            var maxDistanceSquared = brush.Vertices.Select(v => v.LengthSquared).Max();
            Radius = (float)Math.Sqrt(maxDistanceSquared);
        }

        public override bool CollidesWith(Vector3 point)
        {
            var distanceSquared = Math.Pow(point.X - Center.X, 2.0f) + Math.Pow(point.Y - Center.Y, 2.0f) + Math.Pow(point.Z - Center.Z, 2.0f);
            return distanceSquared < Math.Pow(Radius, 2.0f);
        }

        public override bool CollidesWith(Bounds collider) => throw new NotImplementedException();

        public override bool CollidesWith(BoundingCircle boundingCircle)
        {
            throw new NotImplementedException();
        }

        /*public override bool CollidesWith(BoundingSphere boundingSphere)
        {
            var distanceSquared = Math.Pow(Center.X - boundingSphere.Center.X, 2.0f) + Math.Pow(Center.Y - boundingSphere.Center.Y, 2.0f) + Math.Pow(Center.Z - boundingSphere.Center.Z, 2.0f);
            return distanceSquared < Math.Pow(Radius + boundingSphere.Radius, 2.0f);
        }*/

        public override bool CollidesWith(BoundingBox boundingBox) => throw new NotImplementedException();//HasCollision(this, boundingBox);

        public override Vector3 GetBorder(Vector3 direction) => Center + (direction.Normalized() * Radius);
    }
}
