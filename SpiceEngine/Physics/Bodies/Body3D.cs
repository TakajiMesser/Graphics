using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Physics.Collisions;
using SpiceEngine.Physics.Shapes;

namespace SpiceEngine.Physics.Bodies
{
    public abstract class Body3D : IBody
    {
        public int EntityID { get; }
        public IShape Shape { get; }
        public Vector3 Position { get; set; }
        public float Restitution { get; set; }

        public Body3D(IEntity entity, IShape shape)
        {
            EntityID = entity.ID;
            Shape = shape;
            Position = entity.Position;
        }

        public Collision3D GetCollision(Body3D body)
        {
            return new Collision3D(this, body);
        }

        private Collision3D GetSphereBoxCollision(Body3D body)
        {
            return new Collision3D(this, body);
        }

        private Collision3D GetBoxBoxCollision(Body3D body)
        {
            return new Collision3D(this, body);
        }

        private Collision3D GetPolygonPolygonCollision(Body3D body)
        {
            return new Collision3D(this, body);
        }

        private Collision3D GetSpherePolygonCollision(Body3D body)
        {
            return new Collision3D(this, body);
        }

        private Collision3D GetBoxPolygonCollision(Body3D body)
        {
            return new Collision3D(this, body);
        }
    }
}
