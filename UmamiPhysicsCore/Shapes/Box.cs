﻿using OpenTK;
using SavoryPhysicsCore.Collisions;
using SpiceEngineCore.Physics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SavoryPhysicsCore.Shapes
{
    public class Box : Shape3D
    {
        public float Width { get; private set; }
        public float Height { get; private set; }
        public float Depth { get; private set; }

        public Box(int entityID, IEnumerable<Vector3> vertices) : base(entityID)
        {
            var minX = vertices.Select(v => v.X).Min();
            var maxX = vertices.Select(v => v.X).Max();
            Width = maxX - minX;

            var minY = vertices.Select(v => v.Y).Min();
            var maxY = vertices.Select(v => v.Y).Max();
            Height = maxY - minY;

            var minZ = vertices.Select(v => v.Z).Min();
            var maxZ = vertices.Select(v => v.Z).Max();
            Depth = maxZ - minZ;
        }

        public Box(int entityID, float width, float height, float depth) : base(entityID)
        {
            Width = width;
            Height = height;
            Depth = depth;
        }

        public override Shape3D Duplicate(int entityID) => new Box(entityID, Width, Height, Depth);

        public override IPartition ToPartition(Vector3 position)
        {
            var min = new Vector3(position.X - Width / 2.0f, position.Y - Height / 2.0f, position.Z - Depth / 2.0f);
            var max = new Vector3(position.X + Width / 2.0f, position.Y + Height / 2.0f, position.Z + Depth / 2.0f);

            return new Oct(min, max);
        }

        public override Vector3 GetFurthestPointInDirection(Vector3 direction)
        {
            // Check against the X-face
            var x = Math.Sign(direction.X) * (Width / 2.0f);
            var tx = x / direction.X;
            var xPenetration = new Vector3()
            {
                X = x,
                Y = direction.Y * tx,
                Z = direction.Z * tx
            };

            // Check against the Y-face
            var y = Math.Sign(direction.Y) * (Height / 2.0f);
            var ty = y / direction.Y;
            var yPenetration = new Vector3()
            {
                X = direction.X * ty,
                Y = y,
                Z = direction.Z * ty
            };

            // Check against the Z-face
            var z = Math.Sign(direction.Z) * (Depth / 2.0f);
            var tz = z / direction.Z;
            var zPenetration = new Vector3()
            {
                X = direction.X * tz,
                Y = direction.Y * tz,
                Z = z
            };

            // Ensure that we take NaN values into consideration
            if (direction.X != 0.0f
                && (direction.Y == 0.0f || xPenetration.LengthSquared < yPenetration.LengthSquared)
                && (direction.Z == 0.0f || xPenetration.LengthSquared < zPenetration.LengthSquared))
            {
                return xPenetration;
            }
            else if (direction.Y != 0.0f
                && (direction.Z == 0.0f || yPenetration.LengthSquared < zPenetration.LengthSquared))
            {
                return yPenetration;
            }
            else
            {
                return zPenetration;
            }
        }

        // TODO - Correct this (right now it calculates the same as 2D, but just tags on the Z position)
        /*public override Vector3 GetFurthestPoint(Vector3 position, Vector3 direction)
        {
            var xRatio = (Width / 2.0f) / direction.X;
            var yRatio = (Height / 2.0f) / direction.Y;

            var newX = direction.X * yRatio;
            var newY = direction.Y * xRatio;

            return (Math.Abs(newX) < Width / 2.0f)
                ? new Vector3(position.X - Center.X + newX, position.Y - Center.Y + direction.Y * yRatio, position.Z - Center.Z)
                : new Vector3(position.X - Center.X + direction.X * xRatio, position.Y - Center.Y + newY, position.Z - Center.Z);
        }*/

        /*public override bool CollidesWith(Vector3 position, Vector3 point) => point.X > position.X - Center.X - Width / 2.0f
            && point.X < position.X - Center.X + Width / 2.0f
            && point.Y > position.Y - Center.Y - Height / 2.0f
            && point.Y < position.Y - Center.Y + Height / 2.0f
            && point.Z > position.Z - Center.Z - Depth / 2.0f
            && point.Z < position.Z - Center.Z + Depth / 2.0f;*/

        public override float CalculateInertia(float mass) => mass * (Width * Width + Height * Height + Depth * Depth) / 12.0f;
    }
}
