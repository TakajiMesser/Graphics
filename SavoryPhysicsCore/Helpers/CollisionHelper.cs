using OpenTK;
using SavoryPhysicsCore.Collisions;
using SavoryPhysicsCore.Shapes.ThreeDimensional;
using SpiceEngineCore.Utilities;
using System;

namespace SavoryPhysicsCore.Helpers
{
    public static class CollisionHelper
    {
        public static bool HasSphereSphereCollision(CollisionInfo collisionInfo)
        {
            var sphereA = (Sphere)collisionInfo.BodyA.Shape;
            var sphereB = (Sphere)collisionInfo.BodyB.Shape;

            var radius = sphereA.Radius + sphereB.Radius;
            var normal = collisionInfo.EntityB.Position - collisionInfo.EntityA.Position;
            var distanceSquared = normal.LengthSquared;

            return distanceSquared < radius * radius;
        }

        public static CollisionResult GetSphereSphereCollision(CollisionInfo collisionInfo)
        {
            var sphereA = (Sphere)collisionInfo.BodyA.Shape;
            var sphereB = (Sphere)collisionInfo.BodyB.Shape;

            var radius = sphereA.Radius + sphereB.Radius;
            var normal = collisionInfo.EntityB.Position - collisionInfo.EntityA.Position;
            var distanceSquared = normal.LengthSquared;

            if (distanceSquared < radius * radius)
            {
                var distance = (float)Math.Sqrt(distanceSquared);

                if (distance == 0.0f)
                {
                    return CollisionResult.FullCollision(collisionInfo, new Collision()
                    {
                        PenetrationDepth = sphereA.Radius,
                        ContactNormal = new Vector3(1, 0, 0),
                        ContactPoint = collisionInfo.EntityA.Position
                    });
                }
                else
                {
                    return CollisionResult.FullCollision(collisionInfo, new Collision()
                    {
                        PenetrationDepth = radius - distance,
                        ContactNormal = normal / distance,
                        ContactPoint = normal * sphereA.Radius + collisionInfo.EntityA.Position
                    });
                }
            }
            else
            {
                return CollisionResult.NoCollision(collisionInfo);
            }
        }

        public static bool HasBoxBoxCollision(CollisionInfo collisionInfo)
        {
            var boxA = (Box)collisionInfo.BodyA.Shape;
            var boxB = (Box)collisionInfo.BodyB.Shape;

            return collisionInfo.EntityA.Position.X - boxA.Width / 2.0f < collisionInfo.EntityB.Position.X + boxB.Width / 2.0f
                && collisionInfo.EntityA.Position.X + boxA.Width / 2.0f > collisionInfo.EntityB.Position.X - boxB.Width / 2.0f
                && collisionInfo.EntityA.Position.Y - boxA.Height / 2.0f < collisionInfo.EntityB.Position.Y + boxB.Height / 2.0f
                && collisionInfo.EntityA.Position.Y + boxA.Height / 2.0f > collisionInfo.EntityB.Position.Y - boxB.Height / 2.0f
                && collisionInfo.EntityA.Position.Z - boxA.Depth / 2.0f < collisionInfo.EntityB.Position.Z + boxB.Depth / 2.0f
                && collisionInfo.EntityA.Position.Z + boxA.Depth / 2.0f > collisionInfo.EntityB.Position.Z - boxB.Depth / 2.0f;
        }

