using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Helpers;
using SpiceEngine.Physics.Collisions;
using SpiceEngine.Physics.Shapes;
using SpiceEngine.Utilities;
using System;

namespace SpiceEngine.Physics.Bodies
{
    public abstract class Body3D : IBody
    {
        public int EntityID { get; }
        public Shape3D Shape { get; }
        public Vector3 Position { get; set; }
        public float Restitution { get; set; }

        public Body3D(IEntity entity, Shape3D shape)
        {
            EntityID = entity.ID;
            Shape = shape;
            Position = entity.Position;
        }

        public Collision3D GetCollision(Body3D body)
        {
            if (Shape is Sphere && body.Shape is Sphere)
            {
                return GetSphereSphereCollision(body);
            }
            else if (Shape is Box && body.Shape is Box)
            {
                return GetBoxBoxCollision(body);
            }
            else if (Shape is Polyhedron && body.Shape is Polyhedron)
            {
                return GetPolyhedronPolyhedronCollision(body);
            }
            else if (Shape is Sphere && body.Shape is Box)
            {
                return GetSphereBoxCollision(body);
            }
            else if (Shape is Box && body.Shape is Sphere)
            {
                return body.GetSphereBoxCollision(this);
            }
            else if (Shape is Sphere && body.Shape is Polyhedron)
            {
                return GetSpherePolyhedronCollision(body);
            }
            else if (Shape is Polyhedron && body.Shape is Sphere)
            {
                return body.GetSpherePolyhedronCollision(this);
            }
            else if (Shape is Box && body.Shape is Polyhedron)
            {
                return GetBoxPolyhedronCollision(body);
            }
            else if (Shape is Polyhedron && body.Shape is Box)
            {
                return body.GetBoxPolyhedronCollision(this);
            }

            return new Collision3D(this, body);
        }

        private Collision3D GetSphereSphereCollision(Body3D body)
        {
            var collision = new Collision3D(this, body);

            var sphereA = (Sphere)Shape;
            var sphereB = (Sphere)body.Shape;

            var radius = sphereA.Radius + sphereB.Radius;
            var normal = body.Position - Position;
            var distanceSquared = normal.LengthSquared;

            if (distanceSquared < radius * radius)
            {
                var distance = (float)Math.Sqrt(distanceSquared);

                if (distance == 0.0f)
                {
                    collision.PenetrationDepth = sphereA.Radius;
                    collision.ContactNormal = new Vector3(1, 0, 0);
                    collision.ContactPoints.Add(Position);
                }
                else
                {
                    collision.PenetrationDepth = radius - distance;
                    collision.ContactNormal = normal / distance;
                    collision.ContactPoints.Add(normal * sphereA.Radius + Position);
                }
            }

            return collision;
        }

        private Collision3D GetBoxBoxCollision(Body3D body)
        {
            var collision = new Collision3D(this, body);

            var boxA = (Box)Shape;
            var boxB = (Box)body.Shape;

            var doesCollide = Position.X - boxA.Width / 2.0f < body.Position.X + boxB.Width / 2.0f
                && Position.X + boxA.Width / 2.0f > body.Position.X - boxB.Width / 2.0f
                && Position.Y - boxA.Height / 2.0f < body.Position.Y + boxB.Height / 2.0f
                && Position.Y + boxA.Height / 2.0f > body.Position.Y - boxB.Height / 2.0f;

            // Collision is "how far has shape A penetrated shape B?"


            // Is the left-most position on BoxA LEFT of the right-most position on BoxB?
            // PositionX - boxA.Width / 2.0f < body.Position.X + boxB.Width / 2.0f

            // Is the right-most position on BoxA RIGHT of the left-most position on BoxB?
            // Position.X + boxA.Width / 2.0f > body.Position.X - boxB.Width / 2.0f


            /*bool collides = positionA.X - boxA.Width / 2.0f < positionB.X + boxB.Width / 2.0f
                && positionA.X + boxA.Width / 2.0f > positionB.X - boxB.Width / 2.0f
                && positionA.Y - boxA.Height / 2.0f < positionB.Y + boxB.Height / 2.0f
                && positionA.Y + boxA.Height / 2.0f > positionB.Y - boxB.Height / 2.0f
                && positionA.Z - boxA.Depth / 2.0f < positionB.Z + boxB.Depth / 2.0f
                && positionA.Z + boxA.Depth / 2.0f > positionB.Z - boxB.Depth / 2.0f;

            if (collides)
            {
                var minXA = positionA.X - boxA.Width / 2.0f;
                var maxXA = positionA.X + boxA.Width / 2.0f;
                var minYA = positionA.Y - boxA.Height / 2.0f;
                var maxYA = positionA.Y + boxA.Height / 2.0f;
                var minZA = positionA.Z - boxA.Depth / 2.0f;
                var maxZA = positionA.Z + boxA.Depth / 2.0f;

                var minXB = positionB.X - boxB.Width / 2.0f;
                var maxXB = positionB.X + boxB.Width / 2.0f;
                var minYB = positionB.Y - boxB.Height / 2.0f;
                var maxYB = positionB.Y + boxB.Height / 2.0f;
                var minZB = positionB.Z - boxB.Depth / 2.0f;
                var maxZB = positionB.Z + boxB.Depth / 2.0f;
            }*/

            return collision;
        }

