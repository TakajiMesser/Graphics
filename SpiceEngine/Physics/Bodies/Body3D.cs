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
        public BodyStates State { get; set; }
        public Shape3D Shape { get; }

        public Vector3 Position { get; set; }
        public float Restitution { get; set; }

        public bool IsMovable => this is RigidBody3D || this is SoftBody3D;
        public bool IsPhysical { get; set; }

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
                if (IsPhysical && body.IsPhysical)
                {
                    return CollisionHelper.GetSphereSphereCollision(this, body);
                }
                else if (CollisionHelper.HasSphereSphereCollision(this, body))
                {
                    return new Collision3D(this, body)
                    {
                        HasCollision = true
                    };
                }
            }
            else if (Shape is Box && body.Shape is Box)
            {
                if (IsPhysical && body.IsPhysical)
                {
                    return CollisionHelper.GetBoxBoxCollision(this, body);
                }
                else if (CollisionHelper.HasBoxBoxCollision(this, body))
                {
                    return new Collision3D(this, body)
                    {
                        HasCollision = true
                    };
                }
            }
            else if (Shape is Polyhedron && body.Shape is Polyhedron)
            {
                if (IsPhysical && body.IsPhysical)
                {
                    return CollisionHelper.GetPolyhedronPolyhedronCollision(this, body);
                }
                else if (CollisionHelper.HasPolyhedronPolyhedronCollision(this, body))
                {
                    return new Collision3D(this, body)
                    {
                        HasCollision = true
                    };
                }
            }
            else if (Shape is Sphere && body.Shape is Box)
            {
                if (IsPhysical && body.IsPhysical)
                {
                    return CollisionHelper.GetSphereBoxCollision(this, body);
                }
                else if (CollisionHelper.HasSphereBoxCollision(this, body))
                {
                    return new Collision3D(this, body)
                    {
                        HasCollision = true
                    };
                }
            }
            else if (Shape is Box && body.Shape is Sphere)
            {
                if (IsPhysical && body.IsPhysical)
                {
                    return CollisionHelper.GetSphereBoxCollision(body, this);
                }
                else if (CollisionHelper.HasSphereBoxCollision(body, this))
                {
                    return new Collision3D(body, this)
                    {
                        HasCollision = true
                    };
                }
            }
            else if (Shape is Sphere && body.Shape is Polyhedron)
            {
                if (IsPhysical && body.IsPhysical)
                {
                    return CollisionHelper.GetSpherePolyhedronCollision(this, body);
                }
                else if (CollisionHelper.HasSpherePolyhedronCollision(this, body))
                {
                    return new Collision3D(this, body)
                    {
                        HasCollision = true
                    };
                }
            }
            else if (Shape is Polyhedron && body.Shape is Sphere)
            {
                if (IsPhysical && body.IsPhysical)
                {
                    return CollisionHelper.GetSpherePolyhedronCollision(body, this);
                }
                else if (CollisionHelper.HasSpherePolyhedronCollision(body, this))
                {
                    return new Collision3D(body, this)
                    {
                        HasCollision = true
                    };
                }
            }
            else if (Shape is Box && body.Shape is Polyhedron)
            {
                if (IsPhysical && body.IsPhysical)
                {
                    return CollisionHelper.GetBoxPolyhedronCollision(this, body);
                }
                else if (CollisionHelper.GetBoxPolyhedronCollision(this, body))
                {
                    return new Collision3D(this, body)
                    {
                        HasCollision = true
                    };
                }
            }
            else if (Shape is Polyhedron && body.Shape is Box)
            {
                if (IsPhysical && body.IsPhysical)
                {
                    return CollisionHelper.GetBoxPolyhedronCollision(body, this);
                }
                else if (CollisionHelper.GetBoxPolyhedronCollision(body, this))
                {
                    return new Collision3D(body, this)
                    {
                        HasCollision = true
                    };
                }
            }

            return new Collision3D(this, body);
        }

        public bool HasCollision(Body3D body)
        {
            if (Shape is Sphere && body.Shape is Sphere)
            {
                return CollisionHelper.HasSphereSphereCollision(this, body);
            }
            else if (Shape is Box && body.Shape is Box)
            {
                return CollisionHelper.HasBoxBoxCollision(this, body);
            }
            else if (Shape is Polyhedron && body.Shape is Polyhedron)
            {
                return CollisionHelper.HasPolyhedronPolyhedronCollision(this, body);
            }
            else if (Shape is Sphere && body.Shape is Box)
            {
                return CollisionHelper.HasSphereBoxCollision(this, body);
            }
            else if (Shape is Box && body.Shape is Sphere)
            {
                return CollisionHelper.HasSphereBoxCollision(body, this);
            }
            else if (Shape is Sphere && body.Shape is Polyhedron)
            {
                return CollisionHelper.HasSpherePolyhedronCollision(this, body);
            }
            else if (Shape is Polyhedron && body.Shape is Sphere)
            {
                return CollisionHelper.HasSpherePolyhedronCollision(body, this);
            }
            else if (Shape is Box && body.Shape is Polyhedron)
            {
                return CollisionHelper.HasBoxPolyhedronCollision(this, body);
            }
            else if (Shape is Polyhedron && body.Shape is Box)
            {
                return CollisionHelper.HasBoxPolyhedronCollision(body, this);
            }

            return new Collision3D(this, body);
        }
    }
}