        public static CollisionResult GetBoxBoxCollision(CollisionInfo collisionInfo)
        {
            var boxA = (Box)collisionInfo.BodyA.Shape;
            var boxB = (Box)collisionInfo.BodyB.Shape;

            var contactPointB = new Vector3()
            {
                X = MathHelper.Clamp(collisionInfo.EntityA.Position.X, collisionInfo.EntityB.Position.X - boxB.Width / 2.0f, collisionInfo.EntityB.Position.X + boxB.Width / 2.0f),
                Y = MathHelper.Clamp(collisionInfo.EntityA.Position.Y, collisionInfo.EntityB.Position.Y - boxB.Height / 2.0f, collisionInfo.EntityB.Position.Y + boxB.Height / 2.0f),
                Z = MathHelper.Clamp(collisionInfo.EntityA.Position.Z, collisionInfo.EntityB.Position.Z - boxB.Depth / 2.0f, collisionInfo.EntityB.Position.Z + boxB.Depth / 2.0f)
            };

            var offset = collisionInfo.EntityA.Position - contactPointB;

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
                    var contactNormal = offset / offsetLength;

                    return CollisionResult.FullCollision(collisionInfo, new Collision()
                    {
                        PenetrationDepth = (float)Math.Sqrt(offsetLengthASquared) + Vector3.Dot(-offset, contactNormal),
                        ContactNormal = contactNormal,
                        ContactPoint = contactPointB
                    });

                    // TODO - Fix this
                    //collision.PenetrationDepth = sphere.Radius + Vector3.Dot(-offset, collision.ContactNormal);//offsetLength;
                }
            }
            else
            {
                // BoxA center is inside BoxB
                //var penetration = collisionInfo.EntityA.Position - collisionInfo.EntityB.Position;

                // Vector V = Position - body.Position;
                // X = body.Position.X + penetration.X * t
                // t = (X - body.Position.X) / penetration.X
                var v = collisionInfo.EntityA.Position - collisionInfo.EntityB.Position;

                // Check against the X-face
                var x = collisionInfo.EntityA.Position.X >= collisionInfo.EntityB.Position.X ? collisionInfo.EntityB.Position.X + boxB.Width / 2.0f : collisionInfo.EntityB.Position.X - boxB.Width / 2.0f;
                var tx = (x - collisionInfo.EntityB.Position.X) / v.X;
                var xPenetration = new Vector3()
                {
                    X = x,
                    Y = collisionInfo.EntityB.Position.Y + v.Y * tx,
                    Z = collisionInfo.EntityB.Position.Z + v.Z * tx
                };

                // Check against the Y-face
                var y = collisionInfo.EntityA.Position.Y >= collisionInfo.EntityB.Position.Y ? collisionInfo.EntityB.Position.Y + boxB.Height / 2.0f : collisionInfo.EntityB.Position.Y - boxB.Height / 2.0f;
                var ty = (y - collisionInfo.EntityB.Position.Y) / v.Y;
                var yPenetration = new Vector3()
                {
                    X = collisionInfo.EntityB.Position.X + v.X * ty,
                    Y = y,
                    Z = collisionInfo.EntityB.Position.Z + v.Z * ty
                };

                // Check against the Z-face
                var z = collisionInfo.EntityA.Position.Z >= collisionInfo.EntityB.Position.Z ? collisionInfo.EntityB.Position.Z + boxB.Depth / 2.0f : collisionInfo.EntityB.Position.Z - boxB.Depth / 2.0f;
                var tz = (z - collisionInfo.EntityB.Position.Z) / v.Z;
                var zPenetration = new Vector3()
                {
                    X = collisionInfo.EntityB.Position.X + v.X * tz,
                    Y = collisionInfo.EntityB.Position.Y + v.Y * tz,
                    Z = z
                };

                var xDiff = xPenetration - collisionInfo.EntityB.Position;
                var yDiff = yPenetration - collisionInfo.EntityB.Position;
                var zDiff = zPenetration - collisionInfo.EntityB.Position;

                Vector3 contactPoint;
                Vector3 penetration;

                if (xDiff.LengthSquared < yDiff.LengthSquared && xDiff.LengthSquared < zDiff.LengthSquared)
                {
                    contactPoint = xPenetration;
                    penetration = xDiff - collisionInfo.EntityA.Position;
                }
                else if (yDiff.LengthSquared < zDiff.LengthSquared)
                {
                    contactPoint = yPenetration;
                    penetration = yDiff - collisionInfo.EntityA.Position;
                }
                else
                {
                    contactPoint = zPenetration;
                    penetration = zDiff - collisionInfo.EntityA.Position;
                }

                var penetrationLength = penetration.Length;
                var contactNormal = penetration / penetrationLength;

                return CollisionResult.FullCollision(collisionInfo, new Collision()
                {
                    PenetrationDepth = penetrationLength + boxA.GetFurthestPointInDirection(-contactNormal).Length,
                    ContactNormal = contactNormal,
                    ContactPoint = contactPoint
                });
            }