        private Collision3D GetPolyhedronPolyhedronCollision(Body3D body)
        {
            var collision = new Collision3D(this, body);

            var polygonA = (Polyhedron)Shape;
            var polygonB = (Polyhedron)body.Shape;

            if (MinkowskiHelper.GenerateSimplex(this, body))
            {
                // The bodies are colliding
            }

            return collision;
        }

        private Collision3D GetSphereBoxCollision(Body3D body)
        {
            var collision = new Collision3D(this, body);

            var sphere = (Sphere)Shape;
            var box = (Box)body.Shape;

            var contactPoint = new Vector3()
            {
                X = MathHelper.Clamp(Position.X, body.Position.X - box.Width / 2.0f, body.Position.X + box.Width / 2.0f),
                Y = MathHelper.Clamp(Position.Y, body.Position.Y - box.Height / 2.0f, body.Position.Y + box.Height / 2.0f),
                Z = MathHelper.Clamp(Position.Z, body.Position.Z - box.Depth / 2.0f, body.Position.Z + box.Depth / 2.0f)
            };

            var offset = Position - contactPoint;

            if (offset.IsSignificant())
            {
                var offsetLengthSquared = offset.LengthSquared;

                if (offsetLengthSquared < sphere.Radius * sphere.Radius)
                {
                    // The sphere center is outside the box
                    var offsetLength = (float)Math.Sqrt(offsetLengthSquared);

                    collision.ContactNormal = offset / offsetLength;
                    collision.ContactPoints.Add(contactPoint);
                    collision.PenetrationDepth = sphere.Radius + Vector3.Dot(-offset, collision.ContactNormal);//offsetLength;
                }
            }
            else
            {
                // The sphere center is inside the box
                var penetration = Position - body.Position;

                var penetrationDepths = new Vector3()
                {
                    X = penetration.X < 0 ? penetration.X + box.Width / 2.0f : box.Width / 2.0f - penetration.X,
                    Y = penetration.Y < 0 ? penetration.Y + box.Height / 2.0f : box.Height / 2.0f - penetration.Y,
                    Z = penetration.Z < 0 ? penetration.Z + box.Depth / 2.0f : box.Depth / 2.0f - penetration.Z
                };

                // This is wrong, but for now we want to at least detect the collision...
                //collision.ContactPoints.Add(Position);
                collision.ContactPoints.Add(contactPoint);

                if (penetrationDepths.X < penetrationDepths.Y && penetrationDepths.X < penetrationDepths.Z)
                {
                    collision.ContactNormal = penetration.X > 0 ? Vector3.UnitX : -Vector3.UnitX;
                    collision.PenetrationDepth = penetrationDepths.X;
                }
                else if (penetrationDepths.Y < penetrationDepths.Z)
                {
                    collision.ContactNormal = penetration.Y > 0 ? Vector3.UnitY : -Vector3.UnitY;
                    collision.PenetrationDepth = penetrationDepths.Y;
                }
                else
                {
                    collision.ContactNormal = penetration.Z > 0 ? Vector3.UnitZ : -Vector3.UnitZ;
                    collision.PenetrationDepth = penetrationDepths.Z;
                }                

                // Need to get intersection of offset vector and first box face it touches
                // We can check against the 2-3 face planes we need to based on the direction of the vector
            }
            
            return collision;
        }

        private Collision3D GetSpherePolyhedronCollision(Body3D body)
        {
            var collision = new Collision3D(this, body);

            var sphere = (Sphere)Shape;
            var polygon = (Polyhedron)body.Shape;

            return collision;
        }

        private Collision3D GetBoxPolyhedronCollision(Body3D body)
        {
            var collision = new Collision3D(this, body);

            var box = (Box)Shape;
            var polygon = (Polyhedron)body.Shape;

            return collision;
        }

        private void GetMinkowskiSimplex(Polyhedron polyhedronA, Polyhedron polyhedronB)
        {
            
        }
    }
}
