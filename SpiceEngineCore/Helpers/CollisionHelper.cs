using OpenTK;
using SpiceEngineCore.Physics.Bodies;
using SpiceEngineCore.Physics.Collisions;
using SpiceEngineCore.Physics.Shapes;
using SpiceEngineCore.Utilities;
using System;

namespace SpiceEngineCore.Helpers
{
    public static class CollisionHelper
    {
        public static bool HasSphereSphereCollision(Body3D bodyA, Body3D bodyB)
        {
            var sphereA = (Sphere)bodyA.Shape;
            var sphereB = (Sphere)bodyB.Shape;

            var radius = sphereA.Radius + sphereB.Radius;
            var normal = bodyB.Position - bodyA.Position;
            var distanceSquared = normal.LengthSquared;

            return distanceSquared < radius * radius;
        }

        public static Collision3D GetSphereSphereCollision(Body3D bodyA, Body3D bodyB)
        {
            var collision = new Collision3D(bodyA, bodyB);

            var sphereA = (Sphere)bodyA.Shape;
            var sphereB = (Sphere)bodyB.Shape;

            var radius = sphereA.Radius + sphereB.Radius;
            var normal = bodyB.Position - bodyA.Position;
            var distanceSquared = normal.LengthSquared;

            if (distanceSquared < radius * radius)
            {
                var distance = (float)Math.Sqrt(distanceSquared);

                if (distance == 0.0f)
                {
                    collision.HasCollision = true;
                    collision.PenetrationDepth = sphereA.Radius;
                    collision.ContactNormal = new Vector3(1, 0, 0);
                    collision.ContactPoint = bodyA.Position;
                }
                else
                {
                    collision.HasCollision = true;
                    collision.PenetrationDepth = radius - distance;
                    collision.ContactNormal = normal / distance;
                    collision.ContactPoint = normal * sphereA.Radius + bodyA.Position;
                }
            }

            return collision;
        }

        public static bool HasBoxBoxCollision(Body3D bodyA, Body3D bodyB)
        {
            var boxA = (Box)bodyA.Shape;
            var boxB = (Box)bodyB.Shape;

            return bodyA.Position.X - boxA.Width / 2.0f < bodyB.Position.X + boxB.Width / 2.0f
                && bodyA.Position.X + boxA.Width / 2.0f > bodyB.Position.X - boxB.Width / 2.0f
                && bodyA.Position.Y - boxA.Height / 2.0f < bodyB.Position.Y + boxB.Height / 2.0f
                && bodyA.Position.Y + boxA.Height / 2.0f > bodyB.Position.Y - boxB.Height / 2.0f
                && bodyA.Position.Z - boxA.Depth / 2.0f < bodyB.Position.Z + boxB.Depth / 2.0f
                && bodyA.Position.Z + boxA.Depth / 2.0f > bodyB.Position.Z - boxB.Depth / 2.0f;
        }

