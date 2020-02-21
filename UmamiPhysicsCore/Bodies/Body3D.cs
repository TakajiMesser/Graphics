using OpenTK;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Physics;
using SavoryPhysicsCore.Collisions;
using SavoryPhysicsCore.Helpers;
using SavoryPhysicsCore.Shapes;

namespace SavoryPhysicsCore.Bodies
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

        public ICollision GetCollision(IBody body)
        {
            if (body is Body3D body3D)
            {
                if (Shape is Sphere && body3D.Shape is Sphere)
                {
                    if (IsPhysical && body3D.IsPhysical)
                    {
                        return CollisionHelper.GetSphereSphereCollision(this, body3D);
                    }
                    else if (CollisionHelper.HasSphereSphereCollision(this, body3D))
                    {
                        return new Collision3D(this, body3D)
                        {
                            HasCollision = true
                        };
                    }
                }
                else if (Shape is Box && body3D.Shape is Box)
                {
                    if (IsPhysical && body3D.IsPhysical)
                    {
                        return CollisionHelper.GetBoxBoxCollision(this, body3D);
                    }
                    else if (CollisionHelper.HasBoxBoxCollision(this, body3D))
                    {
                        return new Collision3D(this, body3D)
                        {
                            HasCollision = true
                        };
                    }
                }
                else if (Shape is Polyhedron && body3D.Shape is Polyhedron)
                {
                    if (IsPhysical && body.IsPhysical)
                    {
                        return CollisionHelper.GetPolyhedronPolyhedronCollision(this, body3D);
                    }
                    else if (CollisionHelper.HasPolyhedronPolyhedronCollision(this, body3D))
                    {
                        return new Collision3D(this, body3D)
                        {
                            HasCollision = true
                        };
                    }
                }
                else if (Shape is Sphere && body3D.Shape is Box)
                {
                    if (IsPhysical && body3D.IsPhysical)
                    {
                        return CollisionHelper.GetSphereBoxCollision(this, body3D);
                    }
                    else if (CollisionHelper.HasSphereBoxCollision(this, body3D))
                    {
                        return new Collision3D(this, body3D)
                        {
                            HasCollision = true
                        };
                    }
                }
                else if (Shape is Box && body3D.Shape is Sphere)
                {
                    if (IsPhysical && body.IsPhysical)
                    {
                        return CollisionHelper.GetSphereBoxCollision(body3D, this);
                    }
                    else if (CollisionHelper.HasSphereBoxCollision(body3D, this))
                    {
                        return new Collision3D(body3D, this)
                        {
                            HasCollision = true
                        };
                    }
                }
                else if (Shape is Sphere && body3D.Shape is Polyhedron)
                {
                    if (IsPhysical && body.IsPhysical)
                    {
                        return CollisionHelper.GetSpherePolyhedronCollision(this, body3D);
                    }
                    else if (CollisionHelper.HasSpherePolyhedronCollision(this, body3D))
                    {
                        return new Collision3D(this, body3D)
                        {
                            HasCollision = true
                        };
                    }
                }
                else if (Shape is Polyhedron && body3D.Shape is Sphere)
                {
                    if (IsPhysical && body.IsPhysical)
                    {
                        return CollisionHelper.GetSpherePolyhedronCollision(body3D, this);
                    }
                    else if (CollisionHelper.HasSpherePolyhedronCollision(body3D, this))
                    {
                        return new Collision3D(body3D, this)
                        {
                            HasCollision = true
                        };
                    }
                }
                else if (Shape is Box && body3D.Shape is Polyhedron)
                {
                    if (IsPhysical && body3D.IsPhysical)
                    {
                        return CollisionHelper.GetBoxPolyhedronCollision(this, body3D);
                    }
                    else if (CollisionHelper.HasBoxPolyhedronCollision(this, body3D))
                    {
                        return new Collision3D(this, body3D)
                        {
                            HasCollision = true
                        };
                    }
                }
                else if (Shape is Polyhedron && body3D.Shape is Box)
                {
                    if (IsPhysical && body3D.IsPhysical)
                    {
                        return CollisionHelper.GetBoxPolyhedronCollision(body3D, this);
                    }
                    else if (CollisionHelper.HasBoxPolyhedronCollision(body3D, this))
                    {
                        return new Collision3D(body3D, this)
                        {
                            HasCollision = true
                        };
                    }
                }

                return new Collision3D(this, body3D);
            }
            else
            {
                return null;
            }
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

            // TODO - Maybe throw NotImplementedException?
            return false;
        }
    }
}
