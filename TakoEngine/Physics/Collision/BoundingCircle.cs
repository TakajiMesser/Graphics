using TakoEngine.GameObjects;
using TakoEngine.Lighting;
using TakoEngine.Meshes;
using TakoEngine.Rendering.Vertices;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakoEngine.Physics.Collision
{
    public class BoundingCircle : Bounds
    {
        public float Radius { get; set; }

        public BoundingCircle(GameObject gameObject) : base(gameObject)
        {
            var maxDistanceSquared = gameObject.Model.Vertices.Select(v => v.Xy.LengthSquared).Max();
            Radius = (float)Math.Sqrt(maxDistanceSquared);
        }

        public BoundingCircle(Brush brush) : base(brush)
        {
            var maxDistanceSquared = brush.Vertices.Select(v => v.Xy.LengthSquared).Max();
            Radius = (float)Math.Sqrt(maxDistanceSquared);
        }

        public BoundingCircle(Light light) : base(light)
        {
            switch (light)
            {
                case PointLight pLight:
                    Radius = pLight.Radius;
                    break;
            }
        }

        public override bool CollidesWith(Vector3 point)
        {
            var distanceSquared = Math.Pow(point.X - Center.X, 2.0f) + Math.Pow(point.Y - Center.Y, 2.0f);
            return distanceSquared < Math.Pow(Radius, 2.0f);
        }

        public override bool CollidesWith(Bounds collider) => throw new NotImplementedException();

        public override bool CollidesWith(BoundingCircle boundingCircle)
        {
            var distanceSquared = Math.Pow(Center.X - boundingCircle.Center.X, 2.0f) + Math.Pow(Center.Y - boundingCircle.Center.Y, 2.0f);
            return distanceSquared < Math.Pow(Radius + boundingCircle.Radius, 2.0f);
        }

        public override bool CollidesWith(BoundingBox boundingBox) => HasCollision(this, boundingBox);

        public override Vector3 GetBorder(Vector3 direction)
        {
            var extended = Center.Xy + (direction.Xy.Normalized() * Radius);
            return new Vector3(extended.X, extended.Y, Center.Z);
        }
    }
}
