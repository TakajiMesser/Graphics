﻿using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Physics.Collisions;
using SpiceEngine.Physics.Shapes;
using SpiceEngine.Rendering.PostProcessing;
using System;

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
            if (Shape is Sphere && body.Shape is Sphere)
            {
                return GetSphereSphereCollision(body);
            }
            else if (Shape is Box && body.Shape is Box)
            {
                return GetBoxBoxCollision(body);
            }
            else if (Shape is Polygon3D && body.Shape is Polygon3D)
            {
                return GetPolygonPolygonCollision(body);
            }
            else if (Shape is Sphere && body.Shape is Box)
            {
                return GetSphereBoxCollision(body);
            }
            else if (Shape is Box && body.Shape is Sphere)
            {
                return body.GetSphereBoxCollision(this);
            }
            else if (Shape is Sphere && body.Shape is Polygon3D)
            {
                return GetSpherePolygonCollision(body);
            }
            else if (Shape is Polygon3D && body.Shape is Sphere)
            {
                return body.GetSpherePolygonCollision(this);
            }
            else if (Shape is Box && body.Shape is Polygon3D)
            {
                return GetBoxPolygonCollision(body);
            }
            else if (Shape is Polygon3D && body.Shape is Box)
            {
                return body.GetBoxPolygonCollision(this);
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

        private Collision3D GetPolygonPolygonCollision(Body3D body)
        {
            var collision = new Collision3D(this, body);

            var polygonA = (Polygon3D)Shape;
            var polygonB = (Polygon3D)body.Shape;

            return collision;
        }

        private Collision3D GetSphereBoxCollision(Body3D body)
        {
            var collision = new Collision3D(this, body);

            var sphere = (Sphere)Shape;
            var box = (Box)body.Shape;

            var closestPoint = new Vector3()
            {
                X = MathHelper.Clamp(Position.X, body.Position.X - box.Width / 2.0f, body.Position.X + box.Width / 2.0f),
                Y = MathHelper.Clamp(Position.Y, body.Position.Y - box.Height / 2.0f, body.Position.Y + box.Height / 2.0f),
                Z = MathHelper.Clamp(Position.Z, body.Position.Z - box.Depth / 2.0f, body.Position.Z + box.Depth / 2.0f)
            };

            var offset = Position - closestPoint;
            if (offset != Vector3.Zero)
            {
                var offsetLengthSquared = offset.LengthSquared;

                if (offsetLengthSquared < sphere.Radius * sphere.Radius)
                {
                    var offsetLength = (float)Math.Sqrt(offsetLengthSquared);

                    collision.PenetrationDepth = offsetLength;
                    collision.ContactNormal = offset / offsetLength;
                    collision.ContactPoints.Add(closestPoint);

                    LogManager.LogToScreen("Sphere-Box Collision Found. PenetrationDepth = " + offsetLength);
                }
            }
            
            return collision;
        }

        private Collision3D GetSpherePolygonCollision(Body3D body)
        {
            var collision = new Collision3D(this, body);

            var sphere = (Sphere)Shape;
            var polygon = (Polygon3D)body.Shape;

            return collision;
        }

        private Collision3D GetBoxPolygonCollision(Body3D body)
        {
            var collision = new Collision3D(this, body);

            var box = (Box)Shape;
            var polygon = (Polygon3D)body.Shape;

            return collision;
        }
    }
}