            return CollisionResult.NoCollision(collisionInfo);
        }

        public static bool HasPolyhedronPolyhedronCollision(CollisionInfo collisionInfo) => MinkowskiHelper.GenerateSimplex(collisionInfo);

        public static CollisionResult GetPolyhedronPolyhedronCollision(CollisionInfo collisionInfo)
        {
            if (MinkowskiHelper.GenerateSimplex(collisionInfo))
            {
                // The bodies are colliding
                return CollisionResult.LimitedCollision(collisionInfo);
            }

            return CollisionResult.NoCollision(collisionInfo);
        }

        public static bool HasSphereBoxCollision(CollisionInfo collisionInfo)
        {
            var sphere = (Sphere)collisionInfo.BodyA.Shape;
            var box = (Box)collisionInfo.BodyB.Shape;

            var contactPoint = new Vector3()
            {
                X = MathHelper.Clamp(collisionInfo.EntityA.Position.X, collisionInfo.EntityB.Position.X - box.Width / 2.0f, collisionInfo.EntityB.Position.X + box.Width / 2.0f),
                Y = MathHelper.Clamp(collisionInfo.EntityA.Position.Y, collisionInfo.EntityB.Position.Y - box.Height / 2.0f, collisionInfo.EntityB.Position.Y + box.Height / 2.0f),
                Z = MathHelper.Clamp(collisionInfo.EntityA.Position.Z, collisionInfo.EntityB.Position.Z - box.Depth / 2.0f, collisionInfo.EntityB.Position.Z + box.Depth / 2.0f)
            };

            var offset = collisionInfo.EntityA.Position - contactPoint;

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

        public static CollisionResult GetSphereBoxCollision(CollisionInfo collisionInfo)
        {
            var sphere = (Sphere)collisionInfo.BodyA.Shape;
            var box = (Box)collisionInfo.BodyB.Shape;

            var contactPoint = new Vector3()
            {
                X = MathHelper.Clamp(collisionInfo.EntityA.Position.X, collisionInfo.EntityB.Position.X - box.Width / 2.0f, collisionInfo.EntityB.Position.X + box.Width / 2.0f),
                Y = MathHelper.Clamp(collisionInfo.EntityA.Position.Y, collisionInfo.EntityB.Position.Y - box.Height / 2.0f, collisionInfo.EntityB.Position.Y + box.Height / 2.0f),
                Z = MathHelper.Clamp(collisionInfo.EntityA.Position.Z, collisionInfo.EntityB.Position.Z - box.Depth / 2.0f, collisionInfo.EntityB.Position.Z + box.Depth / 2.0f)
            };

            var offset = collisionInfo.EntityA.Position - contactPoint;

            if (offset.IsSignificant())
            {
                var offsetLengthSquared = offset.LengthSquared;

                if (offsetLengthSquared < sphere.Radius * sphere.Radius)
                {
                    // The sphere center is outside the box
                    var offsetLength = (float)Math.Sqrt(offsetLengthSquared);
                    var contactNormal = offset / offsetLength;

                    return CollisionResult.FullCollision(collisionInfo, new Collision()
                    {
                        PenetrationDepth = sphere.Radius + Vector3.Dot(-offset, contactNormal),
                        ContactNormal = contactNormal,
                        ContactPoint = contactPoint
                    });
                }
            }
            else
            {
                // The sphere center is inside the box
                //var penetration = collisionInfo.EntityA.Position - collisionInfo.EntityB.Position;

                // Vector V = Position - collisionInfo.EntityB.Position;
                // X = collisionInfo.EntityB.Position.X + penetration.X * t
                // t = (X - collisionInfo.EntityB.Position.X) / penetration.X
                var v = collisionInfo.EntityA.Position - collisionInfo.EntityB.Position;

                // Check against the X-face
                var x = collisionInfo.EntityA.Position.X >= collisionInfo.EntityB.Position.X ? collisionInfo.EntityB.Position.X + box.Width / 2.0f : collisionInfo.EntityB.Position.X - box.Width / 2.0f;
                var tx = (x - collisionInfo.EntityB.Position.X) / v.X;
                var xPenetration = new Vector3()
                {
                    X = x,
                    Y = collisionInfo.EntityB.Position.Y + v.Y * tx,
                    Z = collisionInfo.EntityB.Position.Z + v.Z * tx
                };

                // Check against the Y-face
                var y = collisionInfo.EntityA.Position.Y >= collisionInfo.EntityB.Position.Y ? collisionInfo.EntityB.Position.Y + box.Height / 2.0f : collisionInfo.EntityB.Position.Y - box.Height / 2.0f;
                var ty = (y - collisionInfo.EntityB.Position.Y) / v.Y;
                var yPenetration = new Vector3()
                {
                    X = collisionInfo.EntityB.Position.X + v.X * ty,
                    Y = y,
                    Z = collisionInfo.EntityB.Position.Z + v.Z * ty
                };

                // Check against the Z-face
                var z = collisionInfo.EntityA.Position.Z >= collisionInfo.EntityB.Position.Z ? collisionInfo.EntityB.Position.Z + box.Depth / 2.0f : collisionInfo.EntityB.Position.Z - box.Depth / 2.0f;
                var tz = (z - collisionInfo.EntityB.Position.Z) / v.Z;
                var zPenetration = new Vector3()
                {
                    X = collisionInfo.EntityB.Position.X + v.X * tz,
                    Y = collisionInfo.EntityB.Position.Y + v.Y * tz,
                    Z = z
                };

                var xDiff = xPenetration - collisionInfo.EntityB.Position;
                var yDiff = yPenetration - collisionInfo.EntityB.Position;
                var zDiff = zPenetration - collisionInfo.EntityB.Position;

                Vector3 penetrationPoint;

                if (xDiff.LengthSquared < yDiff.LengthSquared && xDiff.LengthSquared < zDiff.LengthSquared)
                {
                    penetrationPoint = xPenetration;

                    var penetration = xDiff - collisionInfo.EntityA.Position;
                    var penetrationLength = penetration.Length;

                    return CollisionResult.FullCollision(collisionInfo, new Collision()
                    {
                        PenetrationDepth = penetrationLength + sphere.Radius,
                        ContactNormal = penetration / penetrationLength,
                        ContactPoint = penetrationPoint
                    });
                }
                else if (yDiff.LengthSquared < zDiff.LengthSquared)
                {
                    penetrationPoint = yPenetration;

                    var penetration = yDiff - collisionInfo.EntityA.Position;
                    var penetrationLength = penetration.Length;

                    return CollisionResult.FullCollision(collisionInfo, new Collision()
                    {
                        PenetrationDepth = penetrationLength + sphere.Radius,
                        ContactNormal = penetration / penetrationLength,
                        ContactPoint = penetrationPoint
                    });
                }
                else
                {
                    penetrationPoint = zPenetration;

                    var penetration = zDiff - collisionInfo.EntityA.Position;
                    var penetrationLength = penetration.Length;

                    return CollisionResult.FullCollision(collisionInfo, new Collision()
                    {
                        PenetrationDepth = penetrationLength + sphere.Radius,
                        ContactNormal = penetration / penetrationLength,
                        ContactPoint = penetrationPoint
                    });
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

            return CollisionResult.NoCollision(collisionInfo);
        }

        public static bool HasSpherePolyhedronCollision(CollisionInfo collisionInfo) => false;

        public static CollisionResult GetSpherePolyhedronCollision(CollisionInfo collisionInfo) => CollisionResult.NoCollision(collisionInfo);

        public static bool HasBoxPolyhedronCollision(CollisionInfo collisionInfo) => false;

        public static CollisionResult GetBoxPolyhedronCollision(CollisionInfo collisionInfo) => CollisionResult.NoCollision(collisionInfo);
    }
}