        public static Collision3D GetBoxBoxCollision(Body3D bodyA, Body3D bodyB)
        {
            var collision = new Collision3D(bodyA, bodyB);

            var boxA = (Box)bodyA.Shape;
            var boxB = (Box)bodyB.Shape;

            var contactPointB = new Vector3()
            {
                X = MathHelper.Clamp(bodyA.Position.X, bodyB.Position.X - boxB.Width / 2.0f, bodyB.Position.X + boxB.Width / 2.0f),
                Y = MathHelper.Clamp(bodyA.Position.Y, bodyB.Position.Y - boxB.Height / 2.0f, bodyB.Position.Y + boxB.Height / 2.0f),
                Z = MathHelper.Clamp(bodyA.Position.Z, bodyB.Position.Z - boxB.Depth / 2.0f, bodyB.Position.Z + boxB.Depth / 2.0f)
            };

            var offset = bodyA.Position - contactPointB;

            // TODO - This should probably be checking for being positive AND significant, not just for significance
            if (offset.IsSignificant())
            {
                var offsetLengthSquared = offset.LengthSquared;

                var contactPointA = boxA.GetFurthestPointInDirection(-offset.Normalized());
                var offsetLengthASquared = contactPointA.LengthSquared;

                if (offsetLengthSquared < offsetLengthASquared)
                {
                    // BoxA center is outside BoxB
                    var offsetLength = (float)Math.Sqrt(offsetLengthSquared);

                    collision.HasCollision = true;
                    collision.ContactNormal = offset / offsetLength;
                    collision.ContactPoint = contactPointB;
                    collision.PenetrationDepth = (float)Math.Sqrt(offsetLengthASquared) + Vector3.Dot(-offset, collision.ContactNormal);
                    // TODO - Fix this
                    //collision.PenetrationDepth = sphere.Radius + Vector3.Dot(-offset, collision.ContactNormal);//offsetLength;
                }
            }
            else
            {
                // BoxA center is inside BoxB
                //var penetration = bodyA.Position - bodyB.Position;

                // Vector V = Position - body.Position;
                // X = body.Position.X + penetration.X * t
                // t = (X - body.Position.X) / penetration.X
                var v = bodyA.Position - bodyB.Position;

                // Check against the X-face
                var x = bodyA.Position.X >= bodyB.Position.X ? bodyB.Position.X + boxB.Width / 2.0f : bodyB.Position.X - boxB.Width / 2.0f;
                var tx = (x - bodyB.Position.X) / v.X;
                var xPenetration = new Vector3()
                {
                    X = x,
                    Y = bodyB.Position.Y + v.Y * tx,
                    Z = bodyB.Position.Z + v.Z * tx
                };

                // Check against the Y-face
                var y = bodyA.Position.Y >= bodyB.Position.Y ? bodyB.Position.Y + boxB.Height / 2.0f : bodyB.Position.Y - boxB.Height / 2.0f;
                var ty = (y - bodyB.Position.Y) / v.Y;
                var yPenetration = new Vector3()
                {
                    X = bodyB.Position.X + v.X * ty,
                    Y = y,
                    Z = bodyB.Position.Z + v.Z * ty
                };

                // Check against the Z-face
                var z = bodyA.Position.Z >= bodyB.Position.Z ? bodyB.Position.Z + boxB.Depth / 2.0f : bodyB.Position.Z - boxB.Depth / 2.0f;
                var tz = (z - bodyB.Position.Z) / v.Z;
                var zPenetration = new Vector3()
                {
                    X = bodyB.Position.X + v.X * tz,
                    Y = bodyB.Position.Y + v.Y * tz,
                    Z = z
                };

                var xDiff = xPenetration - bodyB.Position;
                var yDiff = yPenetration - bodyB.Position;
                var zDiff = zPenetration - bodyB.Position;

                Vector3 penetration;

                if (xDiff.LengthSquared < yDiff.LengthSquared && xDiff.LengthSquared < zDiff.LengthSquared)
                {
                    collision.ContactPoint = xPenetration;
                    penetration = xDiff - bodyA.Position;
                }
                else if (yDiff.LengthSquared < zDiff.LengthSquared)
                {
                    collision.ContactPoint = yPenetration;
                    penetration = yDiff - bodyA.Position;
                }
                else
                {
                    collision.ContactPoint = zPenetration;
                    penetration = zDiff - bodyA.Position;
                }

                var penetrationLength = penetration.Length;
                collision.ContactNormal = penetration / penetrationLength;
                collision.PenetrationDepth = penetrationLength + boxA.GetFurthestPointInDirection(-collision.ContactNormal).Length;
                collision.HasCollision = true;
            }

            return collision;
        }

        public static bool HasPolyhedronPolyhedronCollision(Body3D bodyA, Body3D bodyB)
        {
            var polyhedronA = (Polyhedron)bodyA.Shape;
            var polyhedronB = (Polyhedron)bodyB.Shape;

            return MinkowskiHelper.GenerateSimplex(bodyA, bodyB);
        }

        public static Collision3D GetPolyhedronPolyhedronCollision(Body3D bodyA, Body3D bodyB)
        {
            var collision = new Collision3D(bodyA, bodyB);

            var polyhedronA = (Polyhedron)bodyA.Shape;
            var polyhedronB = (Polyhedron)bodyB.Shape;

            if (MinkowskiHelper.GenerateSimplex(bodyA, bodyB))
            {
                // The bodies are colliding
                collision.HasCollision = true;
            }

            return collision;
        }

        public static bool HasSphereBoxCollision(Body3D bodyA, Body3D bodyB)
        {
            var sphere = (Sphere)bodyA.Shape;
            var box = (Box)bodyB.Shape;

            var contactPoint = new Vector3()
            {
                X = MathHelper.Clamp(bodyA.Position.X, bodyB.Position.X - box.Width / 2.0f, bodyB.Position.X + box.Width / 2.0f),
                Y = MathHelper.Clamp(bodyA.Position.Y, bodyB.Position.Y - box.Height / 2.0f, bodyB.Position.Y + box.Height / 2.0f),
                Z = MathHelper.Clamp(bodyA.Position.Z, bodyB.Position.Z - box.Depth / 2.0f, bodyB.Position.Z + box.Depth / 2.0f)
            };

            var offset = bodyA.Position - contactPoint;

            if (offset.IsSignificant())
            {
                // The sphere center is potentially outside the box
                var offsetLengthSquared = offset.LengthSquared;
                return offsetLengthSquared < sphere.Radius * sphere.Radius;
            }
            else
            {
                // The sphere center is inside the box
                return true;
            }
        }

        public static Collision3D GetSphereBoxCollision(Body3D bodyA, Body3D bodyB)
        {
            var collision = new Collision3D(bodyA, bodyB);

            var sphere = (Sphere)bodyA.Shape;
            var box = (Box)bodyB.Shape;

            var contactPoint = new Vector3()
            {
                X = MathHelper.Clamp(bodyA.Position.X, bodyB.Position.X - box.Width / 2.0f, bodyB.Position.X + box.Width / 2.0f),
                Y = MathHelper.Clamp(bodyA.Position.Y, bodyB.Position.Y - box.Height / 2.0f, bodyB.Position.Y + box.Height / 2.0f),
                Z = MathHelper.Clamp(bodyA.Position.Z, bodyB.Position.Z - box.Depth / 2.0f, bodyB.Position.Z + box.Depth / 2.0f)
            };

            var offset = bodyA.Position - contactPoint;

            if (offset.IsSignificant())
            {
                var offsetLengthSquared = offset.LengthSquared;

                if (offsetLengthSquared < sphere.Radius * sphere.Radius)
                {
                    // The sphere center is outside the box
                    var offsetLength = (float)Math.Sqrt(offsetLengthSquared);

                    collision.HasCollision = true;
                    collision.ContactNormal = offset / offsetLength;
                    collision.ContactPoint = contactPoint;
                    collision.PenetrationDepth = sphere.Radius + Vector3.Dot(-offset, collision.ContactNormal);//offsetLength;
                }
            }
            else
            {
                // The sphere center is inside the box
                collision.HasCollision = true;
                //var penetration = bodyA.Position - bodyB.Position;

                // Vector V = Position - bodyB.Position;
                // X = bodyB.Position.X + penetration.X * t
                // t = (X - bodyB.Position.X) / penetration.X
                var v = bodyA.Position - bodyB.Position;

                // Check against the X-face
                var x = bodyA.Position.X >= bodyB.Position.X ? bodyB.Position.X + box.Width / 2.0f : bodyB.Position.X - box.Width / 2.0f;
                var tx = (x - bodyB.Position.X) / v.X;
                var xPenetration = new Vector3()
                {
                    X = x,
                    Y = bodyB.Position.Y + v.Y * tx,
                    Z = bodyB.Position.Z + v.Z * tx
                };

                // Check against the Y-face
                var y = bodyA.Position.Y >= bodyB.Position.Y ? bodyB.Position.Y + box.Height / 2.0f : bodyB.Position.Y - box.Height / 2.0f;
                var ty = (y - bodyB.Position.Y) / v.Y;
                var yPenetration = new Vector3()
                {
                    X = bodyB.Position.X + v.X * ty,
                    Y = y,
                    Z = bodyB.Position.Z + v.Z * ty
                };

                // Check against the Z-face
                var z = bodyA.Position.Z >= bodyB.Position.Z ? bodyB.Position.Z + box.Depth / 2.0f : bodyB.Position.Z - box.Depth / 2.0f;
                var tz = (z - bodyB.Position.Z) / v.Z;
                var zPenetration = new Vector3()
                {
                    X = bodyB.Position.X + v.X * tz,
                    Y = bodyB.Position.Y + v.Y * tz,
                    Z = z
                };

                var xDiff = xPenetration - bodyB.Position;
                var yDiff = yPenetration - bodyB.Position;
                var zDiff = zPenetration - bodyB.Position;

                if (xDiff.LengthSquared < yDiff.LengthSquared && xDiff.LengthSquared < zDiff.LengthSquared)
                {
                    collision.ContactPoint = xPenetration;

                    var penetration = xDiff - bodyA.Position;
                    var penetrationLength = penetration.Length;
                    collision.PenetrationDepth = penetrationLength + sphere.Radius;
                    collision.ContactNormal = penetration / penetrationLength;
                }
                else if (yDiff.LengthSquared < zDiff.LengthSquared)
                {
                    collision.ContactPoint = yPenetration;

                    var penetration = yDiff - bodyA.Position;
                    var penetrationLength = penetration.Length;
                    collision.PenetrationDepth = penetrationLength + sphere.Radius;
                    collision.ContactNormal = penetration / penetrationLength;
                }
                else
                {
                    collision.ContactPoint = zPenetration;

                    var penetration = zDiff - bodyA.Position;
                    var penetrationLength = penetration.Length;
                    collision.PenetrationDepth = penetrationLength + sphere.Radius;
                    collision.ContactNormal = penetration / penetrationLength;
                }

                /*var penetrationDepths = new Vector3()
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
                }*/

                // Need to get intersection of offset vector and first box face it touches
                // We can check against the 2-3 face planes we need to based on the direction of the vector
            }

            return collision;
        }

        public static bool HasSpherePolyhedronCollision(Body3D bodyA, Body3D bodyB)
        {
            return false;
        }

        public static Collision3D GetSpherePolyhedronCollision(Body3D bodyA, Body3D bodyB)
        {
            var collision = new Collision3D(bodyA, bodyB);

            var sphere = (Sphere)bodyA.Shape;
            var polygon = (Polyhedron)bodyB.Shape;

            return collision;
        }

        public static bool HasBoxPolyhedronCollision(Body3D bodyA, Body3D bodyB)
        {
            return false;
        }

        public static Collision3D GetBoxPolyhedronCollision(Body3D bodyA, Body3D bodyB)
        {
            var collision = new Collision3D(bodyA, bodyB);

            var box = (Box)bodyA.Shape;
            var polygon = (Polyhedron)bodyB.Shape;

            return collision;
        }
    }
}
